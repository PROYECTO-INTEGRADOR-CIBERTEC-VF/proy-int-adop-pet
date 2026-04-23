USE ProyectoAdoPet;
GO

-- =============================================
-- PROCEDIMIENTO: Registrar Usuario
-- =============================================
CREATE OR ALTER PROCEDURE sp_RegistrarUsuario
    @Nombre NVARCHAR(100),
    @Correo NVARCHAR(100),
    @Clave NVARCHAR(MAX),
	@IdRol INT,
    @Registrado BIT OUTPUT -- Para avisar si se logro o si el correo ya existía
AS
BEGIN
    IF NOT EXISTS (SELECT 1 FROM Usuario WHERE Correo = @Correo)
    BEGIN
        INSERT INTO Usuario (Nombre, Correo, Clave, IdRol) VALUES (@Nombre, @Correo, @Clave, @IdRol)
        SET @Registrado = 1
    END
    ELSE
    BEGIN
        SET @Registrado = 0
    END
END
GO

-- =============================================
-- PROCEDIMIENTO: Validar usuario
-- =============================================
CREATE OR ALTER PROCEDURE sp_ValidarUsuario
    @Correo NVARCHAR(100),
    @Clave NVARCHAR(MAX)
AS
BEGIN
    SELECT IdUsuario, Nombre, Correo, IdRol 
    FROM Usuario 
    WHERE Correo = @Correo AND Clave = @Clave
END
GO

CREATE OR ALTER PROCEDURE sp_ListarMascotas
AS
BEGIN
    SELECT Id, Nombre, Edad, Descripcion, EstadoId, FotoMascota 
    FROM Mascota
    ORDER BY Id DESC;
END;
GO


-- =============================================
-- PROCEDIMIENTO: Listado de mascotas
-- =============================================
CREATE OR ALTER PROCEDURE sp_ListarMascotas
AS
BEGIN
    SELECT Id, Nombre, Edad, Descripcion, EstadoId, FotoMascota 
    FROM Mascota
    WHERE EstadoId <> 3 
    ORDER BY Id DESC;
END;
GO


-- =============================================
-- PROCEDIMIENTO: Listado estados
-- =============================================
CREATE OR ALTER PROCEDURE sp_ListarEstados
AS
BEGIN
    SELECT Id, Nombre FROM Estado;
END
GO

-- =============================================
-- PROCEDIMIENTO: Registrar mascota
-- =============================================
CREATE OR ALTER PROCEDURE sp_RegistrarMascota
    @Nombre NVARCHAR(100),
    @Edad NVARCHAR(20),
    @Descripcion NVARCHAR(MAX),
    @EstadoId INT,
    @FotoMascota NVARCHAR(255)
AS
BEGIN
    INSERT INTO Mascota (Nombre, Edad, Descripcion, EstadoId, FotoMascota)
    VALUES (@Nombre, @Edad, @Descripcion, @EstadoId, @FotoMascota);
END;
GO

-- =============================================
-- PROCEDIMIENTO: Actualizar mascota
-- =============================================
CREATE PROCEDURE sp_ActualizarMascota
    @Id INT,
    @Nombre VARCHAR(100),
    @Edad VARCHAR(50),
    @Descripcion VARCHAR(200),
    @Estado INT,
    @FotoMascota VARCHAR(200)
AS
BEGIN
    UPDATE Mascota
    SET 
        Nombre = @Nombre,
        Edad = @Edad,
        Descripcion = @Descripcion,
        EstadoId = @Estado,
        FotoMascota = @FotoMascota
    WHERE Id = @Id
END


-- =============================================
-- PROCEDIMIENTO: Obtener mascota por id
-- =============================================
CREATE PROCEDURE sp_ObtenerMascota
    @Id INT
AS
BEGIN
    SELECT 
        Id,
        Nombre,
        Edad,
        Descripcion,
        EstadoId,
        FotoMascota
    FROM Mascota
    WHERE Id = @Id
END

-- =============================================
-- PROCEDIMIENTO: Borrado logico de mascota
-- =============================================
CREATE OR ALTER PROCEDURE sp_EliminarMascota
    @Id INT
AS
BEGIN
    UPDATE Mascota 
    SET EstadoId = 3 --eliminado
    WHERE Id = @Id
END
GO

