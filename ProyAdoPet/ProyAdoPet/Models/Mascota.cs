namespace ProyAdoPet.Models
{
    public class Mascota
    {
        public int Id { get; set; }
        public string? Nombre { get; set; }
        public int Edad { get; set; }
        public string? Descripcion { get; set; }
        public string? Estado { get; set; }
        public string? FotoMascota { get; set; }
    }
}
