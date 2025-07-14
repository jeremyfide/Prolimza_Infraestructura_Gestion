-- Venta del 2024-01-01
INSERT INTO venta (fechaVenta, codigoIngreso, idUsuario)
VALUES ('2024-01-01', 'AI5047', 3);
DECLARE @idVenta INT = SCOPE_IDENTITY();
INSERT INTO detalleVenta (idVenta, idProducto, cantidad)
VALUES (@idVenta, 5, 14);
INSERT INTO historialEstadoVenta (idVenta, idEstadoVenta, fechaEstado)
VALUES (@idVenta, 5, '2024-01-01');
GO

-- Venta del 2024-01-02
INSERT INTO venta (fechaVenta, codigoIngreso, idUsuario)
VALUES ('2024-01-02', 'AI3907', 3);
DECLARE @idVenta INT = SCOPE_IDENTITY();
INSERT INTO detalleVenta (idVenta, idProducto, cantidad)
VALUES (@idVenta, 2, 10);
INSERT INTO historialEstadoVenta (idVenta, idEstadoVenta, fechaEstado)
VALUES (@idVenta, 3, '2024-01-02');
GO

-- Venta del 2024-01-03
INSERT INTO venta (fechaVenta, codigoIngreso, idUsuario)
VALUES ('2024-01-03', 'AI7875', 3);
DECLARE @idVenta INT = SCOPE_IDENTITY();
INSERT INTO detalleVenta (idVenta, idProducto, cantidad)
VALUES (@idVenta, 3, 19);
INSERT INTO historialEstadoVenta (idVenta, idEstadoVenta, fechaEstado)
VALUES (@idVenta, 5, '2024-01-03');
GO

-- Venta del 2024-01-04
INSERT INTO venta (fechaVenta, codigoIngreso, idUsuario)
VALUES ('2024-01-04', 'AI3767', 3);
DECLARE @idVenta INT = SCOPE_IDENTITY();
INSERT INTO detalleVenta (idVenta, idProducto, cantidad)
VALUES (@idVenta, 4, 13);
INSERT INTO historialEstadoVenta (idVenta, idEstadoVenta, fechaEstado)
VALUES (@idVenta, 3, '2024-01-04');
GO

-- Venta del 2024-01-31
INSERT INTO venta (fechaVenta, codigoIngreso, idUsuario)
VALUES ('2024-01-31', 'AI9205', 3);
DECLARE @idVenta INT = SCOPE_IDENTITY();
INSERT INTO detalleVenta (idVenta, idProducto, cantidad)
VALUES (@idVenta, 3, 8);
INSERT INTO historialEstadoVenta (idVenta, idEstadoVenta, fechaEstado)
VALUES (@idVenta, 2, '2024-01-31');
GO

-- Venta del 2024-02-01
INSERT INTO venta (fechaVenta, codigoIngreso, idUsuario)
VALUES ('2024-02-01', 'AI1797', 3);
DECLARE @idVenta INT = SCOPE_IDENTITY();
INSERT INTO detalleVenta (idVenta, idProducto, cantidad)
VALUES (@idVenta, 3, 3);
INSERT INTO historialEstadoVenta (idVenta, idEstadoVenta, fechaEstado)
VALUES (@idVenta, 4, '2024-02-01');
GO

-- Venta del 2024-02-02
INSERT INTO venta (fechaVenta, codigoIngreso, idUsuario)
VALUES ('2024-02-02', 'AI6954', 3);
DECLARE @idVenta INT = SCOPE_IDENTITY();
INSERT INTO detalleVenta (idVenta, idProducto, cantidad)
VALUES (@idVenta, 1, 3);
INSERT INTO historialEstadoVenta (idVenta, idEstadoVenta, fechaEstado)
VALUES (@idVenta, 4, '2024-02-02');
GO

-- Venta del 2024-02-03
INSERT INTO venta (fechaVenta, codigoIngreso, idUsuario)
VALUES ('2024-02-03', 'AI7138', 3);
DECLARE @idVenta INT = SCOPE_IDENTITY();
INSERT INTO detalleVenta (idVenta, idProducto, cantidad)
VALUES (@idVenta, 1, 1);
INSERT INTO historialEstadoVenta (idVenta, idEstadoVenta, fechaEstado)
VALUES (@idVenta, 2, '2024-02-03');
GO

