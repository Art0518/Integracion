namespace Microservicio.Usuario.DTOs
{
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