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
                        lista.Add(new SeguimientoListaVM
                        {
                            SolicitudId = (int)dr["SolicitudId"],
                            Adoptante = dr["Adoptante"].ToString(),
                            DNI = dr["DNI"].ToString(),
                            Telefono = dr["Telefono"].ToString(),
                            Mascota = dr["Mascota"].ToString(),
                            CodigoContrato = dr["CodigoContrato"].ToString(),
                            FechaInicio = (DateTime)dr["FechaInicio"],
                            UltimoControl = dr["UltimoControl"] != DBNull.Value ? (DateTime?)dr["UltimoControl"] : null
                        });
                    }
                }
            }
            return lista;
        }
    }
}
