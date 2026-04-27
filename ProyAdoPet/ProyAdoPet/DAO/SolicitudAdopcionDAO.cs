using Microsoft.Data.SqlClient;
using ProyAdoPet.Models;
using ProyAdoPet.Repository;
using ProyAdoPet.ViewModel;
using System.Data;

namespace ProyAdoPet.DAO
{
    public class SolicitudAdopcionDAO : ISolicitudAdopcion
    {
        string cadena = "server=localhost\\SQLEXPRESS;database=ProyAdoPet;Trusted_Connection=true;multipleActiveResultSets=true;TrustServerCertificate=true;Encrypt=false";

        public List<SolicitudAdopcionVM> ListarParaAdmin()
        {
            List<SolicitudAdopcionVM> lista = new List<SolicitudAdopcionVM>();
            using (SqlConnection conexion = new SqlConnection(cadena))
            {
                conexion.Open();
                SqlCommand cmd = new SqlCommand("sp_ListarSolicitudesAdmin", conexion);
                cmd.CommandType = CommandType.StoredProcedure;

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        lista.Add(new SolicitudAdopcionVM
                        {
                            Id = Convert.ToInt32(dr["Id"]),
                            NombrePostulante = dr["NombrePostulante"].ToString(),
                            DNI = dr["DNI"].ToString(),
                            NombreMascota = dr["NombreMascota"].ToString(),
                            FotoMascota = dr["FotoMascota"].ToString(),
                            FechaCreacion = Convert.ToDateTime(dr["FechaCreacion"]),
                            EstadoNombre = dr["EstadoNombre"].ToString(),
                            EstadoId = Convert.ToInt32(dr["EstadoId"])
                        });
                    }
                }
            }
            return lista;
        }

        public EvaluacionSolicitudVM ObtenerDetalleEvaluacion(int id)
        {
            EvaluacionSolicitudVM detalle = null;
            using (SqlConnection conexion = new SqlConnection(cadena))
            {
                conexion.Open();
                SqlCommand cmd = new SqlCommand("sp_ObtenerDetalleSolicitud", conexion);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", id);

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        detalle = new EvaluacionSolicitudVM
                        {
                            SolicitudId = Convert.ToInt32(dr["SolicitudId"]),
                            NombrePostulante = dr["NombrePostulante"].ToString(),
                            DNI = dr["DNI"].ToString(),
                            Telefono = dr["Telefono"].ToString(),
                            Direccion = dr["Direccion"].ToString(),
                            MotivoAdopcion = dr["MotivoAdopcion"].ToString(),
                            MascotaNombre = dr["MascotaNombre"].ToString(),
                            MascotaFoto = dr["FotoMascota"].ToString(),
                            EstadoActualId = Convert.ToInt32(dr["EstadoActualId"]),
                            EstadoNombre = dr["EstadoNombre"].ToString(),

                            // VALIDAMOS SI HAY CITA (pueden venir nulos desde el LEFT JOIN)
                            FechaCita = dr["FechaCita"] != DBNull.Value ? Convert.ToDateTime(dr["FechaCita"]) : DateTime.MinValue,
                            LugarCita = dr["LugarCita"]?.ToString(),
                            NotasCita = dr["NotasCita"]?.ToString()
                        };
                    }
                }
            }
            return detalle;
        }

        public bool ProgramarCita(CitaAdopcion cita)
        {
            bool respuesta = false;
            using (SqlConnection conexion = new SqlConnection(cadena))
            {
                try
                {
                    conexion.Open();
                    SqlCommand cmd = new SqlCommand("sp_RegistrarCitaAdopcion", conexion);
                    cmd.Parameters.AddWithValue("@SolicitudId", cita.SolicitudId);
                    cmd.Parameters.AddWithValue("@FechaCita", cita.FechaCita);
                    cmd.Parameters.AddWithValue("@Lugar", cita.Lugar ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Notas", cita.Notas ?? (object)DBNull.Value);
                    cmd.CommandType = CommandType.StoredProcedure;

                    int filasAfectadas = cmd.ExecuteNonQuery();
                    if (filasAfectadas > 0) respuesta = true;
                }
                catch (Exception ex)
                {
                    respuesta = false;
                }
            }
            return respuesta;
        }

        public bool Registrar(SolicitudAdopcion solicitud)
        {
            bool respuesta = false;
            try
            {
                using (SqlConnection conexion = new SqlConnection(cadena))
                {
                    conexion.Open();
                    SqlCommand cmd = new SqlCommand("sp_RegistrarSolicitud", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@MascotaId", solicitud.MascotaId);
                    cmd.Parameters.AddWithValue("@UsuarioId", solicitud.UsuarioId);
                    cmd.Parameters.AddWithValue("@NombreCompleto", solicitud.NombreCompleto);
                    cmd.Parameters.AddWithValue("@DNI", solicitud.DNI);
                    cmd.Parameters.AddWithValue("@Telefono", solicitud.Telefono);
                    cmd.Parameters.AddWithValue("@Direccion", solicitud.Direccion);
                    cmd.Parameters.AddWithValue("@MotivoAdopcion", solicitud.MotivoAdopcion);

                    int filas = cmd.ExecuteNonQuery();
                    if (filas > 0) respuesta = true;
                }
            }
            catch (Exception)
            {
                respuesta = false;
            }
            return respuesta;
        }

        public bool YaTieneSolicitud(int mascotaId, int usuarioId)
        {
            bool existe = false;
            using (SqlConnection conexion = new SqlConnection(cadena))
            {
                conexion.Open();
                SqlCommand cmd = new SqlCommand("sp_ExisteSolicitudUsuario", conexion);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MascotaId", mascotaId);
                cmd.Parameters.AddWithValue("@UsuarioId", usuarioId);

                int conteo = (int)cmd.ExecuteScalar();
                if (conteo > 0) existe = true;
            }
            return existe;
        }
    }
}