-- Venta del 2024-03-01
INSERT INTO venta (fechaVenta, codigoIngreso, idUsuario)
VALUES ('2024-03-01', 'AI8331', 3);
DECLARE @idVenta INT = SCOPE_IDENTITY();
INSERT INTO detalleVenta (idVenta, idProducto, cantidad)
VALUES (@idVenta, 4, 9);
INSERT INTO historialEstadoVenta (idVenta, idEstadoVenta, fechaEstado)
VALUES (@idVenta, 5, '2024-03-01');
GO

-- Venta del 2024-03-02
INSERT INTO venta (fechaVenta, codigoIngreso, idUsuario)
VALUES ('2024-03-02', 'AI2346', 3);
DECLARE @idVenta INT = SCOPE_IDENTITY();
INSERT INTO detalleVenta (idVenta, idProducto, cantidad)
VALUES (@idVenta, 1, 5);
INSERT INTO historialEstadoVenta (idVenta, idEstadoVenta, fechaEstado)
VALUES (@idVenta, 1, '2024-03-02');
GO

-- Venta del 2024-03-03
INSERT INTO venta (fechaVenta, codigoIngreso, idUsuario)
VALUES ('2024-03-03', 'AI6317', 3);
DECLARE @idVenta INT = SCOPE_IDENTITY();
INSERT INTO detalleVenta (idVenta, idProducto, cantidad)
VALUES (@idVenta, 5, 5);
INSERT INTO historialEstadoVenta (idVenta, idEstadoVenta, fechaEstado)
VALUES (@idVenta, 4, '2024-03-03');
GO

-- Venta del 2024-03-04
INSERT INTO venta (fechaVenta, codigoIngreso, idUsuario)
VALUES ('2024-03-04', 'AI4443', 3);
DECLARE @idVenta INT = SCOPE_IDENTITY();
INSERT INTO detalleVenta (idVenta, idProducto, cantidad)
VALUES (@idVenta, 4, 14);
INSERT INTO historialEstadoVenta (idVenta, idEstadoVenta, fechaEstado)
VALUES (@idVenta, 3, '2024-03-04');
GO

-- Venta del 2024-03-31
INSERT INTO venta (fechaVenta, codigoIngreso, idUsuario)
VALUES ('2024-03-31', 'AI1501', 3);
DECLARE @idVenta INT = SCOPE_IDENTITY();
INSERT INTO detalleVenta (idVenta, idProducto, cantidad)
VALUES (@idVenta, 2, 11);
INSERT INTO historialEstadoVenta (idVenta, idEstadoVenta, fechaEstado)
VALUES (@idVenta, 5, '2024-03-31');
GO

-- Venta del 2024-04-01
INSERT INTO venta (fechaVenta, codigoIngreso, idUsuario)
VALUES ('2024-04-01', 'AI5761', 3);
DECLARE @idVenta INT = SCOPE_IDENTITY();
INSERT INTO detalleVenta (idVenta, idProducto, cantidad)
VALUES (@idVenta, 5, 4);
INSERT INTO historialEstadoVenta (idVenta, idEstadoVenta, fechaEstado)
VALUES (@idVenta, 3, '2024-04-01');
GO

-- Venta del 2024-04-02
INSERT INTO venta (fechaVenta, codigoIngreso, idUsuario)
VALUES ('2024-04-02', 'AI8016', 3);
DECLARE @idVenta INT = SCOPE_IDENTITY();
INSERT INTO detalleVenta (idVenta, idProducto, cantidad)
VALUES (@idVenta, 3, 3);
INSERT INTO historialEstadoVenta (idVenta, idEstadoVenta, fechaEstado)
VALUES (@idVenta, 5, '2024-04-02');
GO

-- Venta del 2024-04-03
INSERT INTO venta (fechaVenta, codigoIngreso, idUsuario)
VALUES ('2024-04-03', 'AI6024', 3);
DECLARE @idVenta INT = SCOPE_IDENTITY();
INSERT INTO detalleVenta (idVenta, idProducto, cantidad)
VALUES (@idVenta, 3, 16);
INSERT INTO historialEstadoVenta (idVenta, idEstadoVenta, fechaEstado)
VALUES (@idVenta, 4, '2024-04-03');
GO

