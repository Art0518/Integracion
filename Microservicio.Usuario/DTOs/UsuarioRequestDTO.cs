namespace Microservicio.Usuario.DTOs
{
    public class UsuarioRequestDTO
    {
        public string nombre { get; set; } = string.Empty;
        public string apellido { get; set; } = string.Empty;
        public string email { get; set; } = string.Empty;
        public string tipo_identificacion { get; set; } = string.Empty;
        public string identificacion { get; set; } = string.Empty;
    }
}
