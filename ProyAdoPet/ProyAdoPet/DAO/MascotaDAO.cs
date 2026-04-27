using Microsoft.Data.SqlClient;
using ProyAdoPet.Models;
using ProyAdoPet.Repository;
using System;
using System.Collections.Generic;

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
                        Tipo = dr["Tipo"].ToString(),
                        Tamaño = dr["Tamaño"].ToString(),
                        Descripcion = dr["Descripcion"].ToString(),
                        Estado = dr["Estado"].ToString(),
                        FotoMascota = dr["FotoMascota"]?.ToString()
                    });
                }
            }

            return lista;
        }

        // DETALLE
        public Mascota ObtenerMascotaPorId(int id)
        {
            Mascota obj = null;

            using (SqlConnection cn = new SqlConnection(cadena))
            {
                cn.Open();

                SqlCommand cmd = new SqlCommand(
                    "SELECT * FROM Mascota WHERE Id = @id", cn);

                cmd.Parameters.AddWithValue("@id", id);

                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    obj = new Mascota()
                    {
                        Id = Convert.ToInt32(dr["Id"]),
                        Nombre = dr["Nombre"].ToString(),
                        Edad = dr["Edad"].ToString(),
                        Tipo = dr["Tipo"].ToString(),
                        Tamaño = dr["Tamaño"].ToString(),
                        Descripcion = dr["Descripcion"].ToString(),
                        Estado = dr["Estado"].ToString(),
                        FotoMascota = dr["FotoMascota"]?.ToString()
                    };
                }
            }

            return obj;
        }

        // FILTROS
        public List<Mascota> FiltrarMascotas(int? edad, string tipo, string tamaño)
        {
            List<Mascota> lista = new List<Mascota>();

            using (SqlConnection cn = new SqlConnection(cadena))
            {
                cn.Open();

                SqlCommand cmd = new SqlCommand(@"
                    SELECT * FROM Mascota
                    WHERE (@edad IS NULL OR Edad = @edad)
                    AND (@tipo IS NULL OR Tipo = @tipo)
                    AND (@tamaño IS NULL OR [Tamaño] = @tamaño)", cn);

                cmd.Parameters.AddWithValue("@edad", (object)edad ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@tipo", (object)tipo ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@tamaño", (object)tamaño ?? DBNull.Value);

                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    lista.Add(new Mascota()
                    {
                        Id = Convert.ToInt32(dr["Id"]),
                        Nombre = dr["Nombre"].ToString(),
                        Edad = dr["Edad"].ToString(),
                        Tipo = dr["Tipo"].ToString(),
                        Tamaño = dr["Tamaño"].ToString(),
                        Descripcion = dr["Descripcion"].ToString(),
                        Estado = dr["Estado"].ToString(),
                        FotoMascota = dr["FotoMascota"]?.ToString()
                    });
                }
            }

            return lista;
        }
    }
}