-- =======================================
-- ============ INSERTS ==================
-- =======================================

/* ===============================================================
   PROLIMZA - CARGA INICIAL DESDE 0 (SQL Server)
   Incluye: Ubicación, Usuarios, Materia Prima, Productos (25),
            Recetas + BOM, Compras, Ventas (10 ejemplos).
   =============================================================== */

SET NOCOUNT ON;
SET XACT_ABORT ON;
BEGIN TRAN;

------------------------------------------------------------
-- UBICACIÓN
------------------------------------------------------------
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

------------------------------------------------------------
-- ROLES, USUARIOS y samples de auditoria
------------------------------------------------------------
INSERT INTO roles (nombre, descripcion) VALUES 
('Administrador', 'Acceso total al sistema'),
('Contador', 'Acceso a Modulo de inventario y productos'),
('Productor', 'Acceso a funcionalidades de produccion y recetas'),
('Operador', 'Puede registrar compras y ventas, sin permiso de modificacion');

INSERT INTO usuario (nombre, correo, contrasenaEncriptada, idRol) VALUES 
('Carlos López', 'carlos@prolimza.com', '1234hash', 1),
('María Gómez', 'maria@prolimza.com', 'abcdhash', 2),
('Pedro Ruiz', 'pedro@prolimza.com', 'efghhash', 3),
('Ana Vargas', 'ana@prolimza.com', 'ijklhash', 4),
('Luis Mora', 'luis@prolimza.com', 'mnophash', 4);

INSERT INTO auditoria (log, tipoEvento, idUsuario) VALUES 
('Se modificó cantidad', 'Actualización', 1),
('Se eliminó producto', 'Eliminación', 2),
('Ingreso de materia prima', 'Creación', 3),
('Actualización de stock', 'Actualización', 4),
('Ingreso nuevo producto', 'Creación', 5);

------------------------------------------------------------
-- MATERIA PRIMA (químicos, envases, accesorios)
------------------------------------------------------------
INSERT INTO materiaPrima (nombre, descripcion, sku, cantidad, fechaCaducidad, idBodega, stockMinimo) VALUES
('Agua Desmineralizada', 'Base acuosa para formulación', 'MP-AGUA', 2000, '2027-12-31', 1, 200),
('Alcohol Isopropílico', 'Solvente / limpieza', 'MP-IPA', 500, '2026-12-31', 1, 50),
('Hipoclorito de Sodio 12%', 'Act. clorada', 'MP-NAOCL12', 800, '2026-06-30', 1, 80),
('Tensioactivo Aniónico (LAS)', 'Ácido sulfónico neutralizado', 'MP-LAS', 600, '2027-03-31', 1, 60),
('Tensioactivo No iónico', 'Co–tensioactivo', 'MP-NI', 400, '2027-03-31', 1, 40),
('Sosa Cáustica (NaOH)', 'Alcalinizante', 'MP-NAOH', 300, '2027-03-31', 1, 30),
('Fragancia Lavanda', 'Fragancia para desinfectante/jabones', 'MP-FRAG-LAV', 120, '2027-06-30', 1, 12),
('Fragancia Vainilla', 'Fragancia para aromatizador', 'MP-FRAG-VAN', 120, '2027-06-30', 1, 12),
('Conservante', 'Antimicrobiano de formulación', 'MP-CONS', 200, '2027-12-31', 1, 20),
('Colorante Azul', 'Color para línea lavanda', 'MP-COL-AZ', 50, '2028-12-31', 1, 5),
('Silicona para Llantas (Gel)', 'Agente abrillantador', 'MP-SIL-GEL', 350, '2027-10-31', 1, 35),
('Polímero Brillo Pisos', 'Resina/abrillantador de pisos', 'MP-POL-PISO', 300, '2027-10-31', 1, 30),
('Base Aromatizador', 'Vehículo hidroalcohólico', 'MP-BASE-AROMA', 500, '2027-12-31', 1, 50),
('Envase Galón', 'Envase plástico 3.78L', 'MP-ENV-GAL', 1000, '2030-12-31', 1, 100),
('Envase 1/2 Galón', 'Envase plástico 1.9L', 'MP-ENV-05GAL', 1000, '2030-12-31', 1, 100),
('Envase Litro', 'Envase plástico 1L', 'MP-ENV-1L', 1500, '2030-12-31', 1, 150),
('Trigger', 'Dispensador gatillo', 'MP-TRIGGER', 1500, '2030-12-31', 1, 150),
('Tapa Flip-Top', 'Tapa de presión', 'MP-FLIPTOP', 1500, '2030-12-31', 1, 150);

