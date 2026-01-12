namespace Microservicio.Usuario.DTOs
{
    public class UsuarioCreadoResponseDTO
    {
        public int id { get; set; }
        public string mensaje { get; set; } = string.Empty;
        public string nombre { get; set; } = string.Empty;
        public string apellido { get; set; } = string.Empty;
        public string email { get; set; } = string.Empty;
        public string tipo_identificacion { get; set; } = string.Empty;
        public string identificacion { get; set; } = string.Empty;
        public string _links { get; set; } = string.Empty;
    }
}
