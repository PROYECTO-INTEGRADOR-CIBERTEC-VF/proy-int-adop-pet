using Microsoft.Data.SqlClient;
using ProyAdoPet.Repository;
using ProyAdoPet.ViewModel;
using System.Data;

namespace ProyAdoPet.DAO
{
    public class AdopcionDAO : IAdopcion
    {
        string cadena = (new ConfigurationBuilder().AddJsonFile("appsettings.json").Build()).GetConnectionString("cn") ?? "";

        public ActaAdopcionVM ObtenerDatosActa(int solicitudId)
        {
            ActaAdopcionVM acta = null;
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("sp_ObtenerDatosActa", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", solicitudId);
                cn.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        acta = new ActaAdopcionVM
                        {
                            Folio = Convert.ToInt32(dr["Folio"]),
                            CodigoContrato = dr["CodigoContrato"].ToString(),
                            AdoptanteNombre = dr["AdoptanteNombre"].ToString(),
                            AdoptanteDNI = dr["AdoptanteDNI"].ToString(),
                            AdoptanteDireccion = dr["AdoptanteDireccion"].ToString(),
                            MascotaNombre = dr["MascotaNombre"].ToString(),
                            FechaEmision = Convert.ToDateTime(dr["FechaEmision"]),
                            ObservacionesIniciales = dr["ObservacionesIniciales"]?.ToString()
                        };
                    }
                }
            }
            return acta;
        }

        public IEnumerable<MisSolicitudesVM> ObtenerMisSolicitudes(int usuarioId)
        {
            List<MisSolicitudesVM> lista = new List<MisSolicitudesVM>();
            using (SqlConnection conexion = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("sp_ListarSolicitudesPorUsuario", conexion);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UsuarioId", usuarioId);

                conexion.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        lista.Add(new MisSolicitudesVM
                        {
                            SolicitudId = Convert.ToInt32(dr["SolicitudId"]),
                            MascotaNombre = dr["MascotaNombre"].ToString(),
                            MascotaFoto = dr["MascotaFoto"].ToString(),
                            EstadoId = Convert.ToInt32(dr["EstadoId"]),
                            EstadoNombre = dr["EstadoNombre"].ToString(),
                            FechaEnvio = Convert.ToDateTime(dr["FechaEnvio"]),
                            //estos pueden ser null si no hay cita todavia
                            FechaCita = dr["FechaCita"] != DBNull.Value ? Convert.ToDateTime(dr["FechaCita"]) : (DateTime?)null,
                            LugarCita = dr["LugarCita"]?.ToString(),
                            NotasCita = dr["NotasCita"]?.ToString()
                        });
                    }
                }
            }
            return lista;
        }
    }
}
