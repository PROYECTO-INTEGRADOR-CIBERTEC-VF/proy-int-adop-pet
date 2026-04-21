using Microsoft.Data.SqlClient;
using ProyAdoPet.Models;
using System.Data;

namespace ProyAdoPet.DAO
{
    public class MascotaAdminDAO
    {
        string cadena = (new ConfigurationBuilder()
                        .AddJsonFile("appsettings.json")
                        .Build())
                        .GetConnectionString("cn") ?? "";

        public IEnumerable<Mascota> Listado()
        {
            List<Mascota> lista = new List<Mascota>();

            using (SqlConnection cn = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("sp_ListarMascotas", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cn.Open();

                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    lista.Add(new Mascota
                    {
                        Id = Convert.ToInt32(dr["Id"]),
                        Nombre = dr["Nombre"].ToString(),
                        Edad = dr["Edad"].ToString(),
                        Descripcion = dr["Descripcion"].ToString(),
                        Estado = Convert.ToInt32(dr["Estado"]),
                        FotoMascota = dr["FotoMascota"].ToString()
                    });
                }
            }

            return lista;
        }

        public Mascota ObtenerPorId(int id)
        {
            Mascota obj = null;

            using (SqlConnection cn = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("sp_ObtenerMascota", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Id", id);

                cn.Open();

                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    obj = new Mascota
                    {
                        Id = Convert.ToInt32(dr["Id"]),
                        Nombre = dr["Nombre"].ToString(),
                        Edad = dr["Edad"].ToString(),
                        Descripcion = dr["Descripcion"].ToString(),
                        Estado = Convert.ToInt32(dr["Estado"]),
                        FotoMascota = dr["FotoMascota"].ToString()
                    };
                }
            }

            return obj;
        }

        public bool Actualizar(Mascota obj)
        {
            bool ok = false;

            using (SqlConnection cn = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("sp_ActualizarMascota", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Id", obj.Id);
                cmd.Parameters.AddWithValue("@Nombre", obj.Nombre);
                cmd.Parameters.AddWithValue("@Edad", obj.Edad ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Descripcion", obj.Descripcion ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Estado", obj.Estado);
                cmd.Parameters.AddWithValue("@FotoMascota", obj.FotoMascota ?? (object)DBNull.Value);

                cn.Open();
                ok = cmd.ExecuteNonQuery() > 0;
            }

            return ok;
        }
    }
}

