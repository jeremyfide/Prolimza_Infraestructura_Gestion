INSERT INTO ProlimzaGestionInventarioDB.dbo.bodega VALUES (1, 'Bodega Central', 'Ubicada 100m sur de PriceSmart, complejo de bodegas Galeón, bodega 7.', 492);
--Fijarse en estos inserts, hay que poner datos con sentido.
INSERT INTO ProlimzaGestionInventarioDB.dbo.materiaPrima VALUES 
(1, 'Nombre1', 'Desc1', 'SKU001', 100, '2026-01-01', 1),
(2, 'Nombre2', 'Desc2', 'SKU002', 150, '2026-02-01', 1),
(3, 'Nombre3', 'Desc3', 'SKU003', 200, '2025-12-01', 1),
(4, 'Nombre4', 'Desc4', 'SKU004', 120, '2027-01-01', 1),
(5, 'Nombre5', 'Desc5', 'SKU005', 180, '2025-10-01', 1);
INSERT INTO ProlimzaGestionInventarioDB.dbo.producto VALUES 
(1, 'Producto1', 'Desc1', 'P001', 50, '2025-06-01', 1),
(2, 'Producto2', 'Desc2', 'P002', 80, '2025-07-15', 1),
(3, 'Producto3', 'Desc3', 'P003', 60, '2025-05-30', 1),
(4, 'Producto4', 'Desc4', 'P004', 90, '2025-08-01', 1),
(5, 'Producto5', 'Desc5', 'P005', 70, '2025-09-10', 1);
INSERT INTO ProlimzaGestionInventarioDB.dbo.receta VALUES 
(1, 1, 'Receta1', 'Desc1'),
(2, 2, 'Receta2', 'Desc2'),
(3, 3, 'Receta3', 'Desc3'),
(4, 4, 'Receta4', 'Desc4'),
(5, 5, 'Receta5', 'Desc5');
INSERT INTO ProlimzaGestionInventarioDB.dbo.materiaReceta VALUES 
(1, 1, 20), (1, 2, 5), (2, 1, 10), (2, 2, 10), (5, 5, 15);
--EOF
INSERT INTO ProlimzaGestionInventarioDB.dbo.roles VALUES 
(1, 'Administrador', 'Accede a todas las funciones del sistema, incluyendo creación de usuarios y configuración general.'), 
(2, 'Contador', 'Tiene acceso a los módulos de inventario y productos, con capacidad para realizar ajustes y mantener datos actualizados.'), 
(3, 'Productor', 'Tiene acceso a las funcionalidades de producción y recetas.'), 
(4, 'Operador', 'Puede registrar compras y ventas, pero sin permisos de modificación avanzada o configuración.');
INSERT INTO ProlimzaGestionInventarioDB.dbo.usuario VALUES 
(1, 'Administrador', 'adminprolimza@gmail.com', 'enc123', 1),
(2, 'Contador', 'contadroprolimza@gmail.com', 'enc456', 2),
(3, 'Productor', 'productorprolimza@gmail.com', 'enc789', 3),
(4, 'Operador', 'operadorprolimza@gmail.com', 'enc321', 4);
INSERT INTO ProlimzaGestionInventarioDB.dbo.compra VALUES 
(1, '2025-01-01', 4), 
(2, '2025-01-02', 4), 
(3, '2025-01-03', 4), 
(4, '2025-01-04', 4), 
(5, '2025-01-05', 4);
INSERT INTO ProlimzaGestionInventarioDB.dbo.detalleCompraProducto VALUES 
(1, 1, 10), (2, 2, 15), (3, 3, 20), (4, 4, 25), (5, 5, 30);
INSERT INTO ProlimzaGestionInventarioDB.dbo.detalleCompraMateriaPrima VALUES 
(1, 1, 50), (2, 2, 60), (3, 3, 70), (4, 4, 80), (5, 5, 90);
INSERT INTO ProlimzaGestionInventarioDB.dbo.venta VALUES 
(1, '2025-03-01', 4), 
(2, '2025-03-02', 4), 
(3, '2025-03-03', 4), 
(4, '2025-03-04', 4), 
(5, '2025-03-05', 4);
INSERT INTO ProlimzaGestionInventarioDB.dbo.detalleVenta VALUES 
(1, 1, 5), (2, 2, 10), (3, 3, 15), (4, 4, 20), (5, 5, 25);
INSERT INTO ProlimzaGestionInventarioDB.dbo.estadoVenta VALUES 
(1, 'Registrada'), (2, 'Procesada'), (3, 'Enviada'), (4, 'Entregada'), (5, 'Cancelada'), (6, 'Devuelta');
INSERT INTO historialEstadoVenta VALUES 
(1, 1, '2025-03-01 08:00:00'),
(2, 1, '2025-03-01 10:00:00'),
(3, 1, '2025-03-01 12:00:00'),
(4, 1, '2025-03-01 15:00:00');
INSERT INTO historialEstadoVenta VALUES 
(1, 2, '2025-03-02 08:00:00'),
(2, 2, '2025-03-02 10:00:00'),
(3, 2, '2025-03-02 12:00:00'),
(4, 2, '2025-03-02 15:00:00');
INSERT INTO historialEstadoVenta VALUES 
(1, 3, '2025-03-03 08:00:00'),
(2, 3, '2025-03-03 10:00:00'),
(3, 3, '2025-03-03 12:00:00'),
(4, 3, '2025-03-03 15:00:00');
INSERT INTO historialEstadoVenta VALUES 
(1, 4, '2025-03-04 08:00:00'),
(5, 4, '2025-03-04 09:00:00');
INSERT INTO historialEstadoVenta VALUES 
(1, 5, '2025-03-05 08:00:00'),
(2, 5, '2025-03-05 10:00:00'),
(3, 5, '2025-03-05 12:00:00'),
(4, 5, '2025-03-05 15:00:00'),
(6, 5, '2025-03-06 09:00:00');