using System;
using System.Data;
using System.Linq;
using AccesoDatos.DAO;
using GDatos.Entidades;
using Logica.Validaciones;

namespace Logica.Servicios
{
    public class UsuarioLogica
    {
        private readonly UsuarioDAO dao = new UsuarioDAO();

        // Login
        public DataTable Login(string email, string contrasena)
        {
            if (!ValidacionUsuario.EmailValido(email))
                throw new Exception("El correo ingresado no es válido.");

            if (string.IsNullOrEmpty(contrasena))
                throw new Exception("Debe ingresar la contraseña.");

            return dao.Login(email, contrasena);
        }

        // Registrar nuevo usuario
        public int Registrar(Usuario u)
        {
            if (string.IsNullOrEmpty(u.Nombre) || string.IsNullOrEmpty(u.Email))
                throw new Exception("Nombre y correo son obligatorios.");

            if (!ValidacionUsuario.EmailValido(u.Email))
                throw new Exception("Correo electrónico inválido.");

            return dao.Registrar(u);
        }

        // Listar usuarios (método original - mantener para compatibilidad)
        public DataTable Listar(string rol = null, string estado = null)
        {
            return dao.Listar(rol, estado);
        }

        // Método simplificado para paginación usando DataTable
        public DataTable ListarPaginado(int pagina, int tamanoPagina, string rol = null, string estado = null)
        {
            if (pagina < 1)
                throw new Exception("La página debe ser mayor a 0.");

            if (tamanoPagina < 1 || tamanoPagina > 100)
                throw new Exception("El tamaño de página debe estar entre 1 y 100.");

            // Por ahora usar el método normal y hacer paginación en memoria
            // En producción deberías usar un SP de paginación real
            DataTable todosLosUsuarios = dao.Listar(rol, estado);

            // Calcular skip y take
            int skip = (pagina - 1) * tamanoPagina;

            // Crear una nueva tabla con los mismos esquemas
            DataTable paginatedTable = todosLosUsuarios.Clone();

            // Agregar metadatos en la primera fila (hack temporal)
            DataRow metaRow = paginatedTable.NewRow();
            metaRow["IdUsuario"] = -1; // Marcador especial
            metaRow["Nombre"] = "META_TOTAL_REGISTROS";
            metaRow["Apellido"] = todosLosUsuarios.Rows.Count.ToString();
            paginatedTable.Rows.Add(metaRow);

            // Agregar las filas paginadas
            var rows = todosLosUsuarios.AsEnumerable()
                .Skip(skip)
                .Take(tamanoPagina);

            foreach (var row in rows)
            {
                paginatedTable.ImportRow(row);
            }

            return paginatedTable;
        }

        // Actualizar usuario
        public void Actualizar(Usuario u)
        {
            if (u.IdUsuario <= 0)
                throw new Exception("ID de usuario no válido.");
            dao.Actualizar(u);
        }

        // Cambiar estado
        public void CambiarEstado(int idUsuario, string nuevoEstado)
        {
            if (string.IsNullOrEmpty(nuevoEstado))
                throw new Exception("Debe especificar un estado.");
            dao.CambiarEstado(idUsuario, nuevoEstado);
        }
    }
}
