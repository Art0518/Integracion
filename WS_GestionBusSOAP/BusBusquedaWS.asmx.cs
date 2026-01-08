using System;
using System.Data;
using System.Web.Services;
using Logica.Servicios;

namespace WS_GestionBusSOAP
{
    [WebService(Namespace = "http://cafesanjuansoap.runasp.net/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class WS_Busqueda : WebService
    {
        private readonly MesaLogica mesaLogica = new MesaLogica();

        /// <summary>
        /// SOAP equivalente al endpoint REST:
        /// GET /api/v1/integracion/restaurantes/search
        /// </summary>
        [WebMethod(Description = "Buscar mesas registradas (equivalente al endpoint REST /api/integracion/restaurantes/search)")]
        public DataSet BuscarMesas()
        {
            DataSet ds = new DataSet("BuscarMesasResponse");

            try
            {
                DataTable resultado = mesaLogica.ListarMesas();

                // Si no hay mesas
                if (resultado == null || resultado.Rows.Count == 0)
                {
                    DataTable tablaMensaje = new DataTable("Respuesta");
                    tablaMensaje.Columns.Add("Mensaje");
                    tablaMensaje.Columns.Add("Total");

                    tablaMensaje.Rows.Add(
                        "No se encontraron mesas registradas.",
                        0
                    );

                    ds.Tables.Add(tablaMensaje);
                    return ds;
                }

                // Si hay mesas
                resultado.TableName = "Mesas";
                ds.Tables.Add(resultado);

                DataTable info = new DataTable("Info");
                info.Columns.Add("Mensaje");
                info.Columns.Add("Total");

                info.Rows.Add(
                    "Consulta de mesas realizada con éxito.",
                    resultado.Rows.Count.ToString()
                );

                ds.Tables.Add(info);

                return ds;
            }
            catch (Exception ex)
            {
                DataTable error = new DataTable("Error");
                error.Columns.Add("Mensaje");
                error.Rows.Add("Error al buscar mesas: " + ex.Message);

                ds.Tables.Add(error);
                return ds;
            }
        }
    }
}
