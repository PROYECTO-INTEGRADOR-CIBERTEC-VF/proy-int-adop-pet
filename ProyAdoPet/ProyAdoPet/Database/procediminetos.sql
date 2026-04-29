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
    BEGIN TRANSACTION
    BEGIN TRY

        INSERT INTO SolicitudAdopcion (MascotaId, UsuarioId, NombreCompleto, DNI, Telefono, Direccion, MotivoAdopcion, EstadoSolicitudId, FechaCreacion)
        VALUES (@MascotaId, @UsuarioId, @NombreCompleto, @DNI, @Telefono, @Direccion, @MotivoAdopcion, 1, GETDATE());
        
        DECLARE @NuevaSolicitudId INT = SCOPE_IDENTITY();

        UPDATE Mascota SET EstadoId = 3 WHERE Id = @MascotaId;

        COMMIT TRANSACTION
        
        SELECT @NuevaSolicitudId; --id solicitud
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION
        THROW;
    END CATCH
END;


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
        S.UsuarioId,
        S.NombreCompleto AS NombrePostulante,
        S.DNI,
        S.Telefono,
        S.Direccion,
        S.MotivoAdopcion,
        M.Nombre AS MascotaNombre,
        M.FotoMascota,
        S.EstadoSolicitudId AS EstadoActualId,
        E.Nombre AS EstadoNombre,
        C.FechaCita,
        C.Lugar AS LugarCita,
        C.Notas AS NotasCita
    FROM SolicitudAdopcion S
    INNER JOIN Mascota M ON S.MascotaId = M.Id
    INNER JOIN EstadoSolicitud E ON S.EstadoSolicitudId = E.Id
    LEFT JOIN CitaAdopcion C ON S.Id = C.SolicitudId
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



-- =============================================
-- PROCEDIMIENTO: Para cambiar estados, crear contrato, devolver contrato a vista
-- ============================================

CREATE OR ALTER PROCEDURE sp_FinalizarAdopcion
    @SolicitudId INT,
    @Observaciones NVARCHAR(MAX) = 'Sin observaciones iniciales'
AS
BEGIN
    BEGIN TRANSACTION
    BEGIN TRY
        DECLARE @MascotaId INT;
        DECLARE @NuevoContratoId INT;

        --obtener id mascota
        SELECT @MascotaId = MascotaId FROM SolicitudAdopcion WHERE Id = @SolicitudId;

        --cambiar estado solicitud a aprobado
        UPDATE SolicitudAdopcion SET EstadoSolicitudId = 3 WHERE Id = @SolicitudId;

        --cambiar mascota a adoptado
        UPDATE Mascota SET EstadoId = 2 WHERE Id = @MascotaId;

        --crear contrato adopcion
        INSERT INTO ContratoAdopcion (SolicitudId, FechaFirma, ObservacionesIniciales)
        VALUES (@SolicitudId, GETDATE(), @Observaciones);

        --guardamos id de contrato
        SET @NuevoContratoId = SCOPE_IDENTITY();

        --devolver contrato para la vista
        SELECT 
            C.Id AS ContratoNumero,
            C.CodigoContrato,
            S.NombreCompleto AS Adoptante,
            S.DNI,
            S.Telefono,
            M.Nombre AS Mascota,
            C.FechaFirma AS FechaFinal
        FROM ContratoAdopcion C
        INNER JOIN SolicitudAdopcion S ON C.SolicitudId = S.Id
        INNER JOIN Mascota M ON S.MascotaId = M.Id
        WHERE C.Id = @NuevoContratoId;

        COMMIT TRANSACTION
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION
        THROW;
    END CATCH
END;


-- =============================================
-- PROCEDIMIENTO: Obtener detalle del contrato
-- ============================================
CREATE OR ALTER PROCEDURE sp_ObtenerContratoPorSolicitud
    @SolicitudId INT
AS
BEGIN
    SELECT 
        C.Id AS ContratoNumero,
        C.CodigoContrato,
        S.NombreCompleto AS Adoptante,
        S.DNI,
        S.Telefono,
        M.Nombre AS Mascota,
        C.FechaFirma AS FechaFinal
    FROM ContratoAdopcion C
    INNER JOIN SolicitudAdopcion S ON C.SolicitudId = S.Id
    INNER JOIN Mascota M ON S.MascotaId = M.Id
    WHERE S.Id = @SolicitudId;
END;


