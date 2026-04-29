using Microsoft.Data.SqlClient;
using ProyAdoPet.Repository;
using ProyAdoPet.ViewModel;
using System.Data;

namespace ProyAdoPet.DAO
{
    public class SeguimientoDAO : ISeguimiento
    {

        string cadena = (new ConfigurationBuilder().AddJsonFile("appsettings.json").Build()).GetConnectionString("cn") ?? "";


        public List<SeguimientoListaVM> ListarAdopcionesEnSeguimiento()
        {
            List<SeguimientoListaVM> lista = new List<SeguimientoListaVM>();
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("sp_ListarAdopcionesEnSeguimiento", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cn.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        // Dentro del while (dr.Read())
                        lista.Add(new SeguimientoListaVM
                        {
                            SolicitudId = (int)dr["SolicitudId"],
                            Adoptante = dr["Adoptante"].ToString(),
                            Mascota = dr["Mascota"].ToString(),
                            FotoMascota = dr["FotoMascota"].ToString(),
                            CodigoContrato = dr["CodigoContrato"].ToString(),
                            UltimoControl = dr["UltimoControl"] != DBNull.Value ? (DateTime?)dr["UltimoControl"] : null
                        });
                    }
                }
            }
            return lista;
        }

        public List<SeguimientoItemVM> ListarControlesPorSolicitud(int solicitudId)
        {
            List<SeguimientoItemVM> lista = new List<SeguimientoItemVM>();
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("sp_ObtenerHistorialSeguimiento", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SolicitudId", solicitudId);
                cn.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        lista.Add(new SeguimientoItemVM
                        {
                            Id = (int)dr["Id"],

                            //datos programcion cita
                            FechaProgramada = (DateTime)dr["FechaProgramada"],
                            TipoControl = dr["TipoControl"].ToString(),
                            Responsable = dr["Responsable"].ToString(),
                            ObservacionInicial = dr["ObservacionInicial"].ToString(),
                            EstadoVisita = dr["EstadoVisita"].ToString(),

                            //datos resultado (pueden ser null)
                            FechaRealizada = dr["FechaRealizada"] != DBNull.Value ? (DateTime?)dr["FechaRealizada"] : null,
                            Resultado = dr["Resultado"] != DBNull.Value ? dr["Resultado"].ToString() : null,
                            Comentarios = dr["Comentarios"] != DBNull.Value ? dr["Comentarios"].ToString() : null,
                            FotoEvidencia = dr["FotoEvidencia"] != DBNull.Value ? dr["FotoEvidencia"].ToString() : null
                        });
                    }
                }
            }
            return lista;
        }
    }
}