------------------------------------------------------------
-- PRODUCTOS (25 reales del PDF)
-- producto(nombreProducto, descripcion, sku, cantidad, fechaCaducidad, idBodega, stockMinimo)
------------------------------------------------------------

INSERT INTO producto (nombreProducto, descripcion, sku, cantidad, fechaCaducidad, idBodega, stockMinimo) VALUES
-- Abrillantador de Cerámica
('Abrillantador de Cerámica (1/2 galón)', 'Abrillantador de cerámica presentación 1/2 galón', '233', 40, '2026-12-31', 1, 10),
('Abrillantador de Cerámica (Galón)',      'Abrillantador de cerámica presentación galón',      '282', 30, '2026-12-31', 1, 10),
('Abrillantador de Cerámica (Litro)',      'Abrillantador de cerámica presentación litro',      '189', 50, '2026-12-31', 1, 10),

-- Abrillantador de Llantas
('Abrillantador Llantas Gel (1/2 Galón)',  'Gel abrillantador para llantas 1/2 galón',          '383', 40, '2026-12-31', 1, 10),
('Abrillantador Llantas Gel (Galón)',      'Gel abrillantador para llantas galón',              '195', 25, '2026-12-31', 1, 10),
('Abrillantador Llantas Normal (Galón)',   'Abrillantador normal para llantas galón',           '19',  30, '2026-12-31', 1, 10),
('Abrillantador Llantas Normal (Litro)',   'Abrillantador normal para llantas con trigger',     '20',  60, '2026-12-31', 1, 15),

-- Abrillantador para Pisos
('Abrillantador Pisos (1/2 galón)',        'Abrillantador de pisos 1/2 galón',                  '26',  35, '2026-12-31', 1, 10),
('Abrillantador Pisos (Galón)',            'Abrillantador de pisos galón',                      '27',  30, '2026-12-31', 1, 10),

-- Alcohol / Aromatizador
('Alcohol Mentolado (Litro trigger)',      'Alcohol mentolado con trigger',                     '309', 50, '2026-12-31', 1, 15),

('Aromatizador multiuso (1/2 Galón)',      'Aromatizador multiuso 1/2 galón',                   '447', 20, '2027-12-31', 1, 5),
('Aromatizador multiuso Lavanda (250 ml)', 'Aromatizador Lavanda 250 ml',                       '39',  80, '2027-12-31', 1, 20),
('Aromatizador multiuso Vainilla (250 ml)','Aromatizador Vainilla 250 ml',                      '1326',70, '2027-12-31', 1, 20),
('Aromatizador multiuso (Bolsita 250 ml)', 'Aromatizador en bolsita 250 ml',                    '40',  90, '2027-12-31', 1, 20),
('Aromatizador multiuso (Galón)',          'Aromatizador multiuso galón',                       '49',  20, '2027-12-31', 1, 5),

-- Cera para piso
('Cera p/ pisos líquida (1/2 Galón)',      'Cera líquida para pisos 1/2 galón',                 '43',  35, '2026-12-31', 1, 10),
('Cera p/ pisos líquida Blanca (Galón)',   'Cera líquida blanca para pisos galón',              '45',  25, '2026-12-31', 1, 10),

