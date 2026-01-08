using System;
using System.Data;
using AccesoDatos.DAO;
using GDatos.Entidades;
using Logica.Validaciones;

namespace Logica.Servicios
{
    public class DetalleFacturaLogica
    {
        private readonly DetalleFacturaDAO dao = new DetalleFacturaDAO();

        // ✅ Listar detalles de una factura específica
        public DataTable ListarDetallesPorFactura(int idFactura)
        {
            if (idFactura <= 0)
                throw new Exception("El ID de la factura no es válido.");

            return dao.ListarDetallesPorFactura(idFactura);
        }

        // ✅ Insertar un nuevo detalle en la factura
        public void InsertarDetalle(DetalleFactura detalle)
        {
            // 🔍 Validaciones de negocio
            if (detalle.IdFactura <= 0)
                throw new Exception("Debe indicar un ID de factura válido.");

            if (detalle.IdReserva <= 0)
                throw new Exception("Debe indicar una reserva válida.");

            if (detalle.Cantidad <= 0)
                throw new Exception("La cantidad debe ser mayor que 0.");

            if (detalle.PrecioUnitario <= 0)
                throw new Exception("El precio unitario debe ser mayor que 0.");

            // ✅ Calcular subtotal automáticamente
            detalle.Subtotal = detalle.Cantidad * detalle.PrecioUnitario;

            if (!ValidacionFactura.MontosValidos(detalle.Subtotal, 0, 0))
                throw new Exception("El subtotal no puede ser negativo.");

            // ✅ Llamar al DAO
            dao.InsertarDetalle(detalle.IdFactura, detalle.IdReserva, detalle.Cantidad, detalle.PrecioUnitario);
        }
    }
}
