using System;
using System.Data;
using System.Data.SqlClient;
using AccesoDatos.Conexion;

namespace AccesoDatos.DAO
{
    public class PromocionDAO
    {
        private readonly ConexionSQL conexion = new ConexionSQL();

        public DataTable ListarPromociones()
        {
            using (SqlConnection cn = conexion.CrearConexion())
            {
                SqlCommand cmd = new SqlCommand("sp_listar_promociones", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

        public void GestionarPromocion(int idPromocion, string nombre, decimal descuento, DateTime fechaInicio, DateTime fechaFin, string estado)
        {
            using (SqlConnection cn = conexion.CrearConexion())
            {
                SqlCommand cmd = new SqlCommand("sp_gestionar_promocion", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IdPromocion", idPromocion);
                cmd.Parameters.AddWithValue("@Nombre", nombre);
                cmd.Parameters.AddWithValue("@Descuento", descuento);
                cmd.Parameters.AddWithValue("@FechaInicio", fechaInicio);
                cmd.Parameters.AddWithValue("@FechaFin", fechaFin);
                cmd.Parameters.AddWithValue("@Estado", estado);
                cn.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}
