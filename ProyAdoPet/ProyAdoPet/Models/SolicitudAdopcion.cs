namespace ProyAdoPet.Models
{
    public class SolicitudAdopcion
    {
        public int IdSolicitud { get; set; }
        public int IdUsuario { get; set; }
        public int IdMascota { get; set; }
        public string Mensaje { get; set; }
        public string Estado { get; set; }
    }
}