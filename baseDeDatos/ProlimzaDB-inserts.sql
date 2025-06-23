-- =======================================
-- ============ INSERTS ==================
-- =======================================

-- provincia
INSERT INTO provincia (nombre) VALUES 
('San José'), ('Alajuela'), ('Cartago'), ('Heredia'), ('Guanacaste');

-- canton
INSERT INTO canton (nombre, idProvincia) VALUES 
('Central', 1), ('Desamparados', 1), ('Alajuela', 2), ('Orotina', 2), ('Cartago', 3);

-- distrito
INSERT INTO distrito (nombre, idCanton) VALUES 
('Carmen', 1), ('San Juan de Dios', 2), ('El Coyol', 3), ('Mastate', 4), ('Occidental', 5);

-- bodega
INSERT INTO bodega (nombre, detalleDireccion, idDistrito) VALUES 
('Bodega Central', 'Avenida 1, Calle 2', 1),
('Bodega Norte', 'Ruta 32, km 15', 2),
('Bodega Este', 'Zona Industrial', 3),
('Bodega Sur', 'Frente a Plaza Real', 4),
('Bodega Oeste', 'Al lado del hospital', 5);

-- materiaPrima
INSERT INTO materiaPrima (nombre, descripcion, sku, cantidad, fechaCaducidad, idBodega, stockMinimo) VALUES 
('Harina', 'Harina de trigo 000', 'MP001', 100, '2025-12-31', 1, 10),
('Azúcar', 'Azúcar refinada', 'MP002', 200, '2026-01-15', 2, 10),
('Sal', 'Sal marina', 'MP003', 50, '2027-03-01', 3, 20),
('Levadura', 'Levadura seca activa', 'MP004', 25, '2025-10-10', 4, 30),
('Manteca', 'Manteca vegetal', 'MP005', 80, '2025-11-05', 5, 10);

-- producto
INSERT INTO producto (nombreProducto, descripcion, sku, cantidad, fechaCaducidad, idBodega, stockMinimo) VALUES 
('Pan Integral', 'Pan saludable con fibra', 'P001', 30, '2025-06-15', 1, 10),
('Galletas', 'Galletas con chispas de chocolate', 'P002', 100, '2025-07-20', 2, 20),
('Pastel', 'Pastel de tres leches', 'P003', 10, '2025-05-01', 3, 30),
('Donas', 'Donas glaseadas', 'P004', 50, '2025-05-10', 4, 40),
('Queque', 'Queque de vainilla', 'P005', 20, '2025-06-10', 5, 10);

-- roles
INSERT INTO roles (nombre, descripcion) VALUES 
('Administrador', 'Acceso total al sistema'),
('Contador', 'Acceso a Modulo de inventario y productos'),
('Productor', 'Acceso a funcionalidades de produccion y recetas'),
('Operador', 'Puede registrar compras y ventas, sin permiso de modificacion');

-- usuario
INSERT INTO usuario (nombre, correo, contrasenaEncriptada, idRol) VALUES 
('Carlos López', 'carlos@prolimza.com', '1234hash', 1),
('María Gómez', 'maria@prolimza.com', 'abcdhash', 2),
('Pedro Ruiz', 'pedro@prolimza.com', 'efghhash', 3),
('Ana Vargas', 'ana@prolimza.com', 'ijklhash', 4),
('Luis Mora', 'luis@prolimza.com', 'mnophash', 4);

-- auditoria (actualizada)
INSERT INTO auditoria (log, tipoEvento, idUsuario) VALUES 
('Se modificó cantidad', 'Actualización', 1),
('Se eliminó producto', 'Eliminación', 2),
('Ingreso de materia prima', 'Creación', 3),
('Actualización de stock', 'Actualización', 4),
('Ingreso nuevo producto', 'Creación', 5);

-- receta
INSERT INTO receta (idProducto, nombre, descripcion) VALUES 
(1, 'Receta Pan Integral', 'Mezcla de ingredientes integrales'),
(2, 'Receta Galletas', 'Galletas con chips'),
(3, 'Receta Pastel', 'Pastel húmedo y suave'),
(4, 'Receta Donas', 'Donas dulces'),
(5, 'Receta Queque', 'Receta tradicional');

-- materiaReceta
INSERT INTO materiaReceta (idReceta, idMateriaPrima, cantidad) VALUES 
(1, 1, 20),
(2, 2, 10),
(3, 4, 5),
(4, 1, 10),
(5, 5, 7);

-- compra
INSERT INTO compra (fechaCompra, idUsuario, codigoIngreso, estado) VALUES 
('2025-04-01', 4, '001', 'Pendiente'),
('2025-04-02', 4, '001', 'Pendiente'),
('2025-04-03', 4, '001', 'Pendiente'),
('2025-04-04', 4, '001', 'Pendiente'),
('2025-04-05', 4, '001', 'Pendiente');

-- detalleCompraProducto
INSERT INTO detalleCompraProducto (idCompra, idProducto, cantidad) VALUES 
(1, 1, 10),
(2, 2, 20),
(3, 3, 5),
(4, 4, 8),
(5, 5, 7);

-- detalleCompraMateriaPrima
INSERT INTO detalleCompraMateriaPrima (idCompra, idMateriaPrima, cantidad) VALUES 
(1, 1, 30),
(2, 2, 40),
(3, 3, 15),
(4, 4, 12),
(5, 5, 18);

-- venta
INSERT INTO venta (fechaVenta, idUsuario, codigoIngreso) VALUES 
('2025-04-10', 3, '001'),
('2025-04-11', 3, '002'),
('2025-04-12', 3, '003'),
('2025-04-13', 3, '004'),
('2025-04-14', 3, '005');

-- detalleVenta
INSERT INTO detalleVenta (idVenta, idProducto, cantidad) VALUES 
(1, 1, 5),
(2, 2, 10),
(3, 3, 2),
(4, 4, 6),
(5, 5, 3);

-- estadoVenta
INSERT INTO estadoVenta (descripcion) VALUES 
('Pendiente'),
('Procesando'),
('Enviado'),
('Entregado'),
('Cancelado');

-- historialEstadoVenta
INSERT INTO historialEstadoVenta (idVenta, idEstadoVenta, fechaEstado) VALUES 
(1, 1, '2025-04-10'),
(2, 2, '2025-04-11'),
(3, 3, '2025-04-12'),
(4, 4, '2025-04-13'),
(5, 5, '2025-04-14');
