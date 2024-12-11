using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VeterinariaOnlineApi.Core.DTOs.CitaDTOs;
using VeterinariaOnlineApi.Infraestructura.HelperDTOs;

namespace VeterinariaOnlineApi.Infraestructura.Servicios.CitaServices.Interfaces
{
    public interface ICitaServicios
    {
        Task<RespuestaWebApi<string>> SolicitarCita(CitaSolicitudDTO citaDto);
    }
}