-- Champú para carro
('Champú p/ carro (1/2 Galón)',            'Shampoo para carro 1/2 galón',                      '52',  40, '2026-12-31', 1, 10),
('Champú p/ carro (Galón)',                'Shampoo para carro galón',                          '51',  30, '2026-12-31', 1, 10),

-- Cloro
('Cloro 4% (1/2 Galón)',                   'Cloro al 4% 1/2 galón',                              '55',  45, '2026-12-31', 1, 15),
('Cloro 12% (Galón)',                      'Cloro al 12% galón',                                 '61',  35, '2026-12-31', 1, 15),

-- Desengrasantes
('Deseng. Multiuso (1/2 Galón)',           'Desengrasante multiuso 1/2 galón',                  '98',  40, '2026-12-31', 1, 10),
('Deseng. Súper Concentrado (Galón)',      'Desengrasante súper concentrado galón',             '102', 30, '2026-12-31', 1, 10),

-- Desinfectantes
('Desinfectante (1/2 Galón)',              'Desinfectante base 1/2 galón',                      '105', 40, '2026-12-31', 1, 10),
('Desinfectante Lavanda (Galón)',          'Desinfectante aroma lavanda galón',                 '113', 35, '2026-12-31', 1, 10);

------------------------------------------------------------
-- VARIABLES de PRODUCTO por SKU (para Recetas y Ventas)
------------------------------------------------------------
DECLARE @p_Aroma_Galon   INT = (SELECT idProducto FROM producto WHERE sku='49');
DECLARE @p_Desinf_Lav    INT = (SELECT idProducto FROM producto WHERE sku='113');
DECLARE @p_Desinf_Base   INT = (SELECT idProducto FROM producto WHERE sku='105');
DECLARE @p_Deseng_SC_G   INT = (SELECT idProducto FROM producto WHERE sku='102');
DECLARE @p_Deseng_MU_05  INT = (SELECT idProducto FROM producto WHERE sku='98');
DECLARE @p_Cloro12_G     INT = (SELECT idProducto FROM producto WHERE sku='61');
DECLARE @p_Abr_Pisos_G   INT = (SELECT idProducto FROM producto WHERE sku='27');
DECLARE @p_Abr_Llant_Gel INT = (SELECT idProducto FROM producto WHERE sku='195');
DECLARE @p_Shampoo_G     INT = (SELECT idProducto FROM producto WHERE sku='51');
DECLARE @p_CeraBl_G      INT = (SELECT idProducto FROM producto WHERE sku='45');
DECLARE @p_Aroma_250Lav  INT = (SELECT idProducto FROM producto WHERE sku='39');
DECLARE @p_Arom_Vain250  INT = (SELECT idProducto FROM producto WHERE sku='1326');
DECLARE @p_AbrCer_G      INT = (SELECT idProducto FROM producto WHERE sku='282');

IF EXISTS (
    SELECT 1 WHERE
    @p_Aroma_Galon   IS NULL OR @p_Desinf_Lav    IS NULL OR @p_Desinf_Base IS NULL OR
    @p_Deseng_SC_G   IS NULL OR @p_Deseng_MU_05  IS NULL OR @p_Cloro12_G   IS NULL OR
    @p_Abr_Pisos_G   IS NULL OR @p_Abr_Llant_Gel IS NULL OR @p_Shampoo_G   IS NULL OR
    @p_CeraBl_G      IS NULL OR @p_Aroma_250Lav  IS NULL OR @p_Arom_Vain250 IS NULL
)
BEGIN
    THROW 51010, 'Faltan productos base (verifique SKUs).', 1;
END;

