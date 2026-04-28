using Microsoft.Data.SqlClient;
using ProyAdoPet.Models;
using ProyAdoPet.Repository;
using System;
using System.Collections.Generic;
using System.Data;

namespace ProyAdoPet.DAO
{
    public class MascotaDAO : IMascota
    {
        string cadena = "Server=.\\SQLEXPRESS;Database=ProyAdoPet;Trusted_Connection=True;TrustServerCertificate=True;";

        // LISTAR
        public List<Mascota> Listado()
        {
            List<Mascota> lista = new List<Mascota>();

            using (SqlConnection cn = new SqlConnection(cadena))
            {
                cn.Open();

                SqlCommand cmd = new SqlCommand("SELECT * FROM Mascota", cn);
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    lista.Add(new Mascota()
                    {
                        Id = Convert.ToInt32(dr["Id"]),
                        Nombre = dr["Nombre"].ToString(),
                        Edad = dr["Edad"].ToString(),
                        Descripcion = dr["Descripcion"].ToString(),
                        Estado = Convert.ToInt32(dr["EstadoId"]),
                        FotoMascota = dr["FotoMascota"] != DBNull.Value
                                      ? dr["FotoMascota"].ToString()
                                      : "sin-foto.jpg"
                    });
                }
            }

            return lista;
        }

        // DETALLE (HU-04)
        public Mascota ObtenerMascotaPorId(int id)
        {
            Mascota obj = null;

            using (SqlConnection cn = new SqlConnection(cadena))
            {
                cn.Open();

                SqlCommand cmd = new SqlCommand("SELECT * FROM Mascota WHERE Id = @id", cn);
                cmd.Parameters.AddWithValue("@id", id);

                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    obj = new Mascota()
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
                }
            }

            return obj;
        }

        // FILTROS (HU-09)
        public List<Mascota> FiltrarMascotas(string? nombre, string? edad, int? estadoId)
        {
            List<Mascota> lista = new List<Mascota>();

            using (SqlConnection cn = new SqlConnection(cadena))
            {
                cn.Open();

                SqlCommand cmd = new SqlCommand(@"
                    SELECT * FROM Mascota
                    WHERE (@nombre IS NULL OR Nombre LIKE '%' + @nombre + '%')
                    AND (@edad IS NULL OR Edad = @edad)
                    AND (@estadoId IS NULL OR EstadoId = @estadoId)", cn);

                cmd.Parameters.AddWithValue("@nombre", (object?)nombre ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@edad", (object?)edad ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@estadoId", (object?)estadoId ?? DBNull.Value);

                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    lista.Add(new Mascota()
                    {
                        Id = Convert.ToInt32(dr["Id"]),
                        Nombre = dr["Nombre"].ToString(),
                        Edad = dr["Edad"].ToString(),
                        Descripcion = dr["Descripcion"].ToString(),
                        Estado = Convert.ToInt32(dr["EstadoId"]),
                        FotoMascota = dr["FotoMascota"] != DBNull.Value
                                      ? dr["FotoMascota"].ToString()
                                      : "sin-foto.jpg"
                    });
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
            catch (Exception ex)
            {
                respuesta = false;
            }
            return respuesta;
        }

        // HU07 → OBTENER
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
                            Estado = Convert.ToInt32(dr["EstadoId"]),
                            FotoMascota = dr["FotoMascota"] != DBNull.Value
                                ? dr["FotoMascota"].ToString()
                                : "sin-foto.jpg"
                        };
                    }
                }
            }

            return mascota;
        }

        // HU07 → ACTUALIZAR
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
            catch
            {
                respuesta = false;
            }

            return respuesta;
        }

        // HU-09: ELIMINAR MASCOTA
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

                    if (filasAfectadas > 0)
                    {
                        respuesta = true;
                    }
                }
            }
            catch (Exception ex)
            {
                respuesta = false;
            }
            return respuesta;
        }
    }
}
