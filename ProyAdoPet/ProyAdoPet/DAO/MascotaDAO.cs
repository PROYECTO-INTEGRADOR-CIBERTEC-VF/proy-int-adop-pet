using Microsoft.Data.SqlClient;
using ProyAdoPet.Models;
using ProyAdoPet.Repository;
using System.Data;

namespace ProyAdoPet.DAO
{
    public class MascotaDAO : IMascota
    {
        string cadena = (new ConfigurationBuilder().AddJsonFile("appsettings.json").Build()).GetConnectionString("cn") ?? "";


        public IEnumerable<Mascota> listado()
        {
            var lista = new List<Mascota>();
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                using (SqlCommand cmd = new SqlCommand("sp_ListarMascotas", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cn.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            var mascota = new Mascota
                            {
                                Id = Convert.ToInt32(dr["Id"]),
                                Nombre = dr["Nombre"].ToString(),
                                Edad = dr["Edad"].ToString(),
                                Descripcion = dr["Descripcion"].ToString(),
                                Estado = Convert.ToInt32(dr["EstadoId"]),
                                FotoMascota = dr["FotoMascota"] != DBNull.Value
                                              ? dr["FotoMascota"].ToString()
                                              : "sin-foto.jpg"
                            };
                            lista.Add(mascota);
                        }
                    }
                }
            }
            return lista;
        }

        public IEnumerable<Estado> ListarEstado()
        {
            List<Estado> lista = new List<Estado>();

            using (var conexion = new SqlConnection(cadena))
            {
                conexion.Open();
                SqlCommand cmd = new SqlCommand("sp_ListarEstados", conexion);
                cmd.CommandType = CommandType.StoredProcedure;

                using (var dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        lista.Add(new Estado
                        {
                            Id = Convert.ToInt32(dr["Id"]),
                            EstadoNombre = dr["Nombre"].ToString()!
                        });
                    }
                }
            }
            return lista;
        }
    }
}
