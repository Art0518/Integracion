using System;
using System.Data;
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