------------------------------------------------------------
-- VARIABLES de MATERIA PRIMA por SKU (para BOM)
------------------------------------------------------------
DECLARE @mp_AGUA      INT = (SELECT idMateriaPrima FROM materiaPrima WHERE sku='MP-AGUA');
DECLARE @mp_IPA       INT = (SELECT idMateriaPrima FROM materiaPrima WHERE sku='MP-IPA');
DECLARE @mp_NAOCL     INT = (SELECT idMateriaPrima FROM materiaPrima WHERE sku='MP-NAOCL12');
DECLARE @mp_LAS       INT = (SELECT idMateriaPrima FROM materiaPrima WHERE sku='MP-LAS');
DECLARE @mp_NI        INT = (SELECT idMateriaPrima FROM materiaPrima WHERE sku='MP-NI');
DECLARE @mp_NAOH      INT = (SELECT idMateriaPrima FROM materiaPrima WHERE sku='MP-NAOH');
DECLARE @mp_FRAG_LAV  INT = (SELECT idMateriaPrima FROM materiaPrima WHERE sku='MP-FRAG-LAV');
DECLARE @mp_FRAG_VAN  INT = (SELECT idMateriaPrima FROM materiaPrima WHERE sku='MP-FRAG-VAN');
DECLARE @mp_CONS      INT = (SELECT idMateriaPrima FROM materiaPrima WHERE sku='MP-CONS');
DECLARE @mp_COL_AZ    INT = (SELECT idMateriaPrima FROM materiaPrima WHERE sku='MP-COL-AZ');
DECLARE @mp_SIL_GEL   INT = (SELECT idMateriaPrima FROM materiaPrima WHERE sku='MP-SIL-GEL');
DECLARE @mp_POL_PISO  INT = (SELECT idMateriaPrima FROM materiaPrima WHERE sku='MP-POL-PISO');
DECLARE @mp_BASE_AROM INT = (SELECT idMateriaPrima FROM materiaPrima WHERE sku='MP-BASE-AROMA');
DECLARE @mp_ENV_GAL   INT = (SELECT idMateriaPrima FROM materiaPrima WHERE sku='MP-ENV-GAL');
DECLARE @mp_ENV_05GAL INT = (SELECT idMateriaPrima FROM materiaPrima WHERE sku='MP-ENV-05GAL');
DECLARE @mp_ENV_1L    INT = (SELECT idMateriaPrima FROM materiaPrima WHERE sku='MP-ENV-1L');
DECLARE @mp_TRIGGER   INT = (SELECT idMateriaPrima FROM materiaPrima WHERE sku='MP-TRIGGER');
DECLARE @mp_FLIPTOP   INT = (SELECT idMateriaPrima FROM materiaPrima WHERE sku='MP-FLIPTOP');

IF EXISTS (
    SELECT 1 WHERE @mp_AGUA IS NULL OR @mp_LAS IS NULL OR @mp_CONS IS NULL OR @mp_ENV_GAL IS NULL
)
BEGIN
    THROW 51011, 'Faltan materias primas base (verifique inserciones).', 1;
END;

------------------------------------------------------------
-- RECETAS + BOM (10 productos clave)
------------------------------------------------------------
-- Desinfectante Lavanda (Galón) SKU 113
INSERT INTO receta (idProducto, nombre, descripcion)
VALUES (@p_Desinf_Lav, 'Receta Desinfectante Lavanda (Galón)', 'Base clorada con fragancia lavanda');
DECLARE @r_Desinf_Lav INT = SCOPE_IDENTITY();
INSERT INTO materiaReceta (idReceta, idMateriaPrima, cantidad) VALUES
(@r_Desinf_Lav, @mp_AGUA, 2500),
(@r_Desinf_Lav, @mp_NAOCL, 1200),
(@r_Desinf_Lav, @mp_CONS, 5),
(@r_Desinf_Lav, @mp_FRAG_LAV, 8),
(@r_Desinf_Lav, @mp_COL_AZ, 0.5),
(@r_Desinf_Lav, @mp_ENV_GAL, 1);