-- =============================================
-- PROCEDIMIENTO: Rechazar solicitud de adopcion
-- ============================================
CREATE OR ALTER PROCEDURE sp_RechazarSolicitud
    @SolicitudId INT
AS
BEGIN
    DECLARE @MascotaId INT;
    SELECT @MascotaId = MascotaId FROM SolicitudAdopcion WHERE Id = @SolicitudId;

    BEGIN TRANSACTION
    BEGIN TRY
 
        UPDATE SolicitudAdopcion SET EstadoSolicitudId = 4 WHERE Id = @SolicitudId;

        UPDATE Mascota SET EstadoId = 1 WHERE Id = @MascotaId;

        COMMIT TRANSACTION
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION
        THROW;
    END CATCH
END;



-- ================================================================
-- PROCEDIMIENTO: Listar solicitudes de adopcion de un solo usuario
-- ================================================================
CREATE OR ALTER PROCEDURE sp_ListarSolicitudesPorUsuario
    @UsuarioId INT
AS
BEGIN
    SELECT 
        S.Id AS SolicitudId,
        M.Nombre AS MascotaNombre,
        M.FotoMascota AS MascotaFoto,
        S.EstadoSolicitudId AS EstadoId,
        E.Nombre AS EstadoNombre,
        S.FechaCreacion AS FechaEnvio,
        C.FechaCita,
        C.Lugar AS LugarCita,
        C.Notas AS NotasCita
    FROM SolicitudAdopcion S
    INNER JOIN Mascota M ON S.MascotaId = M.Id
    INNER JOIN EstadoSolicitud E ON S.EstadoSolicitudId = E.Id
    LEFT JOIN CitaAdopcion C ON S.Id = C.SolicitudId
    WHERE S.UsuarioId = @UsuarioId
    ORDER BY S.FechaCreacion DESC;
END;
GO


-- ================================================================
-- PROCEDIMIENTO: Obtener datos para acta de adopcion
-- ================================================================
CREATE OR ALTER PROCEDURE sp_ObtenerDatosActa
    @Id INT
AS
BEGIN
    SELECT 
        S.Id AS Folio,
        C.CodigoContrato,
        S.NombreCompleto AS AdoptanteNombre,
        S.DNI AS AdoptanteDNI,
        S.Direccion AS AdoptanteDireccion,
        M.Nombre AS MascotaNombre,
        C.FechaFirma AS FechaEmision,
        C.ObservacionesIniciales
    FROM SolicitudAdopcion S
    INNER JOIN Mascota M ON S.MascotaId = M.Id
    INNER JOIN ContratoAdopcion C ON S.Id = C.SolicitudId
    WHERE S.Id = @Id AND S.EstadoSolicitudId = 3; 
END;


-- ================================================================
-- PROCEDIMIENTO: Obtener todas las adopciones en curso con seguimiento
-- ================================================================
CREATE OR ALTER PROCEDURE sp_ListarAdopcionesEnSeguimiento
AS
BEGIN
    SELECT 
        S.Id AS SolicitudId,
        S.NombreCompleto AS Adoptante,
        S.DNI,
        S.Telefono,
        M.Nombre AS Mascota,
        M.FotoMascota,
        C.CodigoContrato,
        C.FechaFirma AS FechaInicio,
        --fecha ultima visita realizada
        (SELECT MAX(FechaRealizada) 
         FROM SeguimientoAdopcion 
         WHERE SolicitudId = S.Id AND EstadoVisita = 'Realizada') AS UltimoControl
    FROM SolicitudAdopcion S
    INNER JOIN Mascota M ON S.MascotaId = M.Id
    INNER JOIN ContratoAdopcion C ON S.Id = C.SolicitudId
    WHERE S.EstadoSolicitudId = 3
    ORDER BY UltimoControl ASC, FechaInicio ASC;
END;

-- ================================================================
-- PROCEDIMIENTO: Obtener detalles de una adopcion en seguimiento
-- ================================================================
CREATE OR ALTER PROCEDURE sp_ObtenerHistorialSeguimiento
    @SolicitudId INT
AS
BEGIN
    SELECT 
        Id,
        FechaProgramada,
        TipoControl,
        Responsable,
        ObservacionInicial,
        EstadoVisita,
        FechaRealizada,
        Resultado,
        Comentarios,
        FotografiaEvidencia
    FROM SeguimientoAdopcion
    WHERE SolicitudId = @SolicitudId
    ORDER BY FechaProgramada DESC;
END;