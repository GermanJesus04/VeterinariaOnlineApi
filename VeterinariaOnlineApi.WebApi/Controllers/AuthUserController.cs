using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VeterinariaOnlineApi.Core.DTOs.DueñoDTOs;
using VeterinariaOnlineApi.Infraestructura.Servicios.AuthServices.Interfaces;

namespace VeterinariaOnlineApi.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthUserController : ControllerBase
    {
        private readonly IAuthServicios _authServicios;

        public AuthUserController(IAuthServicios authServicios)
        {
            _authServicios = authServicios;
        }

        [HttpPost]
        public async Task<IActionResult> RegistrarUser(DueñoRegistrarDTO dueñoDto)
        {
            try
            {
                var result = await _authServicios.RegistrarDueño(dueñoDto);
                return Ok(result);

            }catch (Exception ex)
            {
                throw;
            }
        }

    }
}
