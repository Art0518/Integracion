using System.Text.RegularExpressions;

namespace Logica.Validaciones
{
    public static class ValidacionUsuario
    {
        // Validar formato de correo
        public static bool EmailValido(string email)
        {
            if (string.IsNullOrEmpty(email)) return false;
            return Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        }
    }
}
