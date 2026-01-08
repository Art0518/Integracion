using Logica.Servicios;
using System;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Http;
using System.Web.Http.Description;
using Ws_GIntegracionBus.Dtos;
using Ws_GIntegracionBus.DTOS;

namespace Ws_GIntegracionBus.Controllers.V1
{
    [RoutePrefix("api/v1/integracion/restaurantes")]
    public class BusReservaController : ApiController
    {
        private readonly ReservaLogica reservaLogica = new ReservaLogica();

        // ============================================================
        // 1. CREAR PRE-RESERVA (HOLD)
        // ============================================================
        [HttpPost]
        [Route("hold")]
        [ResponseType(typeof(PreReservaBusResponse))]
        public IHttpActionResult CrearPreReserva([FromBody] PreReservaBusDTO body)
        {
            try
            {
                if (body == null)
                    return BadRequest("El cuerpo de la solicitud está vacío.");

                if (string.IsNullOrWhiteSpace(body.id_mesa))
                    return BadRequest("El campo 'id_mesa' es requerido.");

                if (body.numero_clientes <= 0)
                    return BadRequest("El campo 'numero_clientes' debe ser mayor a 0.");

                int duracion = body.duracionHoldSegundos ?? 600;

                DataTable dt = reservaLogica.CrearPreReservaBus(
                    body.id_mesa.Trim(),
                    body.fecha,
                    body.numero_clientes,
                    duracion
                );

                if (dt.Rows.Count == 0)
                    return BadRequest("No se pudo crear la pre-reserva.");

                var row = dt.Rows[0];

                return Ok(new PreReservaBusResponse
                {
                    IdMesa = row.Table.Columns.Contains("IdMesa") ? row["IdMesa"]?.ToString() ?? body.id_mesa : body.id_mesa,
                    FechaReserva = row.Table.Columns.Contains("FechaReserva") && row["FechaReserva"] != DBNull.Value ? Convert.ToDateTime(row["FechaReserva"]) : body.fecha,
                    NumeroPersonas = row.Table.Columns.Contains("NumeroPersonas") && row["NumeroPersonas"] != DBNull.Value ? Convert.ToInt32(row["NumeroPersonas"]) : body.numero_clientes,
                    DuracionHoldSegundos = row.Table.Columns.Contains("DuracionHoldSegundos") && row["DuracionHoldSegundos"] != DBNull.Value ? Convert.ToInt32(row["DuracionHoldSegundos"]) : duracion,
                    IdHold = row.Table.Columns.Contains("IdHold") ? row["IdHold"]?.ToString() ?? "" : "",
                    Mensaje = row.Table.Columns.Contains("Mensaje") ? row["Mensaje"]?.ToString() ?? "" : "Pre-reserva creada exitosamente. Use el id_hold para confirmar la reserva."
                });
            }
            catch (Exception ex)
            {
                return BadRequest("Error al crear pre-reserva: " + ex.Message);
            }
        }


        // ============================================================
        // 2. CONFIRMAR RESERVA (BOOK)
        // ============================================================
        [HttpPost]
        [Route("book")]
        [ResponseType(typeof(ConfirmarReservaResponse))]
        public IHttpActionResult ConfirmarReserva([FromBody] ReservaBusDTO body)
        {
            try
            {
                if (body == null)
                    return BadRequest("El cuerpo de la solicitud está vacío.");

                StringBuilder errores = new StringBuilder();

                // Validaciones obligatorias
                if (string.IsNullOrWhiteSpace(body.id_mesa))
                    errores.AppendLine("• El campo 'id_mesa' es requerido.");

                if (string.IsNullOrWhiteSpace(body.id_hold))
                    errores.AppendLine("• El campo 'id_hold' es requerido.");

                if (string.IsNullOrWhiteSpace(body.nombre))
                    errores.AppendLine("• El campo 'nombre' es requerido.");

                if (string.IsNullOrWhiteSpace(body.apellido))
                    errores.AppendLine("• El campo 'apellido' es requerido.");

                if (string.IsNullOrWhiteSpace(body.correo))
                    errores.AppendLine("• El campo 'correo' es requerido.");

                if (body.personas <= 0)
                    errores.AppendLine("• El campo 'personas' debe ser mayor a 0.");

                if (body.fecha < DateTime.Now)
                    errores.AppendLine("• No se permiten reservas en fechas pasadas.");

                if (!string.IsNullOrWhiteSpace(body.correo) &&
                    !Logica.Validaciones.ValidacionUsuario.EmailValido(body.correo))
                    errores.AppendLine("• Correo electrónico inválido.");

                // Validar tipo_identificación
                if (!string.IsNullOrWhiteSpace(body.tipo_identificacion) &&
                    Regex.IsMatch(body.tipo_identificacion, @"^\d+$"))
                    errores.AppendLine("• El tipo de identificación no puede ser solo números.");

                // Identificación solo números
                if (!string.IsNullOrWhiteSpace(body.identificacion) &&
                    !Regex.IsMatch(body.identificacion, @"^\d+$"))
                    errores.AppendLine("• La identificación debe contener solo números.");

                // Validaciones numéricas de mesa
                int idMesaInt = 0;
                if (!int.TryParse(body.id_mesa?.Trim(), out idMesaInt) || idMesaInt <= 0)
                    errores.AppendLine("• El campo 'id_mesa' debe ser un número entero mayor a 0.");

                if (errores.Length > 0)
                    return BadRequest("Se encontraron los siguientes errores:\n" + errores.ToString());

                // Limpiar campos
                body.id_mesa = body.id_mesa?.Trim();
                body.id_hold = body.id_hold?.Trim();
                body.nombre = body.nombre?.Trim();
                body.apellido = body.apellido?.Trim();
                body.correo = body.correo?.Trim();
                body.tipo_identificacion = body.tipo_identificacion?.Trim() ?? "CEDULA";
                body.identificacion = body.identificacion?.Trim();

                DataTable dt = reservaLogica.ConfirmarReservaBusCompleto(
                    body.id_mesa,
                    body.id_hold,
                    body.nombre,
                    body.apellido,
                    body.correo,
                    body.tipo_identificacion,
                    body.identificacion,
                    body.fecha,
                    body.personas
                );

                if (dt.Rows.Count == 0)
                    return BadRequest("No se recibió respuesta del servidor.");

                var row = dt.Rows[0];

                // Mapear la nueva respuesta del SP
                string mensaje = row.Table.Columns.Contains("Mensaje") ? row["Mensaje"]?.ToString() ?? "" : "";
                string idReserva = row.Table.Columns.Contains("IdReserva") ? row["IdReserva"]?.ToString() ?? "" : "";
                string idMesaResp = row.Table.Columns.Contains("IdMesa") ? row["IdMesa"]?.ToString() ?? body.id_mesa : body.id_mesa;
                string nombreCliente = row.Table.Columns.Contains("NombreCliente") ? row["NombreCliente"]?.ToString() ?? body.nombre : body.nombre;
                string apellidoCliente = row.Table.Columns.Contains("ApellidoCliente") ? row["ApellidoCliente"]?.ToString() ?? body.apellido : body.apellido;
                string correoResp = row.Table.Columns.Contains("Correo") ? row["Correo"]?.ToString() ?? body.correo : body.correo;
                string tipoIdResp = row.Table.Columns.Contains("TipoIdentificacion") ? row["TipoIdentificacion"]?.ToString() ?? body.tipo_identificacion : body.tipo_identificacion;
                string identificacionResp = row.Table.Columns.Contains("Identificacion") ? row["Identificacion"]?.ToString() ?? body.identificacion : body.identificacion;

                DateTime fechaReserva = body.fecha;
                if (row.Table.Columns.Contains("FechaReserva") && row["FechaReserva"] != DBNull.Value)
                    fechaReserva = Convert.ToDateTime(row["FechaReserva"]);

                int numeroPersonas = body.personas;
                if (row.Table.Columns.Contains("NumeroPersonas") && row["NumeroPersonas"] != DBNull.Value)
                    numeroPersonas = Convert.ToInt32(row["NumeroPersonas"]);

                decimal valorPagado = 0;
                if (row.Table.Columns.Contains("ValorPagado") && row["ValorPagado"] != DBNull.Value)
                    valorPagado = Convert.ToDecimal(row["ValorPagado"]);

                string uriFactura = "";
                if (row.Table.Columns.Contains("UriFactura") && row["UriFactura"] != DBNull.Value)
                    uriFactura = row["UriFactura"].ToString();

                // Normalizar uri sin leading slash
                if (!string.IsNullOrEmpty(uriFactura))
                    uriFactura = uriFactura.TrimStart('/');

                // Construir y retornar el objeto solicitado
                return Ok(new ConfirmarReservaResponse
                {
                    Mensaje = mensaje,
                    IdReserva = idReserva,
                    IdMesa = idMesaResp,
                    NombreCliente = nombreCliente,
                    ApellidoCliente = apellidoCliente,
                    Correo = correoResp,
                    TipoIdentificacion = tipoIdResp,
                    Identificacion = identificacionResp,
                    FechaReserva = fechaReserva.ToString("o"),
                    NumeroPersonas = numeroPersonas,
                    ValorPagado = valorPagado,
                    UriFactura = uriFactura
                });
            }
            catch (Exception ex)
            {
                return BadRequest("Error al confirmar la reserva: " + ex.Message);
            }
        }


        // ============================================================
        // 3. BUSCAR DATOS DE UNA RESERVA
        // ============================================================
        [HttpGet]
        [Route("reservas/{idReserva}")]
        [ResponseType(typeof(ReservaDetalleResponse))]
        public IHttpActionResult BuscarDatosReserva(string idReserva)
        {
            try
            {
                if (string.IsNullOrEmpty(idReserva))
                    return BadRequest("Debe enviar un ID de reserva.");

                DataTable dt = reservaLogica.BuscarReservaPorId(Convert.ToInt32(idReserva));

                if (dt.Rows.Count == 0)
                    return NotFound();

                var row = dt.Rows[0];

                // Mapear según la nueva estructura del SP
                string mensaje = row.Table.Columns.Contains("Mensaje") ? row["Mensaje"]?.ToString() ?? "" : "";
                string idReservaResp = row.Table.Columns.Contains("IdReserva") ? row["IdReserva"]?.ToString() ?? idReserva : idReserva;
                string idMesa = row.Table.Columns.Contains("IdMesa") ? row["IdMesa"]?.ToString() ?? "" : "";
                DateTime fecha = row.Table.Columns.Contains("Fecha") && row["Fecha"] != DBNull.Value ? Convert.ToDateTime(row["Fecha"]) : DateTime.MinValue;
                int numeroPersonas = row.Table.Columns.Contains("NumeroPersonas") && row["NumeroPersonas"] != DBNull.Value ? Convert.ToInt32(row["NumeroPersonas"]) : 0;
                string estado = row.Table.Columns.Contains("Estado") ? row["Estado"]?.ToString() ?? "" : "";
                string tipoMesa = row.Table.Columns.Contains("TipoMesa") ? row["TipoMesa"]?.ToString() ?? "" : "";
                string nombreCliente = row.Table.Columns.Contains("NombreCliente") ? row["NombreCliente"]?.ToString() ?? "" : "";
                string apellidoCliente = row.Table.Columns.Contains("ApellidoCliente") ? row["ApellidoCliente"]?.ToString() ?? "" : "";
                string correo = row.Table.Columns.Contains("Correo") ? row["Correo"]?.ToString() ?? "" : "";
                decimal valorPagado = row.Table.Columns.Contains("ValorPagado") && row["ValorPagado"] != DBNull.Value ? Convert.ToDecimal(row["ValorPagado"]) : 0;
                string uriFactura = row.Table.Columns.Contains("UriFactura") ? row["UriFactura"]?.ToString() ?? "" : "";

                // Normalizar uriFactura: si viene vacío y hay IdFactura column, construct; else use as is
                if (string.IsNullOrEmpty(uriFactura) && row.Table.Columns.Contains("IdFactura") && row["IdFactura"] != DBNull.Value)
                {
                    uriFactura = $"api/v1/integracion/restaurantes/invoices/{row["IdFactura"]}";
                }

                var response = new
                {
                    Mensaje = mensaje,
                    IdReserva = idReservaResp,
                    IdMesa = idMesa,
                    Fecha = fecha == DateTime.MinValue ? null : fecha.ToString("o"),
                    NumeroPersonas = numeroPersonas,
                    Estado = estado,
                    TipoMesa = tipoMesa,
                    NombreCliente = nombreCliente,
                    ApellidoCliente = apellidoCliente,
                    Correo = correo,
                    ValorPagado = valorPagado,
                    UriFactura = uriFactura
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest("Error al buscar datos de reserva: " + ex.Message);
            }
        }


        // ============================================================
        // 4. CANCELAR RESERVA
        // ============================================================
        [HttpPost]
        [Route("cancelar")]
        [ResponseType(typeof(CancelacionResponse))]
        public IHttpActionResult CancelarReserva([FromBody] CancelacionRequest body)
        {
            try
            {
                if (body == null)
                    return BadRequest("El cuerpo de la solicitud está vacío.");

                if (string.IsNullOrWhiteSpace(body.id_reserva))
                    return BadRequest("El campo 'id_reserva' es requerido.");

                int idReservaInt;
                if (!int.TryParse(body.id_reserva.Trim(), out idReservaInt))
                    return BadRequest("El ID de reserva debe ser un número entero válido.");

                DataTable dt = reservaLogica.CancelarReservaBus(idReservaInt);

                if (dt.Rows.Count == 0)
                    return Ok(new CancelacionResponse { exito = false, valor_pagado = 0 });

                bool exito = Convert.ToBoolean(dt.Rows[0]["exito"] ?? false);
                decimal valorPagado = Convert.ToDecimal(dt.Rows[0]["valor_pagado"] ?? 0);

                return Ok(new CancelacionResponse
                {
                    exito = exito,
                    valor_pagado = valorPagado
                });
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"ERROR CancelarReserva: {ex.Message}");
                return Ok(new CancelacionResponse
                {
                    exito = false,
                    valor_pagado = 0
                });
            }
        }
    }
}
