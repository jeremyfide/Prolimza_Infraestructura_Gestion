-- Crear Base de Datos
--CREATE DATABASE ProlimzaDB;
--GO

USE ProlimzaDB;
GO

-- Tabla: provincia
CREATE TABLE provincia (
    idProvincia INT PRIMARY KEY IDENTITY(1,1),
    nombre VARCHAR(100) NOT NULL
);

-- Tabla: canton
CREATE TABLE canton (
    idCanton INT PRIMARY KEY IDENTITY(1,1),
    nombre VARCHAR(100) NOT NULL,
    idProvincia INT FOREIGN KEY REFERENCES provincia(idProvincia)
);

-- Tabla: distrito
CREATE TABLE distrito (
    idDistrito INT PRIMARY KEY IDENTITY(1,1),
    nombre VARCHAR(100) NOT NULL,
    idCanton INT FOREIGN KEY REFERENCES canton(idCanton)
);

-- Tabla: bodega
CREATE TABLE bodega (
    idBodega INT PRIMARY KEY IDENTITY(1,1),
    nombre VARCHAR(100) NOT NULL,
    detalleDireccion VARCHAR(255),
    idDistrito INT FOREIGN KEY REFERENCES distrito(idDistrito)
);

-- Tabla: materiaPrima
CREATE TABLE materiaPrima (
    idMateriaPrima INT PRIMARY KEY IDENTITY(1,1),
    nombre VARCHAR(100) NOT NULL,
    descripcion VARCHAR(255),
    sku VARCHAR(50) UNIQUE,
    cantidad INT NOT NULL,
    fechaCaducidad DATETIME,
	stockMinimo INT DEFAULT 10,
    idBodega INT FOREIGN KEY REFERENCES bodega(idBodega)
);

-- Tabla: producto
CREATE TABLE producto (
    idProducto INT PRIMARY KEY IDENTITY(1,1),
    nombreProducto VARCHAR(100) NOT NULL,
    descripcion VARCHAR(255),
    sku VARCHAR(50) UNIQUE,
    cantidad INT NOT NULL,
    fechaCaducidad DATETIME,
	stockMinimo INT DEFAULT 10,
    idBodega INT FOREIGN KEY REFERENCES bodega(idBodega)
);

-- Tabla: roles
CREATE TABLE roles (
    idRol INT PRIMARY KEY IDENTITY(1,1),
    nombre VARCHAR(100) NOT NULL,
    descripcion VARCHAR(255)
);

-- Tabla: usuario
CREATE TABLE usuario (
    idUsuario INT PRIMARY KEY IDENTITY(1,1),
    nombre VARCHAR(100) NOT NULL,
    correo VARCHAR(100) UNIQUE NOT NULL,
    contrasenaEncriptada VARCHAR(255) NOT NULL,
    idRol INT FOREIGN KEY REFERENCES roles(idRol)
);

-- Tabla: auditoria (actualizada)
CREATE TABLE auditoria (
    idAuditoria INT PRIMARY KEY IDENTITY(1,1),
    log VARCHAR(255) NOT NULL,
    tipoEvento VARCHAR(100) NOT NULL,
    fechaEvento DATETIME NOT NULL DEFAULT GETDATE(),
    idUsuario INT NOT NULL FOREIGN KEY REFERENCES usuario(idUsuario)
);

-- Tabla: receta
CREATE TABLE receta (
    idReceta INT PRIMARY KEY IDENTITY(1,1),
    idProducto INT FOREIGN KEY REFERENCES producto(idProducto) ON DELETE CASCADE,
    nombre VARCHAR(100) NOT NULL,
    descripcion VARCHAR(255)
);

-- Tabla: materiaReceta
CREATE TABLE materiaReceta (
    idReceta INT FOREIGN KEY REFERENCES receta(idReceta) ON DELETE CASCADE,
    idMateriaPrima INT FOREIGN KEY REFERENCES materiaPrima(idMateriaPrima) ON DELETE CASCADE,
    cantidad INT NOT NULL,
    PRIMARY KEY (idReceta, idMateriaPrima)
);

-- Tabla: compra
CREATE TABLE compra (
    idCompra INT PRIMARY KEY IDENTITY(1,1),
    fechaCompra DATETIME NOT NULL,
	estado VARCHAR(20) NOT NULL DEFAULT 'Pendiente',
	codigoIngreso VARCHAR(50) NOT NULL DEFAULT 'Nulo',
    idUsuario INT FOREIGN KEY REFERENCES usuario(idUsuario)
);

-- Tabla: detalleCompraProducto
CREATE TABLE detalleCompraProducto (
    idCompra INT FOREIGN KEY REFERENCES compra(idCompra),
    idProducto INT FOREIGN KEY REFERENCES producto(idProducto),
    cantidad INT NOT NULL,
    PRIMARY KEY (idCompra, idProducto)
);

-- Tabla: detalleCompraMateriaPrima
CREATE TABLE detalleCompraMateriaPrima (
    idCompra INT FOREIGN KEY REFERENCES compra(idCompra),
    idMateriaPrima INT FOREIGN KEY REFERENCES materiaPrima(idMateriaPrima),
    cantidad INT NOT NULL,
    PRIMARY KEY (idCompra, idMateriaPrima)
);