-- Desinfectante base (1/2 Galón) SKU 105
INSERT INTO receta (idProducto, nombre, descripcion)
VALUES (@p_Desinf_Base, 'Receta Desinfectante Base (1/2 Galón)', 'Versión media con hipoclorito');
DECLARE @r_Desinf_Base INT = SCOPE_IDENTITY();
INSERT INTO materiaReceta (idReceta, idMateriaPrima, cantidad) VALUES
(@r_Desinf_Base, @mp_AGUA, 1200),
(@r_Desinf_Base, @mp_NAOCL, 600),
(@r_Desinf_Base, @mp_CONS, 3),
(@r_Desinf_Base, @mp_ENV_05GAL, 1);

-- Desengrasante Súper Concentrado (Galón) SKU 102
INSERT INTO receta (idProducto, nombre, descripcion)
VALUES (@p_Deseng_SC_G, 'Receta Desengrasante Súper Concentrado (Galón)', 'Alto poder con LAS + No iónico');
DECLARE @r_Deseng_SC_G INT = SCOPE_IDENTITY();
INSERT INTO materiaReceta (idReceta, idMateriaPrima, cantidad) VALUES
(@r_Deseng_SC_G, @mp_AGUA, 1500),
(@r_Deseng_SC_G, @mp_LAS, 900),
(@r_Deseng_SC_G, @mp_NI, 300),
(@r_Deseng_SC_G, @mp_NAOH, 50),
(@r_Deseng_SC_G, @mp_CONS, 4),
(@r_Deseng_SC_G, @mp_ENV_GAL, 1);

-- Desengrasante Multiuso (1/2 Galón) SKU 98
INSERT INTO receta (idProducto, nombre, descripcion)
VALUES (@p_Deseng_MU_05, 'Receta Desengrasante Multiuso (1/2 Galón)', 'Fórmula doméstica');
DECLARE @r_Deseng_MU_05 INT = SCOPE_IDENTITY();
INSERT INTO materiaReceta (idReceta, idMateriaPrima, cantidad) VALUES
(@r_Deseng_MU_05, @mp_AGUA, 1100),
(@r_Deseng_MU_05, @mp_LAS, 350),
(@r_Deseng_MU_05, @mp_NI, 120),
(@r_Deseng_MU_05, @mp_CONS, 3),
(@r_Deseng_MU_05, @mp_ENV_05GAL, 1);

-- Cloro 12% (Galón) SKU 61
INSERT INTO receta (idProducto, nombre, descripcion)
VALUES (@p_Cloro12_G, 'Receta Cloro 12% (Galón)', 'Solución clorada');
DECLARE @r_Cloro12_G INT = SCOPE_IDENTITY();
INSERT INTO materiaReceta (idReceta, idMateriaPrima, cantidad) VALUES
(@r_Cloro12_G, @mp_AGUA, 1800),
(@r_Cloro12_G, @mp_NAOCL, 1300),
(@r_Cloro12_G, @mp_ENV_GAL, 1);

-- Aromatizador multiuso (Galón) SKU 49
INSERT INTO receta (idProducto, nombre, descripcion)
VALUES (@p_Aroma_Galon, 'Receta Aromatizador multiuso (Galón)', 'Base hidroalcohólica con fragancia');
DECLARE @r_Aroma_Galon INT = SCOPE_IDENTITY();
INSERT INTO materiaReceta (idReceta, idMateriaPrima, cantidad) VALUES
(@r_Aroma_Galon, @mp_BASE_AROM, 2100),
(@r_Aroma_Galon, @mp_IPA, 400),
(@r_Aroma_Galon, @mp_FRAG_VAN, 10),
(@r_Aroma_Galon, @mp_CONS, 3),
(@r_Aroma_Galon, @mp_ENV_GAL, 1);

-- Aromatizador Lavanda (250 ml) SKU 39
INSERT INTO receta (idProducto, nombre, descripcion)
VALUES (@p_Aroma_250Lav, 'Receta Aromatizador Lavanda (250 ml)', 'Fragancia lavanda en base');
DECLARE @r_Aroma_250Lav INT = SCOPE_IDENTITY();
INSERT INTO materiaReceta (idReceta, idMateriaPrima, cantidad) VALUES
(@r_Aroma_250Lav, @mp_BASE_AROM, 220),
(@r_Aroma_250Lav, @mp_IPA, 20),
(@r_Aroma_250Lav, @mp_FRAG_LAV, 1.5),
(@r_Aroma_250Lav, @mp_FLIPTOP, 1);

-- Aromatizador Vainilla (250 ml) SKU 1326
INSERT INTO receta (idProducto, nombre, descripcion)
VALUES (@p_Arom_Vain250, 'Receta Aromatizador Vainilla (250 ml)', 'Fragancia vainilla');
DECLARE @r_Arom_Vain250 INT = SCOPE_IDENTITY();
INSERT INTO materiaReceta (idReceta, idMateriaPrima, cantidad) VALUES
(@r_Arom_Vain250, @mp_BASE_AROM, 220),
(@r_Arom_Vain250, @mp_IPA, 20),
(@r_Arom_Vain250, @mp_FRAG_VAN, 1.5),
(@r_Arom_Vain250, @mp_FLIPTOP, 1);

-- Abrillantador de Pisos (Galón) SKU 27
INSERT INTO receta (idProducto, nombre, descripcion)
VALUES (@p_Abr_Pisos_G, 'Receta Abrillantador de Pisos (Galón)', 'Polímero brillo pisos');
DECLARE @r_Abr_Pisos_G INT = SCOPE_IDENTITY();
INSERT INTO materiaReceta (idReceta, idMateriaPrima, cantidad) VALUES
(@r_Abr_Pisos_G, @mp_AGUA, 1800),
(@r_Abr_Pisos_G, @mp_POL_PISO, 700),
(@r_Abr_Pisos_G, @mp_CONS, 3),
(@r_Abr_Pisos_G, @mp_ENV_GAL, 1);

-- Abrillantador Llantas Gel (Galón) SKU 195
INSERT INTO receta (idProducto, nombre, descripcion)
VALUES (@p_Abr_Llant_Gel, 'Receta Abrillantador Llantas Gel (Galón)', 'Silicona en gel');
DECLARE @r_Abr_Llant_Gel INT = SCOPE_IDENTITY();
INSERT INTO materiaReceta (idReceta, idMateriaPrima, cantidad) VALUES
(@r_Abr_Llant_Gel, @mp_SIL_GEL, 1800),
(@r_Abr_Llant_Gel, @mp_CONS, 2),
(@r_Abr_Llant_Gel, @mp_ENV_GAL, 1);

-- Champú para carro (Galón) SKU 51
INSERT INTO receta (idProducto, nombre, descripcion)
VALUES (@p_Shampoo_G, 'Receta Champú para carro (Galón)', 'Tensioactivos balanceados');
DECLARE @r_Shampoo_G INT = SCOPE_IDENTITY();
INSERT INTO materiaReceta (idReceta, idMateriaPrima, cantidad) VALUES
(@r_Shampoo_G, @mp_AGUA, 2000),
(@r_Shampoo_G, @mp_LAS, 500),
(@r_Shampoo_G, @mp_NI, 200),
(@r_Shampoo_G, @mp_CONS, 4),
(@r_Shampoo_G, @mp_ENV_GAL, 1);

-- Cera pisos líquida Blanca (Galón) SKU 45
INSERT INTO receta (idProducto, nombre, descripcion)
VALUES (@p_CeraBl_G, 'Receta Cera pisos líquida Blanca (Galón)', 'Cera/acrílico en emulsión');
DECLARE @r_CeraBl_G INT = SCOPE_IDENTITY();
INSERT INTO materiaReceta (idReceta, idMateriaPrima, cantidad) VALUES
(@r_CeraBl_G, @mp_AGUA, 1600),
(@r_CeraBl_G, @mp_POL_PISO, 900),
(@r_CeraBl_G, @mp_CONS, 3),
(@r_CeraBl_G, @mp_ENV_GAL, 1);

------------------------------------------------------------
-- COMPRAS (materia prima y producto terminado)
------------------------------------------------------------
INSERT INTO compra (fechaCompra, idUsuario, codigoIngreso, estado)
VALUES ('2025-09-05', 4, 'CMP-001', 'Pendiente');
DECLARE @c1 INT = SCOPE_IDENTITY();
INSERT INTO detalleCompraMateriaPrima (idCompra, idMateriaPrima, cantidad) VALUES
(@c1, @mp_AGUA, 2000),
(@c1, @mp_LAS, 600),
(@c1, @mp_NI, 300),
(@c1, @mp_CONS, 50);

INSERT INTO compra (fechaCompra, idUsuario, codigoIngreso, estado)
VALUES ('2025-09-06', 4, 'CMP-002', 'Pendiente');
DECLARE @c2 INT = SCOPE_IDENTITY();
INSERT INTO detalleCompraMateriaPrima (idCompra, idMateriaPrima, cantidad) VALUES
(@c2, @mp_NAOCL, 800),
(@c2, @mp_FRAG_LAV, 50),
(@c2, @mp_COL_AZ, 5),
(@c2, @mp_ENV_GAL, 500);

INSERT INTO compra (fechaCompra, idUsuario, codigoIngreso, estado)
VALUES ('2025-09-07', 4, 'CMP-003', 'Pendiente');
DECLARE @c3 INT = SCOPE_IDENTITY();
INSERT INTO detalleCompraProducto (idCompra, idProducto, cantidad) VALUES
(@c3, @p_Desinf_Lav, 40),
(@c3, @p_Aroma_Galon, 20),
(@c3, @p_Abr_Llant_Gel, 15);

------------------------------------------------------------
-- ESTADOS DE VENTA (catálogo)
------------------------------------------------------------
INSERT INTO estadoVenta (descripcion) VALUES 
('Pendiente'), ('Procesando'), ('Enviado'), ('Entregado'), ('Cancelado');

------------------------------------------------------------
-- VENTAS (10 ejemplos, mapeadas por SKU)
--   p1 = Desinfectante Lavanda (Galón)    -> '113'
--   p2 = Abrillantador Llantas Gel (Galón) -> '195'
--   p3 = Cloro 12% (Galón)                 -> '61'
--   p4 = Desengrasante Súper Conc. (Galón) -> '102'
--   p5 = Aromatizador multiuso (Galón)     -> '49'
------------------------------------------------------------
DECLARE @p1 INT = @p_Desinf_Lav;
DECLARE @p2 INT = @p_Abr_Llant_Gel;
DECLARE @p3 INT = @p_Cloro12_G;
DECLARE @p4 INT = @p_Deseng_SC_G;
DECLARE @p5 INT = @p_Aroma_Galon;

-- 2024-01-01
INSERT INTO venta (fechaVenta, codigoIngreso, idUsuario) VALUES ('2024-01-01','AI5047',3);
DECLARE @idVenta INT = SCOPE_IDENTITY();
INSERT INTO detalleVenta (idVenta, idProducto, cantidad) VALUES (@idVenta, @p5, 14);
INSERT INTO historialEstadoVenta (idVenta, idEstadoVenta, fechaEstado) VALUES (@idVenta, 5, '2024-01-01');

-- 2024-01-02
INSERT INTO venta (fechaVenta, codigoIngreso, idUsuario) VALUES ('2024-01-02','AI3907',3);
SET @idVenta = SCOPE_IDENTITY();
INSERT INTO detalleVenta (idVenta, idProducto, cantidad) VALUES (@idVenta, @p2, 10);
INSERT INTO historialEstadoVenta (idVenta, idEstadoVenta, fechaEstado) VALUES (@idVenta, 3, '2024-01-02');

