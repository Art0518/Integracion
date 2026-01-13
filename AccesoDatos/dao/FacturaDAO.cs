using System;
using System.Data;
using System.Data.SqlClient;
using AccesoDatos.Conexion;

namespace AccesoDatos.DAO
{
    public class FacturaDAO
    {
        private readonly ConexionSQL conexion = new ConexionSQL();

        /// <summary>
        /// Genera una factura para una reserva específica y devuelve el resultado del SP como DataTable.
        /// </summary>
        public DataTable GenerarFacturaBooking(int idReserva, string correo, string nombre, string apellido, string tipoIdentificacion, string identificacion, decimal valor)
        {
            using (SqlConnection cn = conexion.CrearConexion())
            using (SqlCommand cmd = new SqlCommand("sp_b_generar_factura_simple", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@IdReserva", idReserva);
                cmd.Parameters.AddWithValue("@Email", (object)correo ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Nombre", (object)nombre ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Apellido", (object)apellido ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@TipoIdentificacion", (object)tipoIdentificacion ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Identificacion", (object)identificacion ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Valor", valor);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

        /// <summary>
        /// Listar todas las facturas.
        /// </summary>
        public DataTable ListarFacturas()
        {
            using (SqlConnection cn = conexion.CrearConexion())
            {
                SqlCommand cmd = new SqlCommand("sp_listar_facturas", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

        /// <summary>
        /// Obtener detalles de una factura.
        /// </summary>
        public DataTable DetalleFactura(int idFactura)
        {
            using (SqlConnection cn = conexion.CrearConexion())
            {
                SqlCommand cmd = new SqlCommand("sp_detalle_factura", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IdFactura", idFactura);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

        /// <summary>
        /// Anular una factura por su ID.
        /// </summary>
        public void AnularFactura(int idFactura)
        {
            using (SqlConnection cn = conexion.CrearConexion())
            {
                SqlCommand cmd = new SqlCommand("sp_anular_factura", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IdFactura", idFactura);
                cn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Genera la factura por usuario, devolviendo una tabla con los datos.
        /// </summary>
        public DataTable GenerarFacturaPorUsuario(int idUsuario)
        {
            using (SqlConnection cn = conexion.CrearConexion())
            {
                SqlCommand cmd = new SqlCommand("sp_b_generar_factura", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@IdUsuario", idUsuario);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                return dt;
            }
        }


        /// <summary>
        /// Obtener el archivo PDF de la factura.
        /// </summary>
        public byte[] ObtenerPdfFactura(int idFactura)
        {
            using (SqlConnection cn = conexion.CrearConexion())
            {
                SqlCommand cmd = new SqlCommand("sp_obtener_pdf_factura", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IdFactura", idFactura);

                cn.Open();
                var result = cmd.ExecuteScalar(); // Obtiene el contenido binario del PDF
                return result as byte[]; // Devuelve los bytes del archivo PDF
            }
        }

        public void GuardarPdfFactura(int idFactura, byte[] pdfBytes)
        {
            using (SqlConnection cn = conexion.CrearConexion())
            {
                SqlCommand cmd = new SqlCommand("sp_guardar_pdf_factura", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@IdFactura", idFactura);
                cmd.Parameters.AddWithValue("@PdfFile", pdfBytes); // El archivo PDF en formato binario

                cn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Obtiene el detalle de la factura por IdReserva (string).
        /// </summary>
        public DataTable DetalleFacturaPorIdReserva(string idReserva)
        {
            using (SqlConnection cn = conexion.CrearConexion())
            {
                SqlCommand cmd = new SqlCommand("sp_detalle_factura_por_idreserva", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IdReserva", idReserva);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

    }
}
