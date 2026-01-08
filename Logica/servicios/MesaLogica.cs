using AccesoDatos.DAO;
using GDatos.entidades;
using GDatos.Entidades;
using Logica.Validaciones;
using System;
using System.Data;

namespace Logica.Servicios
{
    public class MesaLogica
    {
        private readonly MesaDAO dao = new MesaDAO();

        // ✅ Listar todas las mesas
        public DataTable ListarMesas()
        {
            return dao.ListarMesasBooking();
        }

        // ✅ Actualizar estado de una mesa
        public void ActualizarEstado(int idMesa, string estado)
        {
            if (idMesa <= 0)
                throw new Exception("Debe indicar un ID de mesa válido.");

            if (string.IsNullOrEmpty(estado))
                throw new Exception("Debe especificar el estado de la mesa.");

            dao.ActualizarEstado(idMesa, estado);
        }

        // ✅ Buscar mesas por filtros (opcional)
        public DataTable BuscarMesas(string tipo = null, int capacidad = 0, string estado = null)
        {
            DataTable mesas = dao.ListarMesasBooking();

            // 🔍 Filtros en memoria (si no existe un SP específico)
            if (!string.IsNullOrEmpty(tipo))
                mesas = mesas.Select($"TipoMesa = '{tipo}'").CopyToDataTable();

            if (capacidad > 0)
                mesas = mesas.Select($"Capacidad >= {capacidad}").CopyToDataTable();

            if (!string.IsNullOrEmpty(estado))
                mesas = mesas.Select($"Estado = '{estado}'").CopyToDataTable();

            return mesas;
        }
        // ✅ Crear o actualizar mesa
        public void GestionarMesa(Mesa m)
        {
            // 🔍 Validaciones antes de guardar
            if (m.IdRestaurante <= 0)
                throw new Exception("Debe seleccionar un restaurante válido.");

            if (m.NumeroMesa <= 0)
                throw new Exception("Debe indicar el número de la mesa.");

            if (string.IsNullOrEmpty(m.TipoMesa))
                throw new Exception("Debe especificar el tipo de mesa.");

            if (m.Capacidad <= 0)
                throw new Exception("La capacidad debe ser mayor a 0.");

            if (m.Precio <= 0)
                throw new Exception("El precio debe ser mayor a 0.");

            if (string.IsNullOrEmpty(m.Estado))
                throw new Exception("Debe especificar el estado de la mesa.");

            // ✅ Determinar la operación: INSERT o UPDATE
            string operacion = (m.IdMesa == 0) ? "INSERT" : "UPDATE";

            // ✅ Llamar al DAO (usa el SP sp_gestionar_mesa)
            dao.GestionarMesa(
                operacion,
                m.IdMesa,
                m.IdRestaurante,
                m.NumeroMesa,
                m.TipoMesa,
                m.Capacidad,
                m.Precio,
                m.ImagenURL,
                m.Estado
            );
        }

        // Nuevo: Consultar disponibilidad para una mesa específica
        public MesaDisponibilidad ConsultarDisponibilidad(string idMesa, DateTime fecha, int personas, string ciudad)
        {
            DataTable dt = dao.BuscarDisponibilidad(idMesa, fecha, personas);
            var result = new MesaDisponibilidad();

            if (dt.Rows.Count > 0)
            {
                var row = dt.Rows[0];

                // Si el SP devuelve columna 'Disponible', úsela. Si no, derive de MesasDisponibles.
                if (dt.Columns.Contains("Disponible"))
                {
                    result.Disponible = Convert.ToBoolean(row["Disponible"]);
                }
                else if (dt.Columns.Contains("MesasDisponibles"))
                {
                    result.MesasDisponibles = Convert.ToInt32(row["MesasDisponibles"]);
                    result.Disponible = result.MesasDisponibles > 0;
                }
                else
                {
                    // fallback: si hay filas, asumimos disponible
                    result.MesasDisponibles = dt.Rows.Count;
                    result.Disponible = dt.Rows.Count > 0;
                }

                result.CapacidadMaximaMesa = dt.Columns.Contains("CapacidadMaximaMesa") ? Convert.ToInt32(row["CapacidadMaximaMesa"]) : 0;
                result.HorarioRestaurante = dt.Columns.Contains("HorarioRestaurante") ? row["HorarioRestaurante"].ToString() : string.Empty;
            }
            else
            {
                result.Disponible = false;
            }

            return result;
        }




    }
}