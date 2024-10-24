using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VeterinariaOnlineApi.Core.DTOs.AuthDTOs;
using VeterinariaOnlineApi.Core.DTOs.DueñoDTOs;
using VeterinariaOnlineApi.Infraestructura.HelperDTOs;

namespace VeterinariaOnlineApi.Infraestructura.Servicios.AuthServices.Interfaces
{
    public interface IAuthServicios
    {
        Task<RespuestaWebApi<AuthResultDTO>> LoginDueño(DueñoLoginDTO loginDTO);
        Task<RespuestaWebApi<AuthResultDTO>> RefreshToken(RequestTokenCaducadoDTO refreshtoken);
        Task<RespuestaWebApi<AuthResultDTO>> RegistrarDueño(DueñoRegistrarDTO dueñoDto);
    }
}
