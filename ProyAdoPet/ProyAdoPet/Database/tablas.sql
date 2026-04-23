CREATE DATABASE ProyectoAdoPet;
GO

USE ProyectoAdoPet;
GO

-- =============================================
-- TABLA: Roles			
-- =============================================
CREATE TABLE Rol (
    IdRol INT PRIMARY KEY IDENTITY(1,1),
    NombreRol NVARCHAR(50) NOT NULL
);

INSERT INTO Rol (NombreRol) VALUES ('Administrador'), ('Usuario');
GO

-- =============================================
-- TABLA: User/admin
-- =============================================
CREATE TABLE Usuario (
    IdUsuario INT PRIMARY KEY IDENTITY(1,1),
    Nombre NVARCHAR(100) NOT NULL,
    Correo NVARCHAR(100) NOT NULL UNIQUE,
    Clave NVARCHAR(MAX) NOT NULL,    
    FechaRegistro DATETIME DEFAULT GETDATE(),
    IdRol INT DEFAULT 2,

    CONSTRAINT FK_Usuario_Rol
        FOREIGN KEY (IdRol) REFERENCES Rol(IdRol)
);
GO

-- =============================================
-- TABLA:Estado (mascosta)
-- =============================================
CREATE TABLE Estado (
    Id INT PRIMARY KEY,
    Nombre NVARCHAR(20) NOT NULL
);
GO

--INSERT PRUEBA
INSERT INTO Estado (Id, Nombre) VALUES (1, 'Disponible');
INSERT INTO Estado (Id, Nombre) VALUES (2, 'Adoptado');
INSERT INTO Estado (Id, Nombre) VALUES (3, 'Eliminado');
GO

-- =============================================
-- TABLA: Mascota
-- =============================================
CREATE TABLE Mascota (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Nombre NVARCHAR(100) NOT NULL,
    Edad NVARCHAR(20),
    Descripcion NVARCHAR(MAX),
    EstadoId INT DEFAULT 1,
    FotoMascota NVARCHAR(255) NULL,
    
    CONSTRAINT FK_Mascota_Estado 
        FOREIGN KEY (EstadoId) REFERENCES Estado(Id)
);
GO

--INSERT PRUEBA
INSERT INTO Mascota (Nombre, Edad, Descripcion, EstadoId, FotoMascota)
VALUES ('Max', '3 años', 'Tranquilo y cariñoso, ideal para familia', 1, '0b6856_437e49e1656142bcadfba49e7aa27058~mv2.jpg');
GO