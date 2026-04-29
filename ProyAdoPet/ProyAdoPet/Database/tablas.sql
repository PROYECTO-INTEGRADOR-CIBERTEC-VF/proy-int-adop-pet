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


-- =============================================
-- TABLA: Estado solicitud
-- =============================================
CREATE TABLE EstadoSolicitud (
    Id INT PRIMARY KEY,
    Nombre NVARCHAR(50) NOT NULL
);

INSERT INTO EstadoSolicitud (Id, Nombre) VALUES 
(1, 'Pendiente'), 
(2, 'Citado para Entrevista'), 
(3, 'Aprobado - En Adopción'), 
(4, 'Rechazado');


-- =============================================
-- TABLA: Solicitud de adopcion
-- =============================================
CREATE TABLE SolicitudAdopcion (
    Id INT PRIMARY KEY IDENTITY(1,1),
    MascotaId INT NOT NULL,
    UsuarioId INT NOT NULL,
    NombreCompleto NVARCHAR(100) NOT NULL,
    DNI CHAR(8) NOT NULL,
    Telefono NVARCHAR(15) NOT NULL,
    Direccion NVARCHAR(MAX) NOT NULL,
    MotivoAdopcion NVARCHAR(MAX) NOT NULL,
    EstadoSolicitudId INT DEFAULT 1,
    FechaCreacion DATETIME DEFAULT GETDATE(),
    
    CONSTRAINT FK_Solicitud_Mascota FOREIGN KEY (MascotaId) REFERENCES Mascota(Id),
    CONSTRAINT FK_Solicitud_Usuario FOREIGN KEY (UsuarioId) REFERENCES Usuario(IdUsuario),
    CONSTRAINT FK_Solicitud_Estado FOREIGN KEY (EstadoSolicitudId) REFERENCES EstadoSolicitud(Id)
);

-- =============================================
-- TABLA: Cita de Adopcion
-- =============================================
CREATE TABLE CitaAdopcion (
    Id INT PRIMARY KEY IDENTITY(1,1),
    SolicitudId INT NOT NULL,
    FechaCita DATETIME NOT NULL,
    Lugar NVARCHAR(255) DEFAULT 'Oficina Central del Albergue',
    Notas NVARCHAR(MAX),
    CONSTRAINT FK_Cita_Solicitud FOREIGN KEY (SolicitudId) REFERENCES SolicitudAdopcion(Id)
);


-- =============================================
-- TABLA: Contrato adopcion
-- =============================================
CREATE TABLE ContratoAdopcion (
    Id INT PRIMARY KEY IDENTITY(1,1),
    SolicitudId INT NOT NULL,
    CodigoContrato AS ('CONT-' + CAST(Id AS VARCHAR)), -- Codigo autogenerado
    FechaFirma DATETIME DEFAULT GETDATE(),
    TerminosAceptados BIT DEFAULT 1,
    ObservacionesIniciales NVARCHAR(MAX),
    CONSTRAINT FK_Contrato_Solicitud FOREIGN KEY (SolicitudId) REFERENCES SolicitudAdopcion(Id)
);


-- =============================================
-- TABLA: Control de seguimiento para adopciones en curso
-- =============================================
CREATE TABLE SeguimientoAdopcion (
    Id INT PRIMARY KEY IDENTITY(1,1),
    SolicitudId INT NOT NULL,
    
    --datos programar cita
    FechaProgramada DATETIME NOT NULL,
    TipoControl NVARCHAR(50),
    Responsable NVARCHAR(100),
    ObservacionInicial NVARCHAR(MAX),
    EstadoVisita NVARCHAR(20) DEFAULT 'Pendiente',
    
    --datos resultado visita
    FechaRealizada DATETIME NULL,
    Resultado NVARCHAR(50),
    Comentarios NVARCHAR(MAX),
    FotografiaEvidencia NVARCHAR(255),
    
    CONSTRAINT FK_Seguimiento_Solicitud FOREIGN KEY (SolicitudId) REFERENCES SolicitudAdopcion(Id)
);