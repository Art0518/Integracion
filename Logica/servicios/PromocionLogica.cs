using System;
using System.Data;
using AccesoDatos.DAO;
using GDatos.Entidades;
using Logica.Validaciones;

namespace Logica.Servicios
{
    public class PromocionLogica
    {
        private readonly PromocionDAO dao = new PromocionDAO();

        // ✅ Listar todas las promociones
        public DataTable ListarPromociones()
        {
            return dao.ListarPromociones();
        }

        // ✅ Crear o actualizar promoción
        public void GestionarPromocion(Promocion p)
        {
            // 🔍 Validaciones antes de guardar
            if (string.IsNullOrEmpty(p.Nombre))
                throw new Exception("El nombre de la promoción es obligatorio.");

            if (p.Descuento <= 0 || p.Descuento > 100)
                throw new Exception("El descuento debe ser un valor entre 1 y 100%.");

            if (!ValidacionPromocion.FechasValidas(p.FechaInicio, p.FechaFin))
                throw new Exception("Las fechas de la promoción no son válidas.");

            if (!ValidacionPromocion.EstadoValido(p.Estado))
                throw new Exception("El estado de la promoción no es válido.");

            // ✅ Llamar al DAO
            dao.GestionarPromocion(p.IdPromocion, p.Nombre, p.Descuento, p.FechaInicio, p.FechaFin, p.Estado);
        }
    }
}
