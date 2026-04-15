using ProyAdoPet.DAO;
using ProyAdoPet.Models;
using ProyAdoPet.Repository;

namespace ProyAdoPet.Services
{
    public class UsuarioService
    {
        IUsuario _usuario;

        public UsuarioService(IUsuario usuarioDAO)
        {
            _usuario = usuarioDAO;
        }

        public string Registrar(Usuario usuario)
        {
            //validaciones
            if (string.IsNullOrEmpty(usuario.Nombre) || string.IsNullOrEmpty(usuario.Correo))
            {
                return "Todos los campos son obligatorios.";
            }

            if (usuario.Clave.Length < 8)
            {
                return "La contraseña debe tener al menos 8 caracteres.";
            }

            //si todo pasa correcto
            return _usuario.Registrar(usuario);
        }

        public Usuario ValidarUsuario(string correo, string clave)
        {
            //validacion
            if (string.IsNullOrEmpty(correo) || string.IsNullOrEmpty(clave))
            {
                return null;
            }

            //si es correcto
            return _usuario.ValidarUsuario(correo, clave);
        }
    }
}
