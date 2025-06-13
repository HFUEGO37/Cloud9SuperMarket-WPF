INSERT INTO Personas (IdPersona, Nombre, Apellido, Dni, Correo, Telefono)
VALUES
(3, 'María', 'González', '22222222', 'maria.gonzalez@email.com', '5551234567'),
(4, 'Carlos', 'López', '33333333', 'carlos.lopez@email.com', '5552345678'),
(5, 'Ana', 'Martínez', '44444444', 'ana.martinez@email.com', '5553456789'),
(6, 'Pedro', 'Sánchez', '55555555', 'pedro.sanchez@email.com', '5554567890'),
(7, 'Laura', 'Fernández', '66666666', 'laura.fernandez@email.com', '5555678901'),
(8, 'Javier', 'Rodríguez', '77777777', 'javier.rodriguez@email.com', '5556789012'),
(9, 'Sofía', 'Pérez', '88888888', 'sofia.perez@email.com', '5557890123'),
(10, 'Miguel', 'Gómez', '99999999', 'miguel.gomez@email.com', '5558901234'),
(11, 'Elena', 'Ruiz', '10101010', 'elena.ruiz@email.com', '5559012345'),
(12, 'David', 'Hernández', '12121212', 'david.hernandez@email.com', '5550123456');

INSERT INTO Clientes (IdPersona)
VALUES
(3), (4), (5), (6), (7), (8), (9), (10), (11), (12);

INSERT INTO Productos (Nombre, Categoria, Precio, Stock)
VALUES
('Yogur Natural', 'Lácteos', 0.75, 80),
('Queso Cheddar', 'Lácteos', 2.50, 40),
('Huevos (12 unidades)', 'Huevos', 2.00, 60),
('Arroz Blanco', 'Almacén', 1.10, 150),
('Aceite de Oliva', 'Almacén', 3.80, 45),
('Tomates', 'Frutas y Verduras', 1.80, 90),
('Lechuga', 'Frutas y Verduras', 0.90, 70),
('Filete de Ternera', 'Carnicería', 5.50, 25),
('Salmón Fresco', 'Pescadería', 7.20, 15),
('Pan de Molde', 'Panadería', 1.40, 55),
('Cereales', 'Desayunos', 2.30, 65),
('Café Molido', 'Bebidas', 3.50, 50),
('Agua Mineral (1L)', 'Bebidas', 0.60, 200),
('Galletas', 'Snacks', 1.20, 120),
('Chocolate Negro', 'Dulces', 1.80, 85);

-- Pedido 1
INSERT INTO Pedidos (IdCliente, Fecha, Total)
VALUES (1, '2023-05-15 10:30:00', 8.65);

INSERT INTO LineasPedido (IdPedido, IdProducto, Cantidad, PrecioUnitario)
VALUES
(1, 3, 1, 2.00),  -- Huevos
(1, 6, 2, 1.80),  -- Tomates
(1, 15, 1, 1.80), -- Chocolate
(1, 13, 2, 0.60); -- Agua

-- Pedido 2
INSERT INTO Pedidos (IdCliente, Fecha, Total)
VALUES (2, '2023-05-16 14:45:00', 15.40);

INSERT INTO LineasPedido (IdPedido, IdProducto, Cantidad, PrecioUnitario)
VALUES
(2, 8, 2, 5.50),  -- Filete de Ternera
(2, 9, 1, 7.20),  -- Salmón
(2, 7, 1, 0.90),  -- Lechuga
(2, 6, 1, 1.80);  -- Tomates

-- Pedido 3
INSERT INTO Pedidos (IdCliente, Fecha, Total)
VALUES (3, '2023-05-17 09:15:00', 22.30);

INSERT INTO LineasPedido (IdPedido, IdProducto, Cantidad, PrecioUnitario)
VALUES
(3, 12, 2, 3.50),  -- Café
(3, 10, 3, 1.40),  -- Pan de Molde
(3, 11, 1, 2.30),  -- Cereales
(3, 2, 2, 2.50),   -- Queso Cheddar
(3, 1, 1, 0.75);   -- Yogur

-- Pedido 4
INSERT INTO Pedidos (IdCliente, Fecha, Total)
VALUES (4, '2023-05-18 16:20:00', 5.60);

INSERT INTO LineasPedido (IdPedido, IdProducto, Cantidad, PrecioUnitario)
VALUES
(4, 4, 2, 1.10),  -- Arroz
(4, 5, 1, 3.80);  -- Aceite de Oliva

-- Pedido 5
INSERT INTO Pedidos (IdCliente, Fecha, Total)
VALUES (5, '2023-05-19 11:30:00', 9.45);

INSERT INTO LineasPedido (IdPedido, IdProducto, Cantidad, PrecioUnitario)
VALUES
(5, 14, 3, 1.20),  -- Galletas
(5, 15, 2, 1.80),  -- Chocolate
(5, 13, 3, 0.60);  -- Agua

-- Más productos para categorías específicas
INSERT INTO Productos (Nombre, Categoria, Precio, Stock)
VALUES
('Atún en Lata', 'Conservas', 1.50, 75),
('Sopa Instantánea', 'Conservas', 0.80, 100),
('Manzanas Golden', 'Frutas y Verduras', 1.60, 60),
('Plátanos', 'Frutas y Verduras', 1.20, 55),
('Pechuga de Pollo', 'Carnicería', 4.20, 35),
('Zanahorias', 'Frutas y Verduras', 0.70, 85),
('Cebollas', 'Frutas y Verduras', 0.60, 90),
('Leche Desnatada', 'Lácteos', 1.10, 95),
('Mantequilla', 'Lácteos', 1.90, 50),
('Helado de Vainilla', 'Congelados', 3.20, 30);
