using System;
using System.Data;
using System.Web.Services;
using Logica.Servicios;

namespace WS_GestionBusSOAP
{
    [WebService(Namespace = "http://cafesanjuansoap.runasp.net/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class WS_Factura : WebService
    {
        private readonly FacturaLogica facturaLogica = new FacturaLogica();

        /// <summary>
        /// SOAP equivalente a:
        /// POST /api/v1/integracion/restaurantes/invoices
        /// </summary>
        [WebMethod(Description = "Emitir una factura usando la reserva HOLD del usuario")]
        public DataSet EmitirFactura(int idUsuario)
        {
            DataSet ds = new DataSet("EmitirFacturaResponse");

            try
            {
                if (idUsuario <= 0)
                {
                    DataTable error = new DataTable("Error");
                    error.Columns.Add("Mensaje");
                    error.Rows.Add("IdUsuario inválido.");
                    ds.Tables.Add(error);
                    return ds;
                }

                // 🟢 Lógica REAL (igual al REST)
                var row = facturaLogica.GenerarFacturaPorUsuario(idUsuario);

                if (row == null)
                {
                    DataTable error = new DataTable("Error");
                    error.Columns.Add("Mensaje");
                    error.Rows.Add("No se pudo generar la factura.");
                    ds.Tables.Add(error);
                    return ds;
                }

                // 🟢 Crear respuesta SOAP con los mismos campos del REST
                DataTable table = new DataTable("FacturaEmitida");

                table.Columns.Add("Mensaje");
                table.Columns.Add("IdUsuario");
                table.Columns.Add("IdFactura");
                table.Columns.Add("Subtotal");
                table.Columns.Add("IVA");
                table.Columns.Add("Total");
                table.Columns.Add("Fecha");
                table.Columns.Add("Estado");

                table.Rows.Add(
                    row["Mensaje"].ToString(),
                    row["IdUsuario"].ToString(),
                    row["IdFactura"].ToString(),
                    row["Subtotal"].ToString(),
                    row["IVA"].ToString(),
                    row["Total"].ToString(),
                    row["Fecha"].ToString(),
                    row["Estado"].ToString()
                );

                ds.Tables.Add(table);
                return ds;
            }
            catch (Exception ex)
            {
                DataTable errorTable = new DataTable("Error");
                errorTable.Columns.Add("Mensaje");
                errorTable.Rows.Add("Error al emitir factura: " + ex.Message);
                ds.Tables.Add(errorTable);

                return ds;
            }
        }
    }
}
