using System;
using System.Data;
using AccesoDatos.DAO;
using GDatos.Entidades;
using Logica.Validaciones;

namespace Logica.Servicios
{
    public class FacturaLogica
    {
        private readonly FacturaDAO dao = new FacturaDAO();
        private readonly DetalleFacturaDAO daoDetalle = new DetalleFacturaDAO();

        // ✅ Generar una nueva factura, ahora devuelve DataTable con el resultado del SP
        public DataTable GenerarFactura(int idReserva, string correo, string nombre, string tipoIdentificacion, string identificacion, decimal valor)
        {
            // Validación de parámetros de entrada
            if (idReserva <= 0)
                throw new ArgumentException("Debe indicar una reserva válida.", nameof(idReserva));

            // Ya no validamos el correo
            if (string.IsNullOrEmpty(nombre))
                throw new ArgumentException("Debe indicar un nombre válido.", nameof(nombre));

            if (string.IsNullOrEmpty(tipoIdentificacion))
                throw new ArgumentException("Debe indicar un tipo de identificación válido.", nameof(tipoIdentificacion));

            if (string.IsNullOrEmpty(identificacion))
                throw new ArgumentException("Debe indicar una identificación válida.", nameof(identificacion));

            if (valor < 0)
                throw new ArgumentOutOfRangeException(nameof(valor), "El valor de la factura no puede ser negativo.");

            // Apellido no viene en el DTO actual; pasar cadena vacía
            string apellido = string.Empty;

            // Llamar al DAO para generar la factura y retornar el DataTable con el resultado del SP
            return dao.GenerarFacturaBooking(idReserva, correo, nombre, apellido, tipoIdentificacion, identificacion, valor);
        }






        public void GuardarPdfFactura(int idFactura, byte[] pdfBytes)
        {
            dao.GuardarPdfFactura(idFactura, pdfBytes);
        }

        // ✅ Obtener el PDF de la factura
        public byte[] ObtenerPdfFactura(int idFactura)
        {
            return dao.ObtenerPdfFactura(idFactura);
        }

        // ✅ Listar facturas existentes
        public DataTable ListarFacturas()
        {
            return dao.ListarFacturas();
        }

        // ✅ Obtener el detalle de una factura
        public DataTable DetalleFactura(int idFactura)
        {
            if (idFactura <= 0)
                throw new Exception("El ID de la factura no es válido.");

            return dao.DetalleFactura(idFactura);
        }

        // ✅ Obtener una factura por su ID
        public DataRow ObtenerFacturaPorId(int idFactura)
        {
            if (idFactura <= 0)
                throw new ArgumentException("El ID de la factura no es válido.");

            DataTable dt = dao.DetalleFactura(idFactura); // Llamar al DAO para obtener los detalles de la factura

            if (dt == null || dt.Rows.Count == 0)
                return null; // Si no hay datos, retorna null

            // Aquí debemos asegurar que estamos utilizando el índice adecuado para obtener los datos
            DataRow facturaRow = dt.Rows[0];

            // Validamos si la columna 'IdDetalle' está en la fila
            if (!dt.Columns.Contains("IdDetalle"))
            {
                throw new Exception("La columna 'IdDetalle' no está presente en los resultados.");
            }

            return facturaRow; // Retorna la primera fila
        }


        // ✅ Obtener los detalles asociados a una factura
        public DataTable ObtenerDetallesDeFactura(int idFactura)
        {
            if (idFactura <= 0)
                throw new ArgumentException("El ID de la factura no es válido.");

            return dao.DetalleFactura(idFactura); // Llamar al DAO para obtener los detalles
        }

        // ✅ Calcular totales (subtotal, IVA y total)
        public Factura CalcularTotales(Factura f, decimal porcentajeIVA = 0.12m)
        {
            if (f.Subtotal < 0)
                throw new Exception("El subtotal no puede ser negativo.");

            f.IVA = f.Subtotal * porcentajeIVA;
            f.Total = f.Subtotal + f.IVA;
            return f;
        }

        // ✅ Anular una factura
        public void AnularFactura(int idFactura)
        {
            if (idFactura <= 0)
                throw new Exception("Debe indicar un ID de factura válido.");

            dao.AnularFactura(idFactura);
        }

        /// <summary>
        /// Generar una factura por usuario.
        /// </summary>
        /// <param name="idUsuario">ID del usuario para generar la factura.</param>
        /// <returns>Detalles de la factura o error si no se pudo generar.</returns>
        public DataRow GenerarFacturaPorUsuario(int idUsuario)
        {
            // 1️⃣ Validación
            if (idUsuario <= 0)
                throw new ArgumentException("El IdUsuario no es válido.");

            // 2️⃣ Llamar al DAO
            DataTable dt = dao.GenerarFacturaPorUsuario(idUsuario);

            // 3️⃣ Validar si el SP devolvió datos
            if (dt == null || dt.Rows.Count == 0)
            {
                // Devuelve un DataRow "manual" con la estructura exacta que Swagger pide
                DataTable empty = new DataTable();
                empty.Columns.Add("Mensaje");
                empty.Columns.Add("IdUsuario");
                empty.Columns.Add("IdFactura");
                empty.Columns.Add("Subtotal");
                empty.Columns.Add("IVA");
                empty.Columns.Add("Total");
                empty.Columns.Add("Fecha");
                empty.Columns.Add("Estado");

                empty.Rows.Add(
                    "No se pudo generar la factura.",
                    idUsuario,
                    0,
                    0,
                    0,
                    0,
                    "",
                    "ERROR"
                );

                return empty.Rows[0];
            }

            // 4️⃣ Retornar la fila real generada por el SP
            return dt.Rows[0];
        }
    }
}
