using ProyAdoPet.Models;

namespace ProyAdoPet.Repository
{
    public interface IUsuario
    {
        string Registrar(Usuario oUsuario);
        Usuario ValidarUsuario(string correo, string clave);
    }
}
