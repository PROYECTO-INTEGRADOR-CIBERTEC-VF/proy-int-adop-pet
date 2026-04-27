using Microsoft.Data.SqlClient;
using ProyAdoPet.Models;
using ProyAdoPet.Repository;
using System.Data;

namespace ProyAdoPet.DAO
{
    public class MascotaDAO : IMascota
    {
        string cadena = "server=localhost\\SQLEXPRESS;database=ProyAdoPet;Trusted_Connection=true;multipleActiveResultSets=true;TrustServerCertificate=true;Encrypt=false";

        // Lee el estado como int sin importar si la columna se llama EstadoId o Estado
        private static int LeerEstado(SqlDataReader dr)
        {
            try { return Convert.ToInt32(dr["EstadoId"]); } catch { }
            try
            {
                var val = dr["Estado"];
                if (val == DBNull.Value) return 1;
                if (int.TryParse(val.ToString(), out int n)) return n;
                return val.ToString()?.ToLower().Contains("disp") == true ? 1 : 2;
            }
            catch { return 1; }
        }

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
                            lista.Add(new Mascota
                            {
                                Id = Convert.ToInt32(dr["Id"]),
                                Nombre = dr["Nombre"].ToString(),
                                Edad = dr["Edad"].ToString(),
                                Descripcion = dr["Descripcion"].ToString(),
                                Estado = LeerEstado(dr),
                                FotoMascota = dr["FotoMascota"] != DBNull.Value
                                              ? dr["FotoMascota"].ToString()
                                              : "sin-foto.jpg"
                            });
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

        public bool Registrar(Mascota objeto)
        {
            bool respuesta = false;
            try
            {
                using (var conexion = new SqlConnection(cadena))
                {
                    conexion.Open();
                    SqlCommand cmd = new SqlCommand("sp_RegistrarMascota", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Nombre", objeto.Nombre);
                    cmd.Parameters.AddWithValue("@Edad", objeto.Edad ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Descripcion", objeto.Descripcion ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@EstadoId", objeto.Estado);
                    cmd.Parameters.AddWithValue("@FotoMascota", objeto.FotoMascota ?? (object)DBNull.Value);
                    int filasAfectadas = cmd.ExecuteNonQuery();
                    if (filasAfectadas > 0) respuesta = true;
                }
            }
            catch { respuesta = false; }
            return respuesta;
        }

        public Mascota Obtener(int id)
        {
            Mascota mascota = null;
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("sp_ObtenerMascota", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", id);
                cn.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        mascota = new Mascota
                        {
                            Id = Convert.ToInt32(dr["Id"]),
                            Nombre = dr["Nombre"].ToString(),
                            Edad = dr["Edad"].ToString(),
                            Descripcion = dr["Descripcion"].ToString(),
                            Estado = LeerEstado(dr),
                            FotoMascota = dr["FotoMascota"] != DBNull.Value
                                ? dr["FotoMascota"].ToString()
                                : "sin-foto.jpg"
                        };
                    }
                }
            }
            return mascota;
        }

        public bool Actualizar(Mascota obj)
        {
            bool respuesta = false;
            try
            {
                using (SqlConnection cn = new SqlConnection(cadena))
                {
                    cn.Open();
                    SqlCommand cmd = new SqlCommand("sp_ActualizarMascota", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Id", obj.Id);
                    cmd.Parameters.AddWithValue("@Nombre", obj.Nombre);
                    cmd.Parameters.AddWithValue("@Edad", obj.Edad ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Descripcion", obj.Descripcion ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Estado", obj.Estado);
                    cmd.Parameters.AddWithValue("@FotoMascota", obj.FotoMascota ?? (object)DBNull.Value);
                    int filas = cmd.ExecuteNonQuery();
                    if (filas > 0) respuesta = true;
                }
            }
            catch { respuesta = false; }
            return respuesta;
        }

        public bool Eliminar(int id)
        {
            bool respuesta = false;
            try
            {
                using (SqlConnection conexion = new SqlConnection(cadena))
                {
                    conexion.Open();
                    SqlCommand cmd = new SqlCommand("sp_EliminarMascota", conexion);
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.CommandType = CommandType.StoredProcedure;
                    int filasAfectadas = cmd.ExecuteNonQuery();
                    if (filasAfectadas > 0) respuesta = true;
                }
            }
            catch { respuesta = false; }
            return respuesta;
        }

        // HU-09: Filtrar mascotas
        public IEnumerable<Mascota> Filtrar(string? nombre, string? edad, int? estadoId)
        {
            var lista = new List<Mascota>();
            using (var cn = new SqlConnection(cadena))
            {
                using (var cmd = new SqlCommand("sp_FiltrarMascotas", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Nombre", (object?)nombre ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Edad", (object?)edad ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@EstadoId", (object?)estadoId ?? DBNull.Value);
                    cn.Open();
                    using (var dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new Mascota
                            {
                                Id = Convert.ToInt32(dr["Id"]),
                                Nombre = dr["Nombre"].ToString(),
                                Edad = dr["Edad"].ToString(),
                                Descripcion = dr["Descripcion"].ToString(),
                                Estado = LeerEstado(dr),
                                FotoMascota = dr["FotoMascota"] != DBNull.Value
                                              ? dr["FotoMascota"].ToString()
                                              : "sin-foto.jpg"
                            });
                        }
                    }
                }
            }
            return lista;
        }
    }
}
