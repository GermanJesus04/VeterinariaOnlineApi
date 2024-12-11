using AutoMapper;
using VeterinariaOnlineApi.Core.DTOs.CitaDTOs;
using VeterinariaOnlineApi.Core.Models;
using VeterinariaOnlineApi.Infraestructura.Data;
using VeterinariaOnlineApi.Infraestructura.HelperDTOs;
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
        public async Task<RespuestaWebApi<string>> SolicitarCita(CitaSolicitudDTO citaDto)
        {
            try
            {
                var mascotaExiste = await _context.Mascotas.FindAsync(citaDto.MascotaId);

                if (mascotaExiste is null)
                    throw new ExcepcionPeticionApi("Mascota no encontrada", 400);

                var cita = _mapeo.Map<Cita>(citaDto);
                cita.Estado = Estado.Pendiente;
                cita.FechaCreacion = DateTime.UtcNow;
                cita.UsuarioCreacion = "NA";

                 _context.Citas.Add(cita);
                await _context.SaveChangesAsync();

                return new RespuestaWebApi<string>{
                    Data = "Solicitud de Cita Enviada Correctamente"
                };
            }
            catch (Exception ex)
            {
                throw;
            }

        }

    }
}
