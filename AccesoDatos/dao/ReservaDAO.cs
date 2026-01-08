using System;
using System.Data;
using System.Data.SqlClient;
using AccesoDatos.Conexion;
using GDatos.Entidades;

namespace AccesoDatos.DAO
{
    public class ReservaDAO
    {
        private readonly ConexionSQL conexion = new ConexionSQL();

        // ============================================================
        // 🟦 BLOQUE DE PRE-RESERVAS (Versión adaptada al BUS)
        // ============================================================

        public DataTable CrearPreReserva(int bookingUserId, int idMesa, DateTime fecha, string hora, int personas, int duracionHoldSegundos)
        {
            using (SqlConnection cn = conexion.CrearConexion())
            {
                SqlCommand cmd = new SqlCommand("sp_b_crear_prereserva", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@BookingUserId", bookingUserId);
                cmd.Parameters.AddWithValue("@IdMesa", idMesa);
                cmd.Parameters.AddWithValue("@Fecha", fecha);
                cmd.Parameters.AddWithValue("@Hora", hora);
                cmd.Parameters.AddWithValue("@NumeroPersonas", personas);
                cmd.Parameters.AddWithValue("@DuracionHoldSegundos", duracionHoldSegundos);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                return dt;
            }
        }

        public DataTable CrearPreReservaBus(string idMesa, DateTime fecha, int numeroClientes, int duracionHoldSegundos)
        {
            using (SqlConnection cn = conexion.CrearConexion())
            {
                SqlCommand cmd = new SqlCommand("sp_crear_prereserva_mesa_bus", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@IdMesa", idMesa);
                cmd.Parameters.AddWithValue("@Fecha", fecha);
                cmd.Parameters.AddWithValue("@NumeroClientes", numeroClientes);
                cmd.Parameters.AddWithValue("@DuracionHoldSegundos", duracionHoldSegundos);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

        // ============================================================
        // 🟦 CREAR RESERVA COMPLETA
        // ============================================================

        public DataTable CrearReserva(int bookingUserId, int idMesa, DateTime fecha, string hora, int personas, string metodoPago)
        {
            using (SqlConnection cn = conexion.CrearConexion())
            {
                SqlCommand cmd = new SqlCommand("sp_b_crear_reserva", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@BookingUserId", bookingUserId);
                cmd.Parameters.AddWithValue("@IdMesa", idMesa);
                cmd.Parameters.AddWithValue("@Fecha", fecha);
                cmd.Parameters.AddWithValue("@Hora", hora);
                cmd.Parameters.AddWithValue("@NumeroPersonas", personas);
                cmd.Parameters.AddWithValue("@MetodoPago", metodoPago);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

        // ============================================================
        // 🟦 CONFIRMAR RESERVA BUS
        // ============================================================

        public DataTable ConfirmarReservaBusCompleto(string idMesa, string idHold, string nombre, string apellido,
                                                   string correo, string tipoIdentificacion, string identificacion,
                                                   DateTime fecha, int personas)
        {
            using (SqlConnection cn = conexion.CrearConexion())
            {
                SqlCommand cmd = new SqlCommand("sp_confirmar_reserva_bus_completo", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@IdMesa", idMesa);
                cmd.Parameters.AddWithValue("@IdHold", idHold);
                cmd.Parameters.AddWithValue("@Nombre", nombre);
                cmd.Parameters.AddWithValue("@Apellido", apellido);
                cmd.Parameters.AddWithValue("@Correo", correo);
                cmd.Parameters.AddWithValue("@TipoIdentificacion", tipoIdentificacion);
                cmd.Parameters.AddWithValue("@Identificacion", identificacion);
                cmd.Parameters.AddWithValue("@Fecha", fecha);
                cmd.Parameters.AddWithValue("@Personas", personas);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();

                try
                {
                    da.Fill(dt);
                }
                catch (Exception ex)
                {
                    dt = new DataTable();
                    dt.Columns.Add("Mensaje");
                    dt.Columns.Add("IdReserva");
                    dt.Columns.Add("Estado");
                    dt.Rows.Add("Error al confirmar reserva: " + ex.Message, null, "ERROR");
                }

                return dt;
            }
        }

        // ============================================================
        // 🟦 BÚSQUEDAS
        // ============================================================

        public DataTable BuscarReservaPorId(int idReserva)
        {
            using (SqlConnection cn = conexion.CrearConexion())
            {
                SqlCommand cmd = new SqlCommand("sp_buscar_reserva_por_id", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@IdReserva", idReserva);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                return dt;
            }
        }

        public DataTable BuscarDatosReservaBus(string idReserva)
        {
            using (SqlConnection cn = conexion.CrearConexion())
            {
                SqlCommand cmd = new SqlCommand("sp_buscar_datos_reserva_bus", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@IdReserva", idReserva);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                return dt;
            }
        }

        // ============================================================
        // 🟦 VALIDACIONES Y UTILIDADES
        // ============================================================

        public bool VerificarMesaExiste(int idMesa)
        {
            using (SqlConnection cn = conexion.CrearConexion())
            {
                SqlCommand cmd = new SqlCommand("SELECT COUNT(1) FROM reservas.Mesa WHERE IdMesa = @IdMesa", cn);
                cmd.Parameters.AddWithValue("@IdMesa", idMesa);

                cn.Open();
                return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
            }
        }

        public int ObtenerCapacidadMesa(int idMesa)
        {
            using (SqlConnection cn = conexion.CrearConexion())
            {
                SqlCommand cmd = new SqlCommand("SELECT Capacidad FROM reservas.Mesa WHERE IdMesa = @IdMesa", cn);
                cmd.Parameters.AddWithValue("@IdMesa", idMesa);

                cn.Open();
                object result = cmd.ExecuteScalar();
                return result != null ? Convert.ToInt32(result) : 0;
            }
        }

        // ============================================================
        // ❗ CORREGIDO: YA NO USA ReservaBus
        // ============================================================

        public bool VerificarHoldExiste(string idHold)
        {
            using (SqlConnection cn = conexion.CrearConexion())
            {
                SqlCommand cmd =
                    new SqlCommand("SELECT COUNT(1) FROM reservas.Reserva WHERE BookingId = @Hold AND Estado='HOLD'", cn);

                cmd.Parameters.AddWithValue("@Hold", idHold);

                cn.Open();
                return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
            }
        }

        public DataTable ObtenerDatosHold(string idHold)
        {
            using (SqlConnection cn = conexion.CrearConexion())
            {
                SqlCommand cmd = new SqlCommand(@"
                    SELECT 
                        IdReserva,
                        IdMesa,
                        Fecha,
                        Hora,
                        NumeroPersonas,
                        BookingId AS IdHold
                    FROM reservas.Reserva
                    WHERE BookingId = @Hold
                      AND Estado = 'HOLD'
                ", cn);

                cmd.Parameters.AddWithValue("@Hold", idHold);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                return dt;
            }
        }

        public bool VerificarMesaPerteneceAlHold(string idHold, int idMesa)
        {
            using (SqlConnection cn = conexion.CrearConexion())
            {
                SqlCommand cmd = new SqlCommand(@"
                    SELECT COUNT(1)
                    FROM reservas.Reserva
                    WHERE BookingId = @Hold
                      AND IdMesa = @Mesa
                      AND Estado = 'HOLD'
                ", cn);

                cmd.Parameters.AddWithValue("@Hold", idHold);
                cmd.Parameters.AddWithValue("@Mesa", idMesa);

                cn.Open();
                return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
            }
        }

        public DataTable VerificarCorreoExistente(string correo)
        {
            using (SqlConnection cn = conexion.CrearConexion())
            {
                SqlCommand cmd = new SqlCommand(@"
                    SELECT IdUsuario, Cedula, Email, Nombre
                    FROM seguridad.Usuario WHERE Email = @Correo", cn);

                cmd.Parameters.AddWithValue("@Correo", correo);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                return dt;
            }
        }

        public DataTable VerificarIdentificacionExistente(string identificacion)
        {
            using (SqlConnection cn = conexion.CrearConexion())
            {
                SqlCommand cmd = new SqlCommand(@"
                    SELECT IdUsuario, Cedula, Email, Nombre
                    FROM seguridad.Usuario WHERE Cedula = @Identificacion", cn);

                cmd.Parameters.AddWithValue("@Identificacion", identificacion);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                return dt;
            }
        }

        // ============================================================
        // ❗ CORREGIDO: YA NO USA ReservaBus
        // ============================================================

        public DataTable BuscarDatosReservaBusPorMesa(int idMesa)
        {
            using (SqlConnection cn = conexion.CrearConexion())
            {
                SqlCommand cmd = new SqlCommand(@"
                    SELECT TOP 1
                        IdReserva,
                        IdMesa,
                        Fecha,
                        Hora,
                        NumeroPersonas,
                        BookingId AS IdHold,
                        Estado
                    FROM reservas.Reserva
                    WHERE IdMesa = @IdMesa
                      AND Estado IN ('HOLD', 'CONFIRMADA')
                    ORDER BY Fecha DESC, Hora DESC
                ", cn);

                cmd.Parameters.AddWithValue("@IdMesa", idMesa);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                return dt;
            }
        }

        // ============================================================
        // 🟥 CANCELAR RESERVA BUS
        // ============================================================

        public DataTable CancelarReservaBus(int idReserva)
        {
            using (SqlConnection cn = conexion.CrearConexion())
            {
                SqlCommand cmd = new SqlCommand("sp_CancelarReservaBus", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@id_reserva", idReserva);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();

                da.Fill(dt);
                return dt;
            }
        }

        // ============================================================
        // 🟦 LISTADOS
        // ============================================================

        public DataTable ListarReservas()
        {
            using (SqlConnection cn = conexion.CrearConexion())
            {
                SqlCommand cmd = new SqlCommand("sp_listar_reservas", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();

                da.Fill(dt);
                return dt;
            }
        }

        public DataTable ActualizarEstado(int idReserva, string nuevoEstado)
        {
            using (SqlConnection cn = conexion.CrearConexion())
            {
                SqlCommand cmd = new SqlCommand("sp_actualizar_estado_reserva", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@IdReserva", idReserva);
                cmd.Parameters.AddWithValue("@NuevoEstado", nuevoEstado);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();

                da.Fill(dt);
                return dt;
            }
        }

        public DataTable VerificarPreReservaBus(string idHold, string idMesa, DateTime fecha, int personas)
        {
            using (SqlConnection cn = conexion.CrearConexion())
            {
                SqlCommand cmd = new SqlCommand("sp_verificar_prereserva_bus", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@IdHold", idHold);
                cmd.Parameters.AddWithValue("@IdMesa", idMesa);
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