-- 2024-01-03
INSERT INTO venta (fechaVenta, codigoIngreso, idUsuario) VALUES ('2024-01-03','AI7875',3);
SET @idVenta = SCOPE_IDENTITY();
INSERT INTO detalleVenta (idVenta, idProducto, cantidad) VALUES (@idVenta, @p3, 19);
INSERT INTO historialEstadoVenta (idVenta, idEstadoVenta, fechaEstado) VALUES (@idVenta, 5, '2024-01-03');

-- 2024-01-04
INSERT INTO venta (fechaVenta, codigoIngreso, idUsuario) VALUES ('2024-01-04','AI3767',3);
SET @idVenta = SCOPE_IDENTITY();
INSERT INTO detalleVenta (idVenta, idProducto, cantidad) VALUES (@idVenta, @p4, 13);
INSERT INTO historialEstadoVenta (idVenta, idEstadoVenta, fechaEstado) VALUES (@idVenta, 3, '2024-01-04');

-- 2024-01-31
INSERT INTO venta (fechaVenta, codigoIngreso, idUsuario) VALUES ('2024-01-31','AI9205',3);
SET @idVenta = SCOPE_IDENTITY();
INSERT INTO detalleVenta (idVenta, idProducto, cantidad) VALUES (@idVenta, @p3, 8);
INSERT INTO historialEstadoVenta (idVenta, idEstadoVenta, fechaEstado) VALUES (@idVenta, 2, '2024-01-31');

-- 2024-02-01
INSERT INTO venta (fechaVenta, codigoIngreso, idUsuario) VALUES ('2024-02-01','AI1797',3);
SET @idVenta = SCOPE_IDENTITY();
INSERT INTO detalleVenta (idVenta, idProducto, cantidad) VALUES (@idVenta, @p3, 3);
INSERT INTO historialEstadoVenta (idVenta, idEstadoVenta, fechaEstado) VALUES (@idVenta, 4, '2024-02-01');

-- 2024-02-02
INSERT INTO venta (fechaVenta, codigoIngreso, idUsuario) VALUES ('2024-02-02','AI6954',3);
SET @idVenta = SCOPE_IDENTITY();
INSERT INTO detalleVenta (idVenta, idProducto, cantidad) VALUES (@idVenta, @p1, 3);
INSERT INTO historialEstadoVenta (idVenta, idEstadoVenta, fechaEstado) VALUES (@idVenta, 4, '2024-02-02');

-- 2024-02-03
INSERT INTO venta (fechaVenta, codigoIngreso, idUsuario) VALUES ('2024-02-03','AI7138',3);
SET @idVenta = SCOPE_IDENTITY();
INSERT INTO detalleVenta (idVenta, idProducto, cantidad) VALUES (@idVenta, @p1, 1);
INSERT INTO historialEstadoVenta (idVenta, idEstadoVenta, fechaEstado) VALUES (@idVenta, 2, '2024-02-03');

-- 2024-03-01
INSERT INTO venta (fechaVenta, codigoIngreso, idUsuario) VALUES ('2024-03-01','AI8331',3);
SET @idVenta = SCOPE_IDENTITY();
INSERT INTO detalleVenta (idVenta, idProducto, cantidad) VALUES (@idVenta, @p4, 9);
INSERT INTO historialEstadoVenta (idVenta, idEstadoVenta, fechaEstado) VALUES (@idVenta, 5, '2024-03-01');

-- 2024-03-02
INSERT INTO venta (fechaVenta, codigoIngreso, idUsuario) VALUES ('2024-03-02','AI2346',3);
SET @idVenta = SCOPE_IDENTITY();
INSERT INTO detalleVenta (idVenta, idProducto, cantidad) VALUES (@idVenta, @p1, 5);
INSERT INTO historialEstadoVenta (idVenta, idEstadoVenta, fechaEstado) VALUES (@idVenta, 1, '2024-03-02');

COMMIT;
PRINT 'Carga inicial completada correctamente.';