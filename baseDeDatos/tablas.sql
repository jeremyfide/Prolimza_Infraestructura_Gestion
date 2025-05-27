
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'ProlimzaGestionInventarioDB')
BEGIN
    CREATE DATABASE ProlimzaGestionInventarioDB;
END;
GO


USE ProlimzaGestionInventarioDB;
GO

-- Tabla: provincia
CREATE TABLE provincia (
    idProvincia INT PRIMARY KEY,
    nombre VARCHAR(30)
);

-- Tabla: canton
CREATE TABLE canton (
    idCanton INT PRIMARY KEY,
    nombre VARCHAR(40),
    idProvincia INT FOREIGN KEY REFERENCES provincia(idProvincia)
);

-- Tabla: distrito
CREATE TABLE distrito (
    idDistrito INT PRIMARY KEY,
    nombre VARCHAR(60),
    idCanton INT FOREIGN KEY REFERENCES canton(idCanton)
);

-- Tabla: bodega
CREATE TABLE bodega (
    idBodega INT PRIMARY KEY,
    nombre VARCHAR(60),
    detalleDireccion VARCHAR(256),
    idDistrito INT FOREIGN KEY REFERENCES distrito(idDistrito)
);

-- Tabla: materiaPrima
CREATE TABLE materiaPrima (
    idMateriaPrima INT PRIMARY KEY,
    nombre VARCHAR(60),
    descripcion VARCHAR(256),
    sku VARCHAR(10),
    cantidad INT,
    fechaCaducidad DATETIME,
    idBodega INT FOREIGN KEY REFERENCES bodega(idBodega)
);

-- Tabla: producto
CREATE TABLE producto (
    idProducto INT PRIMARY KEY,
    nombreProducto VARCHAR(60),
    descripcion VARCHAR(256),
    sku VARCHAR(10),
    cantidad INT,
    fechaCaducidad DATETIME,
    bodega_idBodega INT FOREIGN KEY REFERENCES bodega(idBodega)
);

-- Tabla: receta
CREATE TABLE receta (
    idReceta INT PRIMARY KEY,
    idProducto INT FOREIGN KEY REFERENCES producto(idProducto),
    nombre VARCHAR(60),
    descripcion VARCHAR(256)
);

-- Tabla: materiaReceta
CREATE TABLE materiaReceta (
    idReceta INT FOREIGN KEY REFERENCES receta(idReceta),
    idMateriaPrima INT FOREIGN KEY REFERENCES materiaPrima(idMateriaPrima),
    cantidad INT,
    PRIMARY KEY (idReceta, idMateriaPrima)
);
-- Tabla: roles
CREATE TABLE roles (
    idRol INT PRIMARY KEY,
    nombre VARCHAR(60),
    descripcion VARCHAR(256)
);

-- Tabla: usuario
CREATE TABLE usuario (
    idUsuario INT PRIMARY KEY,
    nombre VARCHAR(60),
    correo VARCHAR(60),
    contrasenaEncryptada VARCHAR(512),
    idRol INT FOREIGN KEY REFERENCES roles(idRol)
);

-- Tabla: compra
CREATE TABLE compra (
    idCompra INT PRIMARY KEY,
    fechaCompra DATETIME,
    idUsuario INT FOREIGN KEY REFERENCES usuario(idUsuario)
);

-- Tabla: detalleCompraProducto
CREATE TABLE detalleCompraProducto (
    idCompra INT,
    idProducto INT,
    cantidad INT,
    PRIMARY KEY (idCompra, idProducto),
    FOREIGN KEY (idCompra) REFERENCES compra(idCompra),
    FOREIGN KEY (idProducto) REFERENCES producto(idProducto)
);

-- Tabla: detalleCompraMateriaPrima
CREATE TABLE detalleCompraMateriaPrima (
    idMateriaPrima INT,
    idCompra INT,
    cantidad INT,
    PRIMARY KEY (idMateriaPrima, idCompra),
    FOREIGN KEY (idMateriaPrima) REFERENCES materiaPrima(idMateriaPrima),
    FOREIGN KEY (idCompra) REFERENCES compra(idCompra)
);

-- Tabla: venta
CREATE TABLE venta (
    idVenta INT PRIMARY KEY,
    fechaVenta DATETIME,
    idUsuario INT FOREIGN KEY REFERENCES usuario(idUsuario)
);

-- Tabla: detalleVenta
CREATE TABLE detalleVenta (
    idVenta INT,
    idProducto INT,
    cantidad INT,
    PRIMARY KEY (idVenta, idProducto),
    FOREIGN KEY (idVenta) REFERENCES venta(idVenta),
    FOREIGN KEY (idProducto) REFERENCES producto(idProducto)
);

-- Tabla: estadoVenta
CREATE TABLE estadoVenta (
    idEstadoVenta INT PRIMARY KEY,
    descripcion VARCHAR(60)
);

-- Tabla: historialEstadoVenta
CREATE TABLE historialEstadoVenta (
    idEstadoVenta INT,
    idVenta INT,
    fechaEstado DATETIME,
    PRIMARY KEY (idEstadoVenta, idVenta),
    FOREIGN KEY (idEstadoVenta) REFERENCES estadoVenta(idEstadoVenta),
    FOREIGN KEY (idVenta) REFERENCES venta(idVenta)
);

-- Tabla: auditoria
CREATE TABLE auditoria (
    idAuditoria INT PRIMARY KEY,
    log NVARCHAR(1000),
    tipoEvento VARCHAR(60),
    categoriaEvento VARCHAR(60),
    modulo VARCHAR(60),
    fechaLog DATETIME,
    idUsuario INT FOREIGN KEY REFERENCES usuario(idUsuario)
);
