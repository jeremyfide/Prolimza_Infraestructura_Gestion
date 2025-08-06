using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Prolimza.Models;
using Microsoft.AspNetCore.SignalR;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSignalR();

builder.Services.AddDbContext<ApplicationDbContext>(op =>
{
    op.UseSqlServer(builder.Configuration.GetConnectionString("DB_service"));
});

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Auth/Login";
        options.AccessDeniedPath = "/Auth/AccessDenied";
    });

builder.Services.AddAuthorization();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});


builder.Services.AddHostedService<VerificadorStockService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}

app.UseExceptionHandler("/Home/Error");

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseSession();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapGet("/api/productos", async (ApplicationDbContext db) =>
{
    var productos = await db.Productos
        .Select(p => new
        {         
            p.NombreProducto

        })
        .ToListAsync();

    return Results.Ok(productos);
});

app.MapGet("/api/productos/{id:int}", async (int id, ApplicationDbContext db) =>
{
    var producto = await db.Productos
        .Where(p => p.IdProducto == id)
        .Select(p => new
        {
            p.NombreProducto,
            p.Descripcion,
            p.Cantidad
        })
        .FirstOrDefaultAsync();

    return producto is not null ? Results.Ok(producto) : Results.NotFound();
});


app.MapGet("/api/cadena_produccion/sku/{sku}", async (
    string sku,
    ApplicationDbContext db,
    IHubContext<ProductoHub> hubContext) =>
{
    if (string.IsNullOrWhiteSpace(sku))
        return Results.BadRequest(new { mensaje = "El SKU es requerido." });

    var producto = await db.Productos
        .Include(p => p.Bodega)
        .FirstOrDefaultAsync(p => p.Sku == sku);

    if (producto == null)
        return Results.NotFound(new { existe = false, mensaje = "Producto no encontrado." });

    // Aumentar la cantidad del producto
    await db.Database.ExecuteSqlRawAsync("EXEC AumentarCantidadProductoPorSKU @p0", sku);
    await db.Entry(producto).ReloadAsync();

    int idRegistro = 0;

    // Parámetro OUTPUT para el procedimiento ConsumirMateriaPrimaPorSKUProducto
    var resultadoParam = new Microsoft.Data.SqlClient.SqlParameter
    {
        ParameterName = "@resultado",
        SqlDbType = System.Data.SqlDbType.Int,
        Direction = System.Data.ParameterDirection.Output
    };

    await db.Database.ExecuteSqlRawAsync(
        "EXEC ConsumirMateriaPrimaPorSKUProducto @sku, @resultado OUT",
        new Microsoft.Data.SqlClient.SqlParameter("@sku", sku),
        resultadoParam
    );

    int resultado = (int)(resultadoParam.Value ?? -99);

    if (resultado != 1)
    {
        // Revertir aumento
        await db.Database.ExecuteSqlRawAsync("UPDATE producto SET cantidad = cantidad - 1 WHERE sku = @p0", sku);
        await db.Entry(producto).ReloadAsync();

        string mensajeError = resultado switch
        {
            -1 => "Producto no encontrado.",
            -2 => "El producto no tiene receta asociada.",
            -3 => "No hay suficiente stock de materia prima.",
            _ => "Error desconocido."
        };

        // Registrar fallo en registroCadenaProduccion
        await db.Database.ExecuteSqlInterpolatedAsync($@"
            INSERT INTO registroCadenaProduccion 
            (sku, idProducto, nombreProducto, cantidadFinal, exito, mensaje)
            VALUES ({sku}, {producto.IdProducto}, {producto.NombreProducto}, {producto.Cantidad}, 0, {mensajeError});
        ");

        return Results.BadRequest(new { existe = true, mensaje = mensajeError });
    }

    // Registrar éxito en registroCadenaProduccion
    await db.Database.ExecuteSqlInterpolatedAsync($@"
        INSERT INTO registroCadenaProduccion 
        (sku, idProducto, nombreProducto, cantidadFinal, exito, mensaje)
        VALUES ({sku}, {producto.IdProducto}, {producto.NombreProducto}, {producto.Cantidad}, 1, {"Proceso exitoso"});
    ");

    // Obtener el último IdRegistro insertado
    idRegistro = await db.RegistroCadenaProduccion
        .OrderByDescending(r => r.IdRegistro)
        .Select(r => r.IdRegistro)
        .FirstOrDefaultAsync();

    // Obtener detalles de materias primas consumidas
    var detalles = await (
        from mr in db.MateriasReceta
        join r in db.Recetas on mr.IdReceta equals r.IdReceta
        join mp in db.MateriasPrimas on mr.IdMateriaPrima equals mp.IdMateriaPrima
        where r.IdProducto == producto.IdProducto
        select new
        {
            mp.IdMateriaPrima,
            mp.Nombre,
            mr.Cantidad
        }
    ).ToListAsync();

    // Insertar detalles en detalleRegistroMateriaPrima
    foreach (var item in detalles)
    {
        await db.Database.ExecuteSqlInterpolatedAsync($@"
            INSERT INTO detalleRegistroMateriaPrima 
            (idRegistro, idMateriaPrima, nombreMateriaPrima, cantidadConsumida)
            VALUES ({idRegistro}, {item.IdMateriaPrima}, {item.Nombre}, {item.Cantidad});
        ");
    }

    var productoData = new
    {
        producto.IdProducto,
        producto.NombreProducto,
        producto.Descripcion,
        producto.Sku,
        producto.Cantidad,
        producto.FechaCaducidad,
        producto.IdBodega,
        producto.stockMinimo,
        bodega = producto.Bodega != null ? new
        {
            producto.Bodega.IdBodega,
            producto.Bodega.Nombre,
            producto.Bodega.DetalleDireccion
        } : null
    };

    await hubContext.Clients.All.SendAsync("ProductoConsultado", productoData);

    return Results.Ok(new
    {
        existe = true,
        producto = productoData,
        mensaje = "Producto aumentado y materia prima consumida correctamente."
    });
});







app.MapHub<ProductoHub>("/productoHub");


app.Run();
