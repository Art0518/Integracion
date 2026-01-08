using System;
using System.Data;
using System.Data.SqlClient;
using AccesoDatos.Conexion;

namespace AccesoDatos.DAO
{
    public class DetalleFacturaDAO
    {
        private readonly ConexionSQL conexion = new ConexionSQL();

        public DataTable ListarDetallesPorFactura(int idFactura)
        {
            using (SqlConnection cn = conexion.CrearConexion())
            {
                SqlCommand cmd = new SqlCommand("sp_listar_detalle_factura", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IdFactura", idFactura);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

        public void InsertarDetalle(int idFactura, int idReserva, int cantidad, decimal precioUnitario)
        {
            using (SqlConnection cn = conexion.CrearConexion())
            {
                SqlCommand cmd = new SqlCommand("sp_insertar_detalle_factura", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@IdFactura", idFactura);
                cmd.Parameters.AddWithValue("@IdReserva", idReserva);
                cmd.Parameters.AddWithValue("@Cantidad", cantidad);
                cmd.Parameters.AddWithValue("@PrecioUnitario", precioUnitario);
                cmd.Parameters.AddWithValue("@Subtotal", cantidad * precioUnitario);

                cn.Open();
                cmd.ExecuteNonQuery();
            }
        }

    }
}
