using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VeterinariaOnlineApi.Core.DTOs.DueñoDTOs;
using VeterinariaOnlineApi.Infraestructura.HelperDTOs;

namespace VeterinariaOnlineApi.Infraestructura.Servicios.DueñoServices.Interfaces
{
    public interface IDueñoServicios
    {
        Task<RespuestaWebApi<bool>> ActualizarUser(string id, DueñoActualizarDTO dueño);
        Task<RespuestaWebApi<string>> HardDeleteUser(string id);
        Task<RespuestaWebApi<List<DueñoResponseDTO>>> ListarUser(string? id);
        Task<RespuestaWebApi<DueñoResponseDTO>> ObtenerUser(string id);
        Task<RespuestaWebApi<string>> SoftDeleteUser(string id);
    }
}
