using System;
using System.Data;
using System.Data.SqlClient;
using AccesoDatos.Conexion;

namespace AccesoDatos.DAO
{
    public class MesaDAO
    {
        private readonly Conexion.ConexionSQL conexion = new Conexion.ConexionSQL();

        public DataTable ListarMesasBooking()
        {
            using (SqlConnection cn = conexion.CrearConexion())
            {
                SqlCommand cmd = new SqlCommand("sp_b_listar_mesas", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

        public void ActualizarEstado(int idMesa, string estado)
        {
            using (SqlConnection cn = conexion.CrearConexion())
            {
                SqlCommand cmd = new SqlCommand("sp_actualizar_estado_mesa", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IdMesa", idMesa);
                cmd.Parameters.AddWithValue("@NuevoEstado", estado);
                cn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void GestionarMesa(string operacion, int? idMesa, int idRestaurante, int numeroMesa, string tipoMesa, int capacidad, decimal precio, string imagenURL, string estado)
        {
            using (SqlConnection cn = conexion.CrearConexion())
            {
                SqlCommand cmd = new SqlCommand("sp_gestionar_mesa", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Operacion", operacion);
                cmd.Parameters.AddWithValue("@IdMesa", (object)idMesa ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@IdRestaurante", idRestaurante);
                cmd.Parameters.AddWithValue("@NumeroMesa", numeroMesa);
                cmd.Parameters.AddWithValue("@TipoMesa", tipoMesa);
                cmd.Parameters.AddWithValue("@Capacidad", capacidad);
                cmd.Parameters.AddWithValue("@Precio", precio);
                cmd.Parameters.AddWithValue("@ImagenURL", (object)imagenURL ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Estado", (object)estado ?? DBNull.Value);

                cn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        // Modificado: buscar disponibilidad para una mesa específica en una fecha
        public DataTable BuscarDisponibilidad(string idMesa, DateTime fecha, int personas)
        {
            using (SqlConnection cn = conexion.CrearConexion())
            using (SqlCommand cmd = new SqlCommand("sp_b_disponibilidad_mesas", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                // SP espera @IdMesa INT, @Fecha DATETIME, @Personas INT
                int idMesaInt =0;
                if (!int.TryParse(idMesa, out idMesaInt))
                    idMesaInt =0;

                cmd.Parameters.AddWithValue("@IdMesa", idMesaInt);
                cmd.Parameters.AddWithValue("@Fecha", fecha);
                cmd.Parameters.AddWithValue("@Personas", personas);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }



    }
}