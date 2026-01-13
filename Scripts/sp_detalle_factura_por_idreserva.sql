-- =============================================
-- Stored Procedure: sp_detalle_factura_por_idreserva
-- Descripción: Obtiene el detalle de una factura por IdReserva
-- Devuelve: IdFactura, Subtotal, IVA, Total, EstadoFactura, EstadoReserva, FechaGeneracion
-- =============================================

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE dbo.sp_detalle_factura_por_idreserva
(
    @IdReserva VARCHAR(50)
)
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        -- Verificar si existe la factura para esa reserva
      IF NOT EXISTS (SELECT 1 FROM facturacion.Factura WHERE IdReserva = @IdReserva)
        BEGIN
            -- No se encontró factura para esta reserva
          SELECT 
                NULL AS IdFactura,
  NULL AS IdReserva,
                NULL AS Subtotal,
       NULL AS IVA,
  NULL AS Total,
      NULL AS EstadoFactura,
                NULL AS EstadoReserva,
         NULL AS FechaGeneracion
         WHERE 1 = 0; -- Retorna tabla vacía
   RETURN;
        END

        -- Obtener el detalle de la factura
        SELECT TOP 1
            f.IdFactura,
    f.IdReserva,
          f.Subtotal,
          f.IVA,
            f.Total,
            f.Estado AS EstadoFactura,
            r.Estado AS EstadoReserva,
    f.FechaHora AS FechaGeneracion
   FROM facturacion.Factura f
        LEFT JOIN reservas.Reserva r ON r.IdReserva = f.IdReserva
        WHERE f.IdReserva = @IdReserva
   ORDER BY f.IdFactura DESC; -- Obtener la factura más reciente si hay varias

    END TRY
    BEGIN CATCH
        -- En caso de error, retornar información del error
    SELECT 
        ERROR_MESSAGE() AS Error,
            ERROR_NUMBER() AS ErrorNumber,
 ERROR_LINE() AS ErrorLine;
    END CATCH
END
GO
