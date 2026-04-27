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
                                Estado = dr["Estado"].ToString(),
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
    }
}
