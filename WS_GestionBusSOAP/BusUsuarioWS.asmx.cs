using System;
using System.Data;
using System.Web.Services;
using Logica.Servicios;
using GDatos.Entidades;

namespace WS_GestionBusSOAP
{
    [WebService(Namespace = "http://cafesanjuansoap.runasp.net/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class WS_Usuario : WebService
    {
        private readonly UsuarioLogica usuarioLogica = new UsuarioLogica();

        // =======================================================
        // 1. Crear Usuario (equivalente REST POST /usuarios)
        // =======================================================
        [WebMethod(Description = "Registrar un nuevo usuario (equivalente a POST /usuarios)")]
        public DataSet CrearUsuario(string nombre, string email, string contrasena, string rol, string telefono, string direccion)
        {
            DataSet ds = new DataSet("CrearUsuarioResponse");

            try
            {
                // Validaciones equivalentes a REST
                if (string.IsNullOrWhiteSpace(nombre))
                    throw new Exception("El nombre es obligatorio.");

                if (string.IsNullOrWhiteSpace(email))
                    throw new Exception("El email es obligatorio.");

                if (string.IsNullOrWhiteSpace(contrasena))
                    throw new Exception("La contraseña es obligatoria.");

                if (string.IsNullOrWhiteSpace(rol))
                    rol = "CLIENTE";

                if (telefono == null) telefono = "";
                if (direccion == null) direccion = "";

                // Crear objeto usuario
                var usuario = new Usuario
                {
                    Nombre = nombre,
                    Email = email,
                    Contrasena = contrasena,
                    Rol = rol,
                    Estado = "ACTIVO",
                    Telefono = telefono,
                    Direccion = direccion
                };

                // Lógica real
                usuarioLogica.Registrar(usuario);

                // Construcción respuesta SOAP
                DataTable tabla = new DataTable("UsuarioRegistrado");
                tabla.Columns.Add("Mensaje");
                tabla.Columns.Add("Nombre");
                tabla.Columns.Add("Email");
                tabla.Columns.Add("Rol");
                tabla.Columns.Add("Estado");

                tabla.Rows.Add(
                    "Usuario registrado correctamente.",
                    nombre,
                    email,
                    rol,
                    "ACTIVO"
                );

                ds.Tables.Add(tabla);
                return ds;
            }
            catch (Exception ex)
            {
                DataTable error = new DataTable("Error");
                error.Columns.Add("Mensaje");
                error.Rows.Add("Error al registrar usuario: " + ex.Message);
                ds.Tables.Add(error);
                return ds;
            }
        }

        // =======================================================
        // 2. Listar Usuarios (equivalente REST GET /usuarios)
        // =======================================================
        [WebMethod(Description = "Listar todos los usuarios registrados (equivalente a GET /usuarios)")]
        public DataSet ListarUsuarios()
        {
            DataSet ds = new DataSet("ListarUsuariosResponse");

            try
            {
                DataTable usuarios = usuarioLogica.Listar();

                usuarios.TableName = "Usuarios";

                ds.Tables.Add(usuarios);
                return ds;
            }
            catch (Exception ex)
            {
                DataTable error = new DataTable("Error");
                error.Columns.Add("Mensaje");
                error.Rows.Add("Error al listar usuarios: " + ex.Message);
                ds.Tables.Add(error);
                return ds;
            }
        }
    }
}
