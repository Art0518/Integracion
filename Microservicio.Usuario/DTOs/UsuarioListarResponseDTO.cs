namespace Microservicio.Usuario.DTOs
{
    public class UsuarioListarResponseDTO
    {
     public int idUsuario { get; set; }
        public string nombre { get; set; } = string.Empty;
     public string apellido { get; set; } = string.Empty;
        public string email { get; set; } = string.Empty;
   public string rol { get; set; } = string.Empty;
    }
}