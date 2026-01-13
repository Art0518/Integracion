-- =============================================
-- Stored Procedure: sp_b_generar_factura_simple
-- Descripción: Genera una factura simple y devuelve 
--  INCLUYE EstadoFactura y EstadoReserva
-- =============================================

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE dbo.sp_b_generar_factura_simple
(
    @IdReserva INT,
    @Email VARCHAR(100),
    @Nombre VARCHAR(100),
 @Apellido VARCHAR(100),
    @TipoIdentificacion VARCHAR(20),
    @Identificacion VARCHAR(50),
    @Valor DECIMAL(18,2)
)
AS
BEGIN
    SET NOCOUNT ON;

BEGIN TRY
BEGIN TRANSACTION;

        DECLARE @IdUsuario INT;
      DECLARE @Subtotal DECIMAL(18,2) = ISNULL(@Valor, 0);
     DECLARE @IVA DECIMAL(18,2) = ROUND(@Subtotal * 0.07, 2);
        DECLARE @Total DECIMAL(18,2) = @Subtotal + @IVA;
        DECLARE @FechaFactura DATETIME = GETDATE();
   DECLARE @IdFactura INT;
  DECLARE @EstadoFactura VARCHAR(50);
      DECLARE @EstadoReserva VARCHAR(50);

        --------------------------------------------------------
     -- CREAR U OBTENER USUARIO POR IDENTIFICACION
        --------------------------------------------------------
        IF EXISTS (SELECT 1 FROM seguridad.Usuario WHERE Cedula = @Identificacion)
        BEGIN
    SELECT TOP 1 @IdUsuario = IdUsuario
      FROM seguridad.Usuario
            WHERE Cedula = @Identificacion;

 UPDATE seguridad.Usuario
     SET Nombre = ISNULL(@Nombre, Nombre),
       Apellido = ISNULL(@Apellido, Apellido),
          Email = ISNULL(@Email, Email),
      TipoIdentificacion = ISNULL(@TipoIdentificacion, TipoIdentificacion)
     WHERE IdUsuario = @IdUsuario;
    END
        ELSE
        BEGIN
            INSERT INTO seguridad.Usuario
                (Nombre, Apellido, Email, Cedula, TipoIdentificacion, Rol, Estado)
      VALUES
       (@Nombre, @Apellido, @Email, @Identificacion, @TipoIdentificacion, 'CLIENTE', 'ACTIVO');

         SET @IdUsuario = SCOPE_IDENTITY();
        END

        --------------------------------------------------------
        -- INSERTAR FACTURA
      --------------------------------------------------------
        INSERT INTO facturacion.Factura
 (IdUsuario, IdReserva, Subtotal, IVA, Total, FechaHora, Estado)
    VALUES
  (@IdUsuario, @IdReserva, @Subtotal, @IVA, @Total, @FechaFactura, 'Emitida');

        SET @IdFactura = SCOPE_IDENTITY();

   --------------------------------------------------------
     -- INSERTAR DETALLE DE FACTURA
      --------------------------------------------------------
        INSERT INTO facturacion.DetalleFactura
       (IdFactura, IdReserva, Descripcion, Cantidad, PrecioUnitario, Subtotal)
   VALUES
 (
       @IdFactura,
                @IdReserva,
     CONCAT('Detalle factura reserva ', @IdReserva),
             1,
 @Subtotal,
   @Subtotal
            );

        --------------------------------------------------------
   -- OBTENER ESTADOS REALES DE LA BD
  --------------------------------------------------------
        -- Estado de la factura recién creada
        SELECT @EstadoFactura = Estado
     FROM facturacion.Factura
        WHERE IdFactura = @IdFactura;

        -- Estado de la reserva
        SELECT @EstadoReserva = Estado
        FROM reservas.Reserva
        WHERE IdReserva = @IdReserva;

   COMMIT TRANSACTION;

        --------------------------------------------------------
        -- RESPUESTA CON ESTADOS REALES
        --------------------------------------------------------
        SELECT
       'Factura emitida correctamente.' AS Mensaje,
      @IdFactura AS IdFactura,
        @IdUsuario AS IdUsuario,
            @IdReserva AS IdReserva,
     @Email AS Correo,
@Nombre AS Nombre,
   @TipoIdentificacion AS TipoIdentificacion,
    @Identificacion AS Identificacion,
  @Subtotal AS Subtotal,
       @IVA AS IVA,
       @Total AS Total,
            @FechaFactura AS FechaGeneracion,
            @EstadoFactura AS EstadoFactura,
            @EstadoReserva AS EstadoReserva;
    END TRY
 BEGIN CATCH
     IF @@TRANCOUNT > 0
        ROLLBACK TRANSACTION;

 SELECT
        ERROR_MESSAGE() AS Mensaje,
         NULL AS IdFactura,
            NULL AS IdUsuario,
   @IdReserva AS IdReserva,
    @Email AS Correo,
            @Nombre AS Nombre,
     @TipoIdentificacion AS TipoIdentificacion,
            @Identificacion AS Identificacion,
         NULL AS Subtotal,
            NULL AS IVA,
            NULL AS Total,
            NULL AS FechaGeneracion,
  NULL AS EstadoFactura,
    NULL AS EstadoReserva;
    END CATCH
END
GO
