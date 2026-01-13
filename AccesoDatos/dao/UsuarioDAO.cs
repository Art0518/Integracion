using System;
using System.Data;
using System.Data.SqlClient;
using AccesoDatos.Conexion;
using GDatos.Entidades;

namespace AccesoDatos.DAO
{
    public class UsuarioDAO
    {
        private readonly Conexion.ConexionSQL conexion = new Conexion.ConexionSQL();

        public DataTable Login(string email, string contrasena)
        {
            using (SqlConnection cn = conexion.CrearConexion())
            {
                SqlCommand cmd = new SqlCommand("seguridad.sp_login_usuario", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@Contrasena", contrasena);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

        public int Registrar(Usuario u)
        {
            using (SqlConnection cn = conexion.CrearConexion())
            {
                SqlCommand cmd = new SqlCommand("sp_registrar_usuario", cn);  // Sin esquema seguridad
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Nombre", u.Nombre);
                cmd.Parameters.AddWithValue("@Apellido", u.Apellido);
                cmd.Parameters.AddWithValue("@Email", u.Email);
                cmd.Parameters.AddWithValue("@TipoIdentificacion", u.TipoIdentificacion);
                cmd.Parameters.AddWithValue("@Identificacion", u.Identificacion);

                // Parámetro de salida para obtener el ID generado
                SqlParameter outputIdParam = new SqlParameter("@IdUsuarioGenerado", SqlDbType.Int);
                outputIdParam.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(outputIdParam);

                cn.Open();
                cmd.ExecuteNonQuery();

                // Devolver el ID generado
                return (int)outputIdParam.Value;
            }
        }

        public DataTable Listar(string rol = null, string estado = null)
        {
            using (SqlConnection cn = conexion.CrearConexion())
            {
                SqlCommand cmd = new SqlCommand("seguridad.sp_listar_usuarios", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Rol", (object)rol ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Estado", (object)estado ?? DBNull.Value);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

        public void Actualizar(Usuario u)
        {
            using (SqlConnection cn = conexion.CrearConexion())
            {
                SqlCommand cmd = new SqlCommand("seguridad.sp_actualizar_usuario", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IdUsuario", u.IdUsuario);
                cmd.Parameters.AddWithValue("@Nombre", (object)u.Nombre ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Email", (object)u.Email ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Telefono", (object)u.Telefono ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Direccion", (object)u.Direccion ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Rol", (object)u.Rol ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Estado", (object)u.Estado ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Contrasena", (object)u.Contrasena ?? DBNull.Value);
                cn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void CambiarEstado(int idUsuario, string nuevoEstado)
        {
            using (SqlConnection cn = conexion.CrearConexion())
            {
                SqlCommand cmd = new SqlCommand("seguridad.sp_cambiar_estado_usuario", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IdUsuario", idUsuario);
                cmd.Parameters.AddWithValue("@NuevoEstado", nuevoEstado);
                cn.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}
