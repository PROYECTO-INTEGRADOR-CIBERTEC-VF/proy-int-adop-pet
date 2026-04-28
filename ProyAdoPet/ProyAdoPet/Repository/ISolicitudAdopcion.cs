using ProyAdoPet.Models;
using ProyAdoPet.ViewModel;

namespace ProyAdoPet.Repository
{
    public interface ISolicitudAdopcion
    {
        public bool YaTieneSolicitud(int mascotaId, int usuarioId);
        bool Registrar(SolicitudAdopcion solicitud);

        //METODOS PARA EL ADMIN
        List<SolicitudAdopcionVM> ListarParaAdmin();
        EvaluacionSolicitudVM ObtenerDetalleEvaluacion(int id);
        bool ProgramarCita(CitaAdopcion cita);
        ContratoAdopcionVM FinalizarAdopcion(int solicitudId, string Observaciones);
        public ContratoAdopcionVM ObtenerContratoPorSolicitud(int solicitudId);
        bool RechazarSolicitud(int solicitudId);
    }
}
