using System;
using System.Data;
using System.Text.RegularExpressions;
using AccesoDatos.DAO;
using Logica.Validaciones;

namespace Logica.Servicios
{
    public class ReservaLogica
    {
        private readonly ReservaDAO dao = new ReservaDAO();

        // ============================================================
        // 🟦 PRE-RESERVA BUS (sp_crear_prereserva_mesa_bus)
        // ============================================================
        public DataTable CrearPreReservaBus(string idMesa, DateTime fecha, int numeroClientes, int duracionHoldSegundos)
        {
            if (!int.TryParse(idMesa, out int idMesaInt))
                throw new Exception("ID de mesa inválido.");

            if (!dao.VerificarMesaExiste(idMesaInt))
                throw new Exception($"La mesa {idMesaInt} no existe.");

            if (numeroClientes <= 0)
                throw new Exception("Número de clientes inválido.");

            if (!ValidacionReserva.FechaValida(fecha))
                throw new Exception("Fecha inválida.");

            if (fecha < DateTime.Now)
                throw new Exception("No se permiten pre-reservas en fechas pasadas.");

            int capacidad = dao.ObtenerCapacidadMesa(idMesaInt);
            if (numeroClientes > capacidad)
                throw new Exception($"La mesa soporta {capacidad} personas y se solicitaron {numeroClientes}.");

            if (duracionHoldSegundos < 60)
                throw new Exception("HOLD mínimo de 60 segundos.");

            return dao.CrearPreReservaBus(idMesa, fecha, numeroClientes, duracionHoldSegundos);
        }

        // ============================================================
        // 🟦 CONFIRMACIÓN COMPLETA BUS
        // ============================================================
        public DataTable ConfirmarReservaBusCompleto(
            string idMesa,
            string idHold,
            string nombre,
            string apellido,
            string correo,
            string tipoIdentificacion,
            string identificacion,
            DateTime fecha,
            int personas)
        {
            // VALIDACIONES
            if (string.IsNullOrWhiteSpace(idMesa)) throw new Exception("Mesa requerida.");
            if (string.IsNullOrWhiteSpace(idHold)) throw new Exception("Hold requerido.");
            if (string.IsNullOrWhiteSpace(nombre)) throw new Exception("Nombre requerido.");
            // apellido opcional o requerido según negocio
            if (string.IsNullOrWhiteSpace(correo)) throw new Exception("Correo requerido.");
            if (string.IsNullOrWhiteSpace(identificacion)) throw new Exception("Identificación requerida.");
            if (!ValidacionUsuario.EmailValido(correo)) throw new Exception("Correo inválido.");
            if (personas <= 0) throw new Exception("Cantidad de personas inválida.");

            if (!int.TryParse(idMesa, out int idMesaInt))
                throw new Exception("ID de mesa inválido.");

            if (!dao.VerificarMesaExiste(idMesaInt))
                throw new Exception($"La mesa {idMesaInt} no existe.");

            int capacidad = dao.ObtenerCapacidadMesa(idMesaInt);
            if (personas > capacidad)
                throw new Exception("Excede la capacidad de la mesa.");

            // VALIDACIÓN DE DUPLICADOS (Usuario)
            DataTable correoDT = dao.VerificarCorreoExistente(correo);
            if (correoDT.Rows.Count > 0)
            {
                string ced = correoDT.Rows[0]["Cedula"].ToString();
                if (ced != identificacion)
                    throw new Exception($"El correo {correo} ya está registrado con otra identificación.");
            }

            DataTable idDT = dao.VerificarIdentificacionExistente(identificacion);
            if (idDT.Rows.Count > 0)
            {
                string corr = idDT.Rows[0]["Email"].ToString();
                if (corr != correo)
                    throw new Exception($"La identificación {identificacion} ya está registrada con otro correo.");
            }

            // VALIDAR CONSISTENCIA DEL HOLD
            ValidarHold(idHold, idMesaInt, fecha, personas);

            // LLAMAR AL SP
            DataTable dt = dao.ConfirmarReservaBusCompleto(
                idMesa,
                idHold,
                nombre,
                apellido,
                correo,
                tipoIdentificacion,
                identificacion,
                fecha,
                personas
            );

            if (dt.Rows.Count == 0)
                throw new Exception("Error interno: no se recibió respuesta del servidor.");

            return dt;
        }

        // ============================================================
        // 🟦 VALIDACIÓN DEL HOLD
        // ============================================================
        private void ValidarHold(string idHold, int idMesa, DateTime fecha, int personas)
        {
            // Ahora el HOLD se guarda en Reserva.BookingId, no en ReservaBus
            DataTable dt = dao.ObtenerDatosHold(idHold);

            if (dt.Rows.Count == 0)
                throw new Exception("El hold no existe o ha expirado.");

            DataRow row = dt.Rows[0];

            int mesaHold = Convert.ToInt32(row["IdMesa"]);
            int personasHold = Convert.ToInt32(row["NumeroPersonas"]);
            DateTime fechaHold = Convert.ToDateTime(row["Fecha"]);

            if (mesaHold != idMesa)
                throw new Exception($"El hold corresponde a la mesa {mesaHold}, no a {idMesa}.");

            if (fechaHold.Date != fecha.Date)
                throw new Exception("La fecha no coincide con el HOLD.");

            if (personasHold != personas)
                throw new Exception("El número de personas no coincide con el HOLD.");
        }

        // ============================================================
        // 🟦 BUSCAR RESERVA
        // ============================================================
        public DataTable BuscarReservaPorId(int idReserva)
        {
            if (idReserva <= 0) throw new Exception("ID inválido.");
            return dao.BuscarReservaPorId(idReserva);
        }

        public DataTable BuscarDatosReservaBus(string idReserva)
        {
            return dao.BuscarDatosReservaBus(idReserva);
        }

        public DataTable BuscarDatosReservaBusPorMesa(int idMesa)
        {
            return dao.BuscarDatosReservaBusPorMesa(idMesa);
        }

        // ============================================================
        // 🟥 CANCELAR RESERVA
        // ============================================================
        public DataTable CancelarReservaBus(int idReserva)
        {
            if (idReserva <= 0) throw new Exception("ID inválido.");
            return dao.CancelarReservaBus(idReserva);
        }
    }
}
