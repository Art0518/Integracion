namespace ApiGateway.DTOs.Usuario
{
    public class UsuarioRequestDTO
    {
        public string nombre { get; set; } = string.Empty;
        public string apellido { get; set; } = string.Empty;
        public string email { get; set; } = string.Empty;
        public string tipo_identificacion { get; set; } = string.Empty;
        public string identificacion { get; set; } = string.Empty;
    }

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

    public class UsuarioListarResponseDTO
    {
        public int idUsuario { get; set; }
        public string nombre { get; set; } = string.Empty;
        public string apellido { get; set; } = string.Empty;
        public string email { get; set; } = string.Empty;
        public string rol { get; set; } = string.Empty;
    }

    public class UsuarioPaginadoResponseDTO
    {
        public List<UsuarioListarResponseDTO> usuarios { get; set; } = new List<UsuarioListarResponseDTO>();
        public int totalUsuarios { get; set; }
        public int paginaActual { get; set; }
        public int totalPaginas { get; set; }
        public int tamanoPagina { get; set; }
        public bool tienePaginaAnterior { get; set; }
        public bool tienePaginaSiguiente { get; set; }
        public string _links { get; set; } = string.Empty;
    }
}
