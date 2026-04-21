using Microsoft.Data.SqlClient;
using ProyAdoPet.Models;
using ProyAdoPet.Repository;
using System.Data;

namespace ProyAdoPet.DAO

{
    public class SolicitudDAO : ISolicitud
    {
        string cadena = (new ConfigurationBuilder()
                            .AddJsonFile("appsettings.json")
                            .Build())
                            .GetConnectionString("cn") ?? "";

        public bool Solicitar(int idUsuario, int idMascota, string mensaje)
        {
            bool respuesta = false;

            try
            {
                using (SqlConnection cn = new SqlConnection(cadena))
                {
                    SqlCommand cmd = new SqlCommand("sp_SolicitarAdopcion", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@IdUsuario", idUsuario);
                    cmd.Parameters.AddWithValue("@IdMascota", idMascota);
                    cmd.Parameters.AddWithValue("@Mensaje", mensaje ?? (object)DBNull.Value);

                    cn.Open();

                    int filas = cmd.ExecuteNonQuery();

                    if (filas > 0)
                        respuesta = true;
                }
            }
            catch (Exception ex)
            {
                // 🔥 SOLO mostramos el error sin romper tu estructura
                Console.WriteLine("ERROR DAO: " + ex.Message);
                respuesta = false;
            }

            return respuesta;
        }

        public IEnumerable<SolicitudAdopcion> ListarPorUsuario(int idUsuario)
        {
            List<SolicitudAdopcion> lista = new List<SolicitudAdopcion>();

            try
            {
                using (SqlConnection cn = new SqlConnection(cadena))
                {
                    SqlCommand cmd = new SqlCommand("sp_VerEstadoSolicitud", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@IdUsuario", idUsuario);

                    cn.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new SolicitudAdopcion
                            {
                                IdSolicitud = Convert.ToInt32(dr["IdSolicitud"]),
                                Estado = dr["Estado"].ToString()
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR LISTAR: " + ex.Message);
            }

            return lista;
        }
    }
}