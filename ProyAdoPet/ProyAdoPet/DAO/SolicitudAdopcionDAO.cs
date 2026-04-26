using Microsoft.Data.SqlClient;
using ProyAdoPet.Models;
using ProyAdoPet.Repository;
using System.Data;

namespace ProyAdoPet.DAO
{
    public class SolicitudAdopcionDAO : ISolicitudAdopcion
    {
        string cadena = (new ConfigurationBuilder().AddJsonFile("appsettings.json").Build()).GetConnectionString("cn") ?? "";


        public bool Registrar(SolicitudAdopcion solicitud)
        {
            bool respuesta = false;
            try
            {
                using (SqlConnection conexion = new SqlConnection(cadena))
                {
                    conexion.Open();
                    SqlCommand cmd = new SqlCommand("sp_RegistrarSolicitud", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@MascotaId", solicitud.MascotaId);
                    cmd.Parameters.AddWithValue("@UsuarioId", solicitud.UsuarioId);
                    cmd.Parameters.AddWithValue("@NombreCompleto", solicitud.NombreCompleto);
                    cmd.Parameters.AddWithValue("@DNI", solicitud.DNI);
                    cmd.Parameters.AddWithValue("@Telefono", solicitud.Telefono);
                    cmd.Parameters.AddWithValue("@Direccion", solicitud.Direccion);
                    cmd.Parameters.AddWithValue("@MotivoAdopcion", solicitud.MotivoAdopcion);

                    int filas = cmd.ExecuteNonQuery();
                    if (filas > 0) respuesta = true;
                }
            }
            catch (Exception)
            {
                respuesta = false;
            }
            return respuesta;
        }
    }
}
