using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Prolimza.Migrations
{
    /// <inheritdoc />
    public partial class InitDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "estadoVenta",
                columns: table => new
                {
                    IdEstadoVenta = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_estadoVenta", x => x.IdEstadoVenta);
                });

            migrationBuilder.CreateTable(
                name: "provincia",
                columns: table => new
                {
                    IdProvincia = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_provincia", x => x.IdProvincia);
                });

            migrationBuilder.CreateTable(
                name: "roles",
                columns: table => new
                {
                    IdRol = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_roles", x => x.IdRol);
                });

            migrationBuilder.CreateTable(
                name: "canton",
                columns: table => new
                {
                    IdCanton = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IdProvincia = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_canton", x => x.IdCanton);
                    table.ForeignKey(
                        name: "FK_canton_provincia_IdProvincia",
                        column: x => x.IdProvincia,
                        principalTable: "provincia",
                        principalColumn: "IdProvincia",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "usuario",
                columns: table => new
                {
                    IdUsuario = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Correo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContrasenaEncriptada = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IdRol = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_usuario", x => x.IdUsuario);
                    table.ForeignKey(
                        name: "FK_usuario_roles_IdRol",
                        column: x => x.IdRol,
                        principalTable: "roles",
                        principalColumn: "IdRol",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "distrito",
                columns: table => new
                {
                    IdDistrito = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IdCanton = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_distrito", x => x.IdDistrito);
                    table.ForeignKey(
                        name: "FK_distrito_canton_IdCanton",
                        column: x => x.IdCanton,
                        principalTable: "canton",
                        principalColumn: "IdCanton",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "compra",
                columns: table => new
                {
                    IdCompra = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FechaCompra = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IdUsuario = table.Column<int>(type: "int", nullable: false),
                    UsuarioIdUsuario = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_compra", x => x.IdCompra);
                    table.ForeignKey(
                        name: "FK_compra_usuario_UsuarioIdUsuario",
                        column: x => x.UsuarioIdUsuario,
                        principalTable: "usuario",
                        principalColumn: "IdUsuario");
                });

            migrationBuilder.CreateTable(
                name: "venta",
                columns: table => new
                {
                    IdVenta = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FechaVenta = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IdUsuario = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_venta", x => x.IdVenta);
                    table.ForeignKey(
                        name: "FK_venta_usuario_IdUsuario",
                        column: x => x.IdUsuario,
                        principalTable: "usuario",
                        principalColumn: "IdUsuario",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "bodega",
                columns: table => new
                {
                    IdBodega = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DetalleDireccion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IdDistrito = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bodega", x => x.IdBodega);
                    table.ForeignKey(
                        name: "FK_bodega_distrito_IdDistrito",
                        column: x => x.IdDistrito,
                        principalTable: "distrito",
                        principalColumn: "IdDistrito",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "historialEstadoVenta",
                columns: table => new
                {
                    IdHistorialEstadoVenta = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdVenta = table.Column<int>(type: "int", nullable: false),
                    VentaIdVenta = table.Column<int>(type: "int", nullable: true),
                    IdEstadoVenta = table.Column<int>(type: "int", nullable: false),
                    EstadoVentaIdEstadoVenta = table.Column<int>(type: "int", nullable: true),
                    FechaEstado = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_historialEstadoVenta", x => x.IdHistorialEstadoVenta);
                    table.ForeignKey(
                        name: "FK_historialEstadoVenta_estadoVenta_EstadoVentaIdEstadoVenta",
                        column: x => x.EstadoVentaIdEstadoVenta,
                        principalTable: "estadoVenta",
                        principalColumn: "IdEstadoVenta");
                    table.ForeignKey(
                        name: "FK_historialEstadoVenta_venta_VentaIdVenta",
                        column: x => x.VentaIdVenta,
                        principalTable: "venta",
                        principalColumn: "IdVenta");
                });

            migrationBuilder.CreateTable(
                name: "materiaPrima",
                columns: table => new
                {
                    IdMateriaPrima = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Sku = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Cantidad = table.Column<int>(type: "int", nullable: false),
                    FechaCaducidad = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IdBodega = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_materiaPrima", x => x.IdMateriaPrima);
                    table.ForeignKey(
                        name: "FK_materiaPrima_bodega_IdBodega",
                        column: x => x.IdBodega,
                        principalTable: "bodega",
                        principalColumn: "IdBodega",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "producto",
                columns: table => new
                {
                    IdProducto = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreProducto = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Sku = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Cantidad = table.Column<int>(type: "int", nullable: false),
                    FechaCaducidad = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IdBodega = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_producto", x => x.IdProducto);
                    table.ForeignKey(
                        name: "FK_producto_bodega_IdBodega",
                        column: x => x.IdBodega,
                        principalTable: "bodega",
                        principalColumn: "IdBodega",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "detalleCompraMateriaPrima",
                columns: table => new
                {
                    IdCompra = table.Column<int>(type: "int", nullable: false),
                    IdMateriaPrima = table.Column<int>(type: "int", nullable: false),
                    CompraIdCompra = table.Column<int>(type: "int", nullable: false),
                    MateriaPrimaIdMateriaPrima = table.Column<int>(type: "int", nullable: true),
                    Cantidad = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_detalleCompraMateriaPrima", x => new { x.IdCompra, x.IdMateriaPrima });
                    table.ForeignKey(
                        name: "FK_detalleCompraMateriaPrima_compra_CompraIdCompra",
                        column: x => x.CompraIdCompra,
                        principalTable: "compra",
                        principalColumn: "IdCompra",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_detalleCompraMateriaPrima_materiaPrima_MateriaPrimaIdMateriaPrima",
                        column: x => x.MateriaPrimaIdMateriaPrima,
                        principalTable: "materiaPrima",
                        principalColumn: "IdMateriaPrima");
                });

            migrationBuilder.CreateTable(
                name: "auditoria",
                columns: table => new
                {
                    IdAuditoria = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Log = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TipoEvento = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IdProducto = table.Column<int>(type: "int", nullable: true),
                    IdMateriaPrima = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_auditoria", x => x.IdAuditoria);
                    table.ForeignKey(
                        name: "FK_auditoria_materiaPrima_IdMateriaPrima",
                        column: x => x.IdMateriaPrima,
                        principalTable: "materiaPrima",
                        principalColumn: "IdMateriaPrima");
                    table.ForeignKey(
                        name: "FK_auditoria_producto_IdProducto",
                        column: x => x.IdProducto,
                        principalTable: "producto",
                        principalColumn: "IdProducto");
                });

            migrationBuilder.CreateTable(
                name: "detalleCompraProducto",
                columns: table => new
                {
                    IdCompra = table.Column<int>(type: "int", nullable: false),
                    IdProducto = table.Column<int>(type: "int", nullable: false),
                    CompraIdCompra = table.Column<int>(type: "int", nullable: true),
                    ProductoIdProducto = table.Column<int>(type: "int", nullable: true),
                    Cantidad = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_detalleCompraProducto", x => new { x.IdCompra, x.IdProducto });
                    table.ForeignKey(
                        name: "FK_detalleCompraProducto_compra_CompraIdCompra",
                        column: x => x.CompraIdCompra,
                        principalTable: "compra",
                        principalColumn: "IdCompra");
                    table.ForeignKey(
                        name: "FK_detalleCompraProducto_producto_ProductoIdProducto",
                        column: x => x.ProductoIdProducto,
                        principalTable: "producto",
                        principalColumn: "IdProducto");
                });

            migrationBuilder.CreateTable(
                name: "detalleVenta",
                columns: table => new
                {
                    IdVenta = table.Column<int>(type: "int", nullable: false),
                    IdProducto = table.Column<int>(type: "int", nullable: false),
                    Cantidad = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_detalleVenta", x => new { x.IdVenta, x.IdProducto });
                    table.ForeignKey(
                        name: "FK_detalleVenta_producto_IdProducto",
                        column: x => x.IdProducto,
                        principalTable: "producto",
                        principalColumn: "IdProducto",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_detalleVenta_venta_IdVenta",
                        column: x => x.IdVenta,
                        principalTable: "venta",
                        principalColumn: "IdVenta",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "receta",
                columns: table => new
                {
                    IdReceta = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdProducto = table.Column<int>(type: "int", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_receta", x => x.IdReceta);
                    table.ForeignKey(
                        name: "FK_receta_producto_IdProducto",
                        column: x => x.IdProducto,
                        principalTable: "producto",
                        principalColumn: "IdProducto",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "materiaReceta",
                columns: table => new
                {
                    IdReceta = table.Column<int>(type: "int", nullable: false),
                    IdMateriaPrima = table.Column<int>(type: "int", nullable: false),
                    Cantidad = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_materiaReceta", x => new { x.IdReceta, x.IdMateriaPrima });
                    table.ForeignKey(
                        name: "FK_materiaReceta_materiaPrima_IdMateriaPrima",
                        column: x => x.IdMateriaPrima,
                        principalTable: "materiaPrima",
                        principalColumn: "IdMateriaPrima",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_materiaReceta_receta_IdReceta",
                        column: x => x.IdReceta,
                        principalTable: "receta",
                        principalColumn: "IdReceta",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_auditoria_IdMateriaPrima",
                table: "auditoria",
                column: "IdMateriaPrima");

            migrationBuilder.CreateIndex(
                name: "IX_auditoria_IdProducto",
                table: "auditoria",
                column: "IdProducto");

            migrationBuilder.CreateIndex(
                name: "IX_bodega_IdDistrito",
                table: "bodega",
                column: "IdDistrito");

            migrationBuilder.CreateIndex(
                name: "IX_canton_IdProvincia",
                table: "canton",
                column: "IdProvincia");

            migrationBuilder.CreateIndex(
                name: "IX_compra_UsuarioIdUsuario",
                table: "compra",
                column: "UsuarioIdUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_detalleCompraMateriaPrima_CompraIdCompra",
                table: "detalleCompraMateriaPrima",
                column: "CompraIdCompra");

            migrationBuilder.CreateIndex(
                name: "IX_detalleCompraMateriaPrima_MateriaPrimaIdMateriaPrima",
                table: "detalleCompraMateriaPrima",
                column: "MateriaPrimaIdMateriaPrima");

            migrationBuilder.CreateIndex(
                name: "IX_detalleCompraProducto_CompraIdCompra",
                table: "detalleCompraProducto",
                column: "CompraIdCompra");

            migrationBuilder.CreateIndex(
                name: "IX_detalleCompraProducto_ProductoIdProducto",
                table: "detalleCompraProducto",
                column: "ProductoIdProducto");

            migrationBuilder.CreateIndex(
                name: "IX_detalleVenta_IdProducto",
                table: "detalleVenta",
                column: "IdProducto");

            migrationBuilder.CreateIndex(
                name: "IX_distrito_IdCanton",
                table: "distrito",
                column: "IdCanton");

            migrationBuilder.CreateIndex(
                name: "IX_historialEstadoVenta_EstadoVentaIdEstadoVenta",
                table: "historialEstadoVenta",
                column: "EstadoVentaIdEstadoVenta");

            migrationBuilder.CreateIndex(
                name: "IX_historialEstadoVenta_VentaIdVenta",
                table: "historialEstadoVenta",
                column: "VentaIdVenta");

            migrationBuilder.CreateIndex(
                name: "IX_materiaPrima_IdBodega",
                table: "materiaPrima",
                column: "IdBodega");

            migrationBuilder.CreateIndex(
                name: "IX_materiaReceta_IdMateriaPrima",
                table: "materiaReceta",
                column: "IdMateriaPrima");

            migrationBuilder.CreateIndex(
                name: "IX_producto_IdBodega",
                table: "producto",
                column: "IdBodega");

            migrationBuilder.CreateIndex(
                name: "IX_receta_IdProducto",
                table: "receta",
                column: "IdProducto");

            migrationBuilder.CreateIndex(
                name: "IX_usuario_IdRol",
                table: "usuario",
                column: "IdRol");

            migrationBuilder.CreateIndex(
                name: "IX_venta_IdUsuario",
                table: "venta",
                column: "IdUsuario");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "auditoria");

            migrationBuilder.DropTable(
                name: "detalleCompraMateriaPrima");

            migrationBuilder.DropTable(
                name: "detalleCompraProducto");

            migrationBuilder.DropTable(
                name: "detalleVenta");

            migrationBuilder.DropTable(
                name: "historialEstadoVenta");

            migrationBuilder.DropTable(
                name: "materiaReceta");

            migrationBuilder.DropTable(
                name: "compra");

            migrationBuilder.DropTable(
                name: "estadoVenta");

            migrationBuilder.DropTable(
                name: "venta");

            migrationBuilder.DropTable(
                name: "materiaPrima");

            migrationBuilder.DropTable(
                name: "receta");

            migrationBuilder.DropTable(
                name: "usuario");

            migrationBuilder.DropTable(
                name: "producto");

            migrationBuilder.DropTable(
                name: "roles");

            migrationBuilder.DropTable(
                name: "bodega");

            migrationBuilder.DropTable(
                name: "distrito");

            migrationBuilder.DropTable(
                name: "canton");

            migrationBuilder.DropTable(
                name: "provincia");
        }
    }
}
