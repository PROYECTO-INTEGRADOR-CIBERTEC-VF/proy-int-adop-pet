using Microsoft.Data.SqlClient;
using ProyAdoPet.Models;
using ProyAdoPet.Repository;
using System.Data;

namespace ProyAdoPet.DAO
{
    public class UsuarioDAO : IUsuario
    {
        string cadena = (new ConfigurationBuilder().AddJsonFile("appsettings.json").Build()).GetConnectionString("cn") ?? "";

        public string Registrar(Usuario oUsuario)
        {
            string mensaje;
            try
            {
                using (SqlConnection cn = new SqlConnection(cadena))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_RegistrarUsuario", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Nombre", oUsuario.Nombre);
                        cmd.Parameters.AddWithValue("@Correo", oUsuario.Correo);
                        cmd.Parameters.AddWithValue("@Clave", oUsuario.Clave);

                        //parametro de salida
                        SqlParameter registradoParam = new SqlParameter("@Registrado", SqlDbType.Bit)
                        {
                            Direction = ParameterDirection.Output
                        };
                        cmd.Parameters.Add(registradoParam);

                        cn.Open();
                        cmd.ExecuteNonQuery();

                        //leer el valor 1 = true 0 = false)
                        bool registrado = (bool)cmd.Parameters["@Registrado"].Value;

                        mensaje = registrado ? "OK" : "El correo ya se encuentra registrado.";
                    }
                }
            }
            catch (Exception ex)
            {
                mensaje = "Error en el servidor: " + ex.Message;
            }
            return mensaje;
        }

        public Usuario ValidarUsuario(string correo, string clave)
        {
            throw new NotImplementedException();
        }
    }
}