-- Venta del 2024-04-30
INSERT INTO venta (fechaVenta, codigoIngreso, idUsuario)
VALUES ('2024-04-30', 'AI5667', 3);
DECLARE @idVenta INT = SCOPE_IDENTITY();
INSERT INTO detalleVenta (idVenta, idProducto, cantidad)
VALUES (@idVenta, 1, 7);
INSERT INTO historialEstadoVenta (idVenta, idEstadoVenta, fechaEstado)
VALUES (@idVenta, 3, '2024-04-30');
GO

-- Venta del 2024-05-01
INSERT INTO venta (fechaVenta, codigoIngreso, idUsuario)
VALUES ('2024-05-01', 'AI8982', 3);
DECLARE @idVenta INT = SCOPE_IDENTITY();
INSERT INTO detalleVenta (idVenta, idProducto, cantidad)
VALUES (@idVenta, 2, 9);
INSERT INTO historialEstadoVenta (idVenta, idEstadoVenta, fechaEstado)
VALUES (@idVenta, 1, '2024-05-01');
GO

-- Venta del 2024-05-02
INSERT INTO venta (fechaVenta, codigoIngreso, idUsuario)
VALUES ('2024-05-02', 'AI1973', 3);
DECLARE @idVenta INT = SCOPE_IDENTITY();
INSERT INTO detalleVenta (idVenta, idProducto, cantidad)
VALUES (@idVenta, 3, 4);
INSERT INTO historialEstadoVenta (idVenta, idEstadoVenta, fechaEstado)
VALUES (@idVenta, 2, '2024-05-02');
GO

-- Venta del 2024-05-03
INSERT INTO venta (fechaVenta, codigoIngreso, idUsuario)
VALUES ('2024-05-03', 'AI1050', 3);
DECLARE @idVenta INT = SCOPE_IDENTITY();
INSERT INTO detalleVenta (idVenta, idProducto, cantidad)
VALUES (@idVenta, 1, 9);
INSERT INTO historialEstadoVenta (idVenta, idEstadoVenta, fechaEstado)
VALUES (@idVenta, 2, '2024-05-03');
GO

-- Venta del 2024-05-30
INSERT INTO venta (fechaVenta, codigoIngreso, idUsuario)
VALUES ('2024-05-30', 'AI8500', 3);
DECLARE @idVenta INT = SCOPE_IDENTITY();
INSERT INTO detalleVenta (idVenta, idProducto, cantidad)
VALUES (@idVenta, 5, 19);
INSERT INTO historialEstadoVenta (idVenta, idEstadoVenta, fechaEstado)
VALUES (@idVenta, 2, '2024-05-30');
GO

-- Venta del 2024-05-31
INSERT INTO venta (fechaVenta, codigoIngreso, idUsuario)
VALUES ('2024-05-31', 'AI3267', 3);
DECLARE @idVenta INT = SCOPE_IDENTITY();
INSERT INTO detalleVenta (idVenta, idProducto, cantidad)
VALUES (@idVenta, 1, 10);
INSERT INTO historialEstadoVenta (idVenta, idEstadoVenta, fechaEstado)
VALUES (@idVenta, 5, '2024-05-31');
GO

-- Venta del 2024-06-01
INSERT INTO venta (fechaVenta, codigoIngreso, idUsuario)
VALUES ('2024-06-01', 'AI8075', 3);
DECLARE @idVenta INT = SCOPE_IDENTITY();
INSERT INTO detalleVenta (idVenta, idProducto, cantidad)
VALUES (@idVenta, 2, 14);
INSERT INTO historialEstadoVenta (idVenta, idEstadoVenta, fechaEstado)
VALUES (@idVenta, 2, '2024-06-01');
GO

-- Venta del 2024-06-02
INSERT INTO venta (fechaVenta, codigoIngreso, idUsuario)
VALUES ('2024-06-02', 'AI7036', 3);
DECLARE @idVenta INT = SCOPE_IDENTITY();
INSERT INTO detalleVenta (idVenta, idProducto, cantidad)
VALUES (@idVenta, 5, 9);
INSERT INTO historialEstadoVenta (idVenta, idEstadoVenta, fechaEstado)
VALUES (@idVenta, 5, '2024-06-02');
GO

