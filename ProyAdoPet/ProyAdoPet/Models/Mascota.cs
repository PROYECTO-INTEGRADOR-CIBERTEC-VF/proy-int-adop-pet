namespace ProyAdoPet.Models
{
    public class Mascota
    {
        public int Id { get; set; }
        public string? Nombre { get; set; }
        public string? Edad { get; set; }
        public string? Descripcion { get; set; }
        public int Estado { get; set; }
        public string? FotoMascota { get; set; }
    }
}
