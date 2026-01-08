using System;
using System.Data;
using System.Data.SqlClient;

namespace AccesoDatos.Conexion
{
    public class ConexionSQL
    {
        // ✅ Leer cadena de conexión desde variable de entorno o usar default
        private readonly string cadenaConexion =
 Environment.GetEnvironmentVariable("CONNECTION_STRING") ??
     "Server=db31553.public.databaseasp.net;" +
            "Database=db31553;" +
    "User Id=db31553;" +
            "Password=0520ARTU;" +
       "Encrypt=True;" +
            "TrustServerCertificate=True;" +
     "MultipleActiveResultSets=True;";

        // 🔹 Retorna un objeto SqlConnection
  public SqlConnection CrearConexion()
        {
            return new SqlConnection(cadenaConexion);
        }

        // 🔹 Método auxiliar: ejecuta comandos directos
        public void ProbarConexion()
   {
          using (SqlConnection cn = CrearConexion())
      {
    try
            {
            cn.Open();
         Console.WriteLine("Conexión exitosa a la base de datos.");
            }
        catch (Exception ex)
           {
    Console.WriteLine("Error al conectar: " + ex.Message);
        }
    }
        }
    }
}