-- Venta del 2024-06-29
INSERT INTO venta (fechaVenta, codigoIngreso, idUsuario)
VALUES ('2024-06-29', 'AI9969', 3);
DECLARE @idVenta INT = SCOPE_IDENTITY();
INSERT INTO detalleVenta (idVenta, idProducto, cantidad)
VALUES (@idVenta, 2, 2);
INSERT INTO historialEstadoVenta (idVenta, idEstadoVenta, fechaEstado)
VALUES (@idVenta, 4, '2024-06-29');
GO

-- Venta del 2024-06-30
INSERT INTO venta (fechaVenta, codigoIngreso, idUsuario)
VALUES ('2024-06-30', 'AI9298', 3);
DECLARE @idVenta INT = SCOPE_IDENTITY();
INSERT INTO detalleVenta (idVenta, idProducto, cantidad)
VALUES (@idVenta, 2, 12);
INSERT INTO historialEstadoVenta (idVenta, idEstadoVenta, fechaEstado)
VALUES (@idVenta, 1, '2024-06-30');
GO

-- Venta del 2024-07-01
INSERT INTO venta (fechaVenta, codigoIngreso, idUsuario)
VALUES ('2024-07-01', 'AI2069', 3);
DECLARE @idVenta INT = SCOPE_IDENTITY();
INSERT INTO detalleVenta (idVenta, idProducto, cantidad)
VALUES (@idVenta, 2, 2);
INSERT INTO historialEstadoVenta (idVenta, idEstadoVenta, fechaEstado)
VALUES (@idVenta, 1, '2024-07-01');
GO

-- Venta del 2024-07-02
INSERT INTO venta (fechaVenta, codigoIngreso, idUsuario)
VALUES ('2024-07-02', 'AI4325', 3);
DECLARE @idVenta INT = SCOPE_IDENTITY();
INSERT INTO detalleVenta (idVenta, idProducto, cantidad)
VALUES (@idVenta, 1, 20);
INSERT INTO historialEstadoVenta (idVenta, idEstadoVenta, fechaEstado)
VALUES (@idVenta, 4, '2024-07-02');
GO

-- Venta del 2024-07-29
INSERT INTO venta (fechaVenta, codigoIngreso, idUsuario)
VALUES ('2024-07-29', 'AI6657', 3);
DECLARE @idVenta INT = SCOPE_IDENTITY();
INSERT INTO detalleVenta (idVenta, idProducto, cantidad)
VALUES (@idVenta, 2, 6);
INSERT INTO historialEstadoVenta (idVenta, idEstadoVenta, fechaEstado)
VALUES (@idVenta, 1, '2024-07-29');
GO

-- Venta del 2024-07-30
INSERT INTO venta (fechaVenta, codigoIngreso, idUsuario)
VALUES ('2024-07-30', 'AI3717', 3);
DECLARE @idVenta INT = SCOPE_IDENTITY();
INSERT INTO detalleVenta (idVenta, idProducto, cantidad)
VALUES (@idVenta, 5, 18);
INSERT INTO historialEstadoVenta (idVenta, idEstadoVenta, fechaEstado)
VALUES (@idVenta, 1, '2024-07-30');
GO

-- Venta del 2024-07-31
INSERT INTO venta (fechaVenta, codigoIngreso, idUsuario)
VALUES ('2024-07-31', 'AI9573', 3);
DECLARE @idVenta INT = SCOPE_IDENTITY();
INSERT INTO detalleVenta (idVenta, idProducto, cantidad)
VALUES (@idVenta, 1, 11);
INSERT INTO historialEstadoVenta (idVenta, idEstadoVenta, fechaEstado)
VALUES (@idVenta, 3, '2024-07-31');
GO

-- Venta del 2024-08-01
INSERT INTO venta (fechaVenta, codigoIngreso, idUsuario)
VALUES ('2024-08-01', 'AI6788', 3);
DECLARE @idVenta INT = SCOPE_IDENTITY();
INSERT INTO detalleVenta (idVenta, idProducto, cantidad)
VALUES (@idVenta, 3, 17);
INSERT INTO historialEstadoVenta (idVenta, idEstadoVenta, fechaEstado)
VALUES (@idVenta, 1, '2024-08-01');
GO

-- Venta del 2024-08-28
INSERT INTO venta (fechaVenta, codigoIngreso, idUsuario)
VALUES ('2024-08-28', 'AI8708', 3);
DECLARE @idVenta INT = SCOPE_IDENTITY();
INSERT INTO detalleVenta (idVenta, idProducto, cantidad)
VALUES (@idVenta, 3, 1);
INSERT INTO historialEstadoVenta (idVenta, idEstadoVenta, fechaEstado)
VALUES (@idVenta, 4, '2024-08-28');
GO

-- Venta del 2024-08-29
INSERT INTO venta (fechaVenta, codigoIngreso, idUsuario)
VALUES ('2024-08-29', 'AI3316', 3);
DECLARE @idVenta INT = SCOPE_IDENTITY();
INSERT INTO detalleVenta (idVenta, idProducto, cantidad)
VALUES (@idVenta, 1, 17);
INSERT INTO historialEstadoVenta (idVenta, idEstadoVenta, fechaEstado)
VALUES (@idVenta, 2, '2024-08-29');
GO

-- Venta del 2024-08-30
INSERT INTO venta (fechaVenta, codigoIngreso, idUsuario)
VALUES ('2024-08-30', 'AI4266', 3);
DECLARE @idVenta INT = SCOPE_IDENTITY();
INSERT INTO detalleVenta (idVenta, idProducto, cantidad)
VALUES (@idVenta, 5, 3);
INSERT INTO historialEstadoVenta (idVenta, idEstadoVenta, fechaEstado)
VALUES (@idVenta, 4, '2024-08-30');
GO

-- Venta del 2024-08-31
INSERT INTO venta (fechaVenta, codigoIngreso, idUsuario)
VALUES ('2024-08-31', 'AI2467', 3);
DECLARE @idVenta INT = SCOPE_IDENTITY();
INSERT INTO detalleVenta (idVenta, idProducto, cantidad)
VALUES (@idVenta, 2, 4);
INSERT INTO historialEstadoVenta (idVenta, idEstadoVenta, fechaEstado)
VALUES (@idVenta, 3, '2024-08-31');
GO

-- Venta del 2024-09-27
INSERT INTO venta (fechaVenta, codigoIngreso, idUsuario)
VALUES ('2024-09-27', 'AI3847', 3);
DECLARE @idVenta INT = SCOPE_IDENTITY();
INSERT INTO detalleVenta (idVenta, idProducto, cantidad)
VALUES (@idVenta, 5, 5);
INSERT INTO historialEstadoVenta (idVenta, idEstadoVenta, fechaEstado)
VALUES (@idVenta, 2, '2024-09-27');
GO

-- Venta del 2024-09-28
INSERT INTO venta (fechaVenta, codigoIngreso, idUsuario)
VALUES ('2024-09-28', 'AI5731', 3);
DECLARE @idVenta INT = SCOPE_IDENTITY();
INSERT INTO detalleVenta (idVenta, idProducto, cantidad)
VALUES (@idVenta, 4, 16);
INSERT INTO historialEstadoVenta (idVenta, idEstadoVenta, fechaEstado)
VALUES (@idVenta, 4, '2024-09-28');
GO

-- Venta del 2024-09-29
INSERT INTO venta (fechaVenta, codigoIngreso, idUsuario)
VALUES ('2024-09-29', 'AI5440', 3);
DECLARE @idVenta INT = SCOPE_IDENTITY();
INSERT INTO detalleVenta (idVenta, idProducto, cantidad)
VALUES (@idVenta, 3, 18);
INSERT INTO historialEstadoVenta (idVenta, idEstadoVenta, fechaEstado)
VALUES (@idVenta, 1, '2024-09-29');
GO

-- Venta del 2024-09-30
INSERT INTO venta (fechaVenta, codigoIngreso, idUsuario)
VALUES ('2024-09-30', 'AI8313', 3);
DECLARE @idVenta INT = SCOPE_IDENTITY();
INSERT INTO detalleVenta (idVenta, idProducto, cantidad)
VALUES (@idVenta, 3, 8);
INSERT INTO historialEstadoVenta (idVenta, idEstadoVenta, fechaEstado)
VALUES (@idVenta, 4, '2024-09-30');
GO

-- Venta del 2024-10-27
INSERT INTO venta (fechaVenta, codigoIngreso, idUsuario)
VALUES ('2024-10-27', 'AI3259', 3);
DECLARE @idVenta INT = SCOPE_IDENTITY();
INSERT INTO detalleVenta (idVenta, idProducto, cantidad)
VALUES (@idVenta, 1, 2);
INSERT INTO historialEstadoVenta (idVenta, idEstadoVenta, fechaEstado)
VALUES (@idVenta, 2, '2024-10-27');
GO

-- Venta del 2024-10-28
INSERT INTO venta (fechaVenta, codigoIngreso, idUsuario)
VALUES ('2024-10-28', 'AI5684', 3);
DECLARE @idVenta INT = SCOPE_IDENTITY();
INSERT INTO detalleVenta (idVenta, idProducto, cantidad)
VALUES (@idVenta, 5, 7);
INSERT INTO historialEstadoVenta (idVenta, idEstadoVenta, fechaEstado)
VALUES (@idVenta, 1, '2024-10-28');
GO

-- Venta del 2024-10-29
INSERT INTO venta (fechaVenta, codigoIngreso, idUsuario)
VALUES ('2024-10-29', 'AI5476', 3);
DECLARE @idVenta INT = SCOPE_IDENTITY();
INSERT INTO detalleVenta (idVenta, idProducto, cantidad)
VALUES (@idVenta, 2, 18);
INSERT INTO historialEstadoVenta (idVenta, idEstadoVenta, fechaEstado)
VALUES (@idVenta, 4, '2024-10-29');
GO

-- Venta del 2024-10-30
INSERT INTO venta (fechaVenta, codigoIngreso, idUsuario)
VALUES ('2024-10-30', 'AI6648', 3);
DECLARE @idVenta INT = SCOPE_IDENTITY();
INSERT INTO detalleVenta (idVenta, idProducto, cantidad)
VALUES (@idVenta, 1, 6);
INSERT INTO historialEstadoVenta (idVenta, idEstadoVenta, fechaEstado)
VALUES (@idVenta, 1, '2024-10-30');
GO

-- Venta del 2024-11-26
INSERT INTO venta (fechaVenta, codigoIngreso, idUsuario)
VALUES ('2024-11-26', 'AI2990', 3);
DECLARE @idVenta INT = SCOPE_IDENTITY();
INSERT INTO detalleVenta (idVenta, idProducto, cantidad)
VALUES (@idVenta, 4, 12);
INSERT INTO historialEstadoVenta (idVenta, idEstadoVenta, fechaEstado)
VALUES (@idVenta, 1, '2024-11-26');
GO

-- Venta del 2024-11-27
INSERT INTO venta (fechaVenta, codigoIngreso, idUsuario)
VALUES ('2024-11-27', 'AI4301', 3);
DECLARE @idVenta INT = SCOPE_IDENTITY();
INSERT INTO detalleVenta (idVenta, idProducto, cantidad)
VALUES (@idVenta, 5, 18);
INSERT INTO historialEstadoVenta (idVenta, idEstadoVenta, fechaEstado)
VALUES (@idVenta, 4, '2024-11-27');
GO

-- Venta del 2024-11-28
INSERT INTO venta (fechaVenta, codigoIngreso, idUsuario)
VALUES ('2024-11-28', 'AI1778', 3);
DECLARE @idVenta INT = SCOPE_IDENTITY();
INSERT INTO detalleVenta (idVenta, idProducto, cantidad)
VALUES (@idVenta, 3, 11);
INSERT INTO historialEstadoVenta (idVenta, idEstadoVenta, fechaEstado)
VALUES (@idVenta, 2, '2024-11-28');
GO

-- Venta del 2024-11-29
INSERT INTO venta (fechaVenta, codigoIngreso, idUsuario)
VALUES ('2024-11-29', 'AI9804', 3);
DECLARE @idVenta INT = SCOPE_IDENTITY();
INSERT INTO detalleVenta (idVenta, idProducto, cantidad)
VALUES (@idVenta, 1, 13);
INSERT INTO historialEstadoVenta (idVenta, idEstadoVenta, fechaEstado)
VALUES (@idVenta, 5, '2024-11-29');
GO
