using System;
using System.Data;
using System.Web.Services;
using Logica.Servicios;

namespace WS_GestionBusSOAP
{
    [WebService(Namespace = "http://cafesanjuansoap.runasp.net/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class WS_Reserva : WebService
    {
        private readonly ReservaLogica reservaLogica = new ReservaLogica();

        // ============================================================
        // 1. Crear Pre-Reserva (HOLD) — equivalente a POST /hold
        // ============================================================
        [WebMethod(Description = "Crear una pre-reserva (equivalente a /hold)")]
        public DataSet CrearPreReserva(int bookingUserId, int idMesa, DateTime fecha, string hora, int personas, int duracionHoldSegundos)
        {
            DataSet ds = new DataSet("CrearPreReservaResponse");

            try
            {
                reservaLogica.CrearPreReserva(
                    bookingUserId,
                    idMesa,
                    fecha,
                    hora,
                    personas,
                    duracionHoldSegundos
                );

                DataTable tabla = new DataTable("PreReserva");
                tabla.Columns.Add("Mensaje");
                tabla.Columns.Add("HoldId");
                tabla.Columns.Add("Fecha");
                tabla.Columns.Add("Hora");
                tabla.Columns.Add("Personas");
                tabla.Columns.Add("DuracionSegundos");

                tabla.Rows.Add(
                    "Pre-reserva creada correctamente.",
                    Guid.NewGuid().ToString(),   // Igual que en REST
                    fecha.ToString("yyyy-MM-dd"),
                    hora,
                    personas,
                    duracionHoldSegundos
                );

                ds.Tables.Add(tabla);
                return ds;
            }
            catch (Exception ex)
            {
                DataTable error = new DataTable("Error");
                error.Columns.Add("Mensaje");
                error.Rows.Add("Error al crear pre-reserva: " + ex.Message);
                ds.Tables.Add(error);
                return ds;
            }
        }

        // ============================================================
        // 2. Confirmar Reserva (BOOK) — equivalente a POST /book
        // ============================================================
        [WebMethod(Description = "Confirmar una reserva (equivalente a /book)")]
        public DataSet CrearReserva(int bookingUserId, int idMesa, DateTime fecha, string hora, int personas, string metodoPago)
        {
            DataSet ds = new DataSet("ConfirmarReservaResponse");

            try
            {
                if (string.IsNullOrWhiteSpace(metodoPago))
                    metodoPago = "EFECTIVO";

                // EJECUTAR SP
                DataTable dtReserva = reservaLogica.CrearReserva(
                    bookingUserId,
                    idMesa,
                    fecha,
                    hora,
                    personas,
                    metodoPago
                );

                // SI NO VOLVIÓ NADA
                if (dtReserva == null || dtReserva.Rows.Count == 0)
                    throw new Exception("El procedimiento almacenado no devolvió información.");

                // SI EL SP DEVOLVIÓ UN ERROR
                if (dtReserva.Columns.Contains("CodigoError"))
                {
                    DataTable error = new DataTable("Error");
                    error.Columns.Add("Mensaje");
                    error.Rows.Add(dtReserva.Rows[0]["Mensaje"].ToString());
                    ds.Tables.Add(error);
                    return ds;
                }

                // SI NO EXISTE LA COLUMNA IdReserva -> ERROR CLARO
                if (!dtReserva.Columns.Contains("IdReserva"))
                    throw new Exception("El procedimiento almacenado no retornó el campo IdReserva.");

                // OBTENER EL ID DE RESERVA
                int idReservaCreada = Convert.ToInt32(dtReserva.Rows[0]["IdReserva"]);

                // RESPUESTA NORMAL
                DataTable tabla = new DataTable("ReservaConfirmada");
                tabla.Columns.Add("Mensaje");
                tabla.Columns.Add("IdReserva");
                tabla.Columns.Add("IdMesa");
                tabla.Columns.Add("Fecha");
                tabla.Columns.Add("Hora");
                tabla.Columns.Add("MetodoPago");
                tabla.Columns.Add("Estado");

                tabla.Rows.Add(
                    "Reserva confirmada correctamente.",
                    idReservaCreada,
                    idMesa,
                    fecha.ToString("yyyy-MM-dd"),
                    hora,
                    metodoPago,
                    "CONFIRMADA"
                );

                ds.Tables.Add(tabla);
                return ds;
            }
            catch (Exception ex)
            {
                DataTable error = new DataTable("Error");
                error.Columns.Add("Mensaje");
                error.Rows.Add("Error al confirmar la reserva: " + ex.Message);
                ds.Tables.Add(error);
                return ds;
            }
        }



        // ============================================================
        // 3. Buscar Reserva por ID — equivalente a GET /reservas/{idReserva}
        // ============================================================
        [WebMethod(Description = "Buscar una reserva por ID (equivalente a /reservas/{idReserva})")]
        public DataSet BuscarReserva(int idReserva)
        {
            DataSet ds = new DataSet("BuscarReservaResponse");

            try
            {
                DataTable dt = reservaLogica.BuscarReservaPorId(idReserva);

                if (dt == null || dt.Rows.Count == 0)
                {
                    DataTable vacio = new DataTable("NoEncontrado");
                    vacio.Columns.Add("Mensaje");
                    vacio.Rows.Add("No existe una reserva con ese ID.");
                    ds.Tables.Add(vacio);
                    return ds;
                }

                dt.TableName = "Reserva";
                ds.Tables.Add(dt);

                return ds;
            }
            catch (Exception ex)
            {
                DataTable error = new DataTable("Error");
                error.Columns.Add("Mensaje");
                error.Rows.Add("Error al buscar reserva: " + ex.Message);
                ds.Tables.Add(error);
                return ds;
            }
        }
    }
}
