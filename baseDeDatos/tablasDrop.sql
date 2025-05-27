USE ProlimzaGestionInventarioDB;
GO

-- Eliminar tablas hijas primero (detalle, relaciones)
DROP TABLE IF EXISTS historialEstadoVenta;
DROP TABLE IF EXISTS detalleVenta;
DROP TABLE IF EXISTS detalleCompraProducto;
DROP TABLE IF EXISTS detalleCompraMateriaPrima;
DROP TABLE IF EXISTS materiaReceta;

-- Luego eliminar tablas intermedias/padres con dependencias
DROP TABLE IF EXISTS estadoVenta;
DROP TABLE IF EXISTS venta;
DROP TABLE IF EXISTS compra;
DROP TABLE IF EXISTS receta;
DROP TABLE IF EXISTS producto;
DROP TABLE IF EXISTS materiaPrima;

-- Eliminar usuarios, roles
DROP TABLE IF EXISTS usuario;
DROP TABLE IF EXISTS roles;

-- Eliminar tablas de ubicación
DROP TABLE IF EXISTS bodega;
DROP TABLE IF EXISTS distrito;
DROP TABLE IF EXISTS canton;
DROP TABLE IF EXISTS provincia;

-- Eliminar tabla de auditoría (independiente)
DROP TABLE IF EXISTS auditoria;