-- Tabla: venta
CREATE TABLE venta (
    idVenta INT PRIMARY KEY IDENTITY(1,1),
    fechaVenta DATETIME NOT NULL,
	codigoIngreso VARCHAR(50) NOT NULL DEFAULT 'Nulo',
    idUsuario INT FOREIGN KEY REFERENCES usuario(idUsuario)
);

-- Tabla: detalleVenta
CREATE TABLE detalleVenta (
    idVenta INT FOREIGN KEY REFERENCES venta(idVenta),
    idProducto INT FOREIGN KEY REFERENCES producto(idProducto),
    cantidad INT NOT NULL,
    PRIMARY KEY (idVenta, idProducto)
);

-- Tabla: estadoVenta
CREATE TABLE estadoVenta (
    idEstadoVenta INT PRIMARY KEY IDENTITY(1,1),
    descripcion VARCHAR(255) NOT NULL
);

-- Tabla: historialEstadoVenta
CREATE TABLE historialEstadoVenta (
    idHistorialEstadoVenta INT PRIMARY KEY IDENTITY(1,1),
    idVenta INT FOREIGN KEY REFERENCES venta(idVenta),
    idEstadoVenta INT FOREIGN KEY REFERENCES estadoVenta(idEstadoVenta),
    fechaEstado DATETIME NOT NULL
);

-- Tabla: alerta
CREATE TABLE alerta (
    idAlerta INT PRIMARY KEY IDENTITY(1,1),
    tipo VARCHAR(50) NOT NULL,
    descripcion VARCHAR(255),
    fechaAlerta DATETIME NOT NULL DEFAULT GETDATE(),
    estado VARCHAR(50) NOT NULL DEFAULT 'Pendiente',
    idUsuario INT NULL,
    FOREIGN KEY (idUsuario) REFERENCES usuario(idUsuario)
);

-- Tabla: registroCadenaProduccion

CREATE TABLE registroCadenaProduccion (
    idRegistro INT PRIMARY KEY IDENTITY(1,1),
    sku VARCHAR(50) NOT NULL,
    idProducto INT NOT NULL,
    nombreProducto VARCHAR(100),
    cantidadFinal INT,
    exito BIT NOT NULL,
    mensaje VARCHAR(255),
    fechaRegistro DATETIME NOT NULL DEFAULT GETDATE(),
    idUsuario INT NULL, -- si en el futuro quieres enlazar con usuarios
    FOREIGN KEY (idProducto) REFERENCES producto(idProducto),
    FOREIGN KEY (idUsuario) REFERENCES usuario(idUsuario)
);

-- Tabla: registroCadenaProduccion

CREATE TABLE detalleRegistroMateriaPrima (
    idDetalle INT PRIMARY KEY IDENTITY(1,1),
    idRegistro INT NOT NULL,
    idMateriaPrima INT NOT NULL,
    nombreMateriaPrima VARCHAR(100),
    cantidadConsumida INT NOT NULL,
    FOREIGN KEY (idRegistro) REFERENCES registroCadenaProduccion(idRegistro),
    FOREIGN KEY (idMateriaPrima) REFERENCES materiaPrima(idMateriaPrima)
);
GO

---- STORED PROCEDURES ----

CREATE PROCEDURE AumentarCantidadProductoPorSKU
    @sku VARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (SELECT 1 FROM producto WHERE sku = @sku)
    BEGIN
        UPDATE producto
        SET cantidad = cantidad + 1
        WHERE sku = @sku;

        PRINT 'Cantidad actualizada correctamente.';
    END
    ELSE
    BEGIN
        PRINT 'Producto con ese SKU no existe.';
    END
END;
GO

------------------------------------

CREATE PROCEDURE ConsumirMateriaPrimaPorSKUProducto
    @sku VARCHAR(50),
    @resultado INT OUTPUT -- 1 = éxito, <0 = error
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @idProducto INT;

    SELECT @idProducto = idProducto FROM producto WHERE sku = @sku;

    IF @idProducto IS NULL
    BEGIN
        SET @resultado = -1; -- Producto no encontrado
        RETURN;
    END

    IF NOT EXISTS (SELECT 1 FROM receta WHERE idProducto = @idProducto)
    BEGIN
        SET @resultado = -2; -- Sin receta
        RETURN;
    END

    IF EXISTS (
        SELECT 1
        FROM materiaPrima mp
        INNER JOIN materiaReceta mr ON mp.idMateriaPrima = mr.idMateriaPrima
        INNER JOIN receta r ON r.idReceta = mr.idReceta
        WHERE r.idProducto = @idProducto
        AND mp.cantidad < mr.cantidad
    )
    BEGIN
        SET @resultado = -3; -- Sin stock suficiente
        RETURN;
    END

    UPDATE mp
    SET mp.cantidad = mp.cantidad - mr.cantidad
    FROM materiaPrima mp
    INNER JOIN materiaReceta mr ON mp.idMateriaPrima = mr.idMateriaPrima
    INNER JOIN receta r ON r.idReceta = mr.idReceta
    WHERE r.idProducto = @idProducto;

    SET @resultado = 1; -- Éxito
END;
GO
