using AutoMapper;
using VeterinariaOnlineApi.Infraestructura.Data;
using VeterinariaOnlineApi.Infraestructura.Servicios.CitaServices.Interfaces;

namespace VeterinariaOnlineApi.Infraestructura.Servicios.CitaServices
{
    public class CitaServicios: ICitaServicios
    {
        private readonly VeterinariaDbContext _context;
        private readonly IMapper _mapeo;
        public CitaServicios(VeterinariaDbContext context, IMapper mapeo)
        {
            _context = context;
            _mapeo = mapeo;
        }

        //solicitar Cita (Dueño)
        public async Task<object> SolicitarCita()
        {

        }

    }
}
