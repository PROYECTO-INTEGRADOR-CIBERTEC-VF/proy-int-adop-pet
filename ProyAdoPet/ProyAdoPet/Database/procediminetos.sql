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
    WHERE EstadoId = 1 
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
    DELETE FROM Mascota 
    WHERE Id = @Id AND EstadoId = 1;
END
GO

-- =============================================
-- PROCEDIMIENTO: Verificar si un usuario tiene un solicitud activa
-- =============================================
CREATE OR ALTER PROCEDURE sp_ExisteSolicitudUsuario
    @MascotaId INT,
    @UsuarioId INT
AS
BEGIN
    SELECT COUNT(1) 
    FROM SolicitudAdopcion 
    WHERE MascotaId = @MascotaId 
      AND UsuarioId = @UsuarioId 
      AND EstadoSolicitudId <> 4;
END;


-- =============================================
-- PROCEDIMIENTO: Registro de solicitud de adopcion
-- =============================================
CREATE OR ALTER PROCEDURE sp_RegistrarSolicitud
    @MascotaId INT,
    @UsuarioId INT,
    @NombreCompleto NVARCHAR(100),
    @DNI CHAR(8),
    @Telefono NVARCHAR(15),
    @Direccion NVARCHAR(MAX),
    @MotivoAdopcion NVARCHAR(MAX)
AS
BEGIN
    INSERT INTO SolicitudAdopcion (MascotaId, UsuarioId, NombreCompleto, DNI, Telefono, Direccion, MotivoAdopcion, EstadoSolicitudId, FechaCreacion)
    VALUES (@MascotaId, @UsuarioId, @NombreCompleto, @DNI, @Telefono, @Direccion, @MotivoAdopcion, 1, GETDATE());
    
    SELECT SCOPE_IDENTITY(); -- Retorna el ID insertado
END;
GO


-- =============================================
-- PROCEDIMIENTO: Listado de todas las solicitudes enviadas
-- =============================================
CREATE OR ALTER PROCEDURE sp_ListarSolicitudesAdmin
AS
BEGIN
    SELECT 
        S.Id,
        S.NombreCompleto AS NombrePostulante,
        S.DNI,
        M.Nombre AS NombreMascota,
        M.FotoMascota,
        S.FechaCreacion,
        E.Nombre AS EstadoNombre,
        S.EstadoSolicitudId AS EstadoId
    FROM SolicitudAdopcion S
    INNER JOIN Mascota M ON S.MascotaId = M.Id
    INNER JOIN EstadoSolicitud E ON S.EstadoSolicitudId = E.Id
    ORDER BY S.FechaCreacion DESC;
END;


-- =============================================
-- PROCEDIMIENTO: Obtener el detalle de una solicitud
-- =============================================
CREATE OR ALTER PROCEDURE sp_ObtenerDetalleSolicitud
    @Id INT
AS
BEGIN
    SELECT 
        S.Id AS SolicitudId,
        S.NombreCompleto AS NombrePostulante,
        S.DNI,
        S.Telefono,
        S.Direccion,
        S.MotivoAdopcion,
        M.Nombre AS MascotaNombre,
        M.FotoMascota,
        S.EstadoSolicitudId AS EstadoActualId,
        E.Nombre AS EstadoNombre
    FROM SolicitudAdopcion S
    INNER JOIN Mascota M ON S.MascotaId = M.Id
    INNER JOIN EstadoSolicitud E ON S.EstadoSolicitudId = E.Id
    WHERE S.Id = @Id;
END;


-- =============================================
-- PROCEDIMIENTO: Programar cita presencial a una solicitud
-- =============================================
CREATE OR ALTER PROCEDURE sp_RegistrarCitaAdopcion
    @SolicitudId INT,
    @FechaCita DATETIME,
    @Lugar NVARCHAR(250),
    @Notas NVARCHAR(MAX)
AS
BEGIN
    BEGIN TRANSACTION
    BEGIN TRY
        --insertar cita
        INSERT INTO CitaAdopcion (SolicitudId, FechaCita, Lugar, Notas)
        VALUES (@SolicitudId, @FechaCita, @Lugar, @Notas);

        -- cambiar estado (citado)
        UPDATE SolicitudAdopcion 
        SET EstadoSolicitudId = 2 
        WHERE Id = @SolicitudId;

        COMMIT TRANSACTION
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION
        THROW;
    END CATCH
END;