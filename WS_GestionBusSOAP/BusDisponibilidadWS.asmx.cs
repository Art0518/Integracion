using System;
using System.Data;
using System.Web.Services;
using Logica.Servicios;

namespace WS_GestionBusSOAP
{
    [WebService(Namespace = "http://cafesanjuansoap.runasp.net/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class WS_Disponibilidad : WebService
    {
        private readonly MesaLogica mesaLogica = new MesaLogica();

        /// <summary>
        /// SOAP equivalente de:
        /// POST /api/v1/integracion/restaurantes/availability
        /// </summary>
        [WebMethod(Description = "Validar disponibilidad de mesas (equivalente a /api/integracion/restaurantes/availability)")]
        public DataSet ValidarDisponibilidad(DateTime fecha, string hora, int numeroPersonas, string ciudad)
        {
            DataSet ds = new DataSet("ValidarDisponibilidadResponse");

            try
            {
                DataTable mesas = mesaLogica.ListarMesas();

                DataTable tablaResultado = new DataTable("Disponibilidad");
                tablaResultado.Columns.Add("Mensaje");
                tablaResultado.Columns.Add("Fecha");
                tablaResultado.Columns.Add("Hora");
                tablaResultado.Columns.Add("NumeroPersonas");
                tablaResultado.Columns.Add("Ciudad");
                tablaResultado.Columns.Add("Disponible");

                bool disponible = false;

                foreach (DataRow row in mesas.Rows)
                {
                    int capacidad = Convert.ToInt32(row["Capacidad"]);
                    string estado = row["Estado"].ToString().ToUpper();

                    string ubicacion = row.Table.Columns.Contains("Ciudad")
                        ? row["Ciudad"].ToString()
                        : "N/A";

                    if (capacidad >= numeroPersonas &&
                        estado == "DISPONIBLE" &&
                        (string.IsNullOrWhiteSpace(ciudad) ||
                         ubicacion.Equals(ciudad, StringComparison.OrdinalIgnoreCase)))
                    {
                        disponible = true;
                        break;
                    }
                }

                tablaResultado.Rows.Add(
                    "Validación completada correctamente.",
                    fecha.ToString("yyyy-MM-dd"),
                    hora,
                    numeroPersonas.ToString(),
                    ciudad,
                    disponible ? "true" : "false"
                );

                ds.Tables.Add(tablaResultado);
                return ds;
            }
            catch (Exception ex)
            {
                DataTable error = new DataTable("Error");
                error.Columns.Add("Mensaje");
                error.Rows.Add("Error al validar disponibilidad: " + ex.Message);

                ds.Tables.Add(error);
                return ds;
            }
        }
    }
}
