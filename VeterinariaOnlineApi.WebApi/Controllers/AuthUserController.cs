using Microsoft.AspNetCore.Mvc;
using VeterinariaOnlineApi.Core.DTOs.AuthDTOs;
using VeterinariaOnlineApi.Core.DTOs.DueñoDTOs;
using VeterinariaOnlineApi.Infraestructura.HelperDTOs;
using VeterinariaOnlineApi.Infraestructura.Servicios.AuthServices.Interfaces;

namespace VeterinariaOnlineApi.WebApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthUserController : ControllerBase
    {
        private readonly IAuthServicios _authServicios;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthUserController> _logger;

        public AuthUserController(IAuthServicios authServicios, IConfiguration configuration, ILogger<AuthUserController> logger)
        {
            _authServicios = authServicios;
            _configuration = configuration;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> RegistrarUser(DueñoRegistrarDTO dueñoDto)
        {
            try
            {
                var result = await _authServicios.RegistrarDueño(dueñoDto);
                return Ok(result);

            }
            catch (ExcepcionPeticionApi ex)
            {
                return StatusCode(ex.CodigoError, new RespuestaWebApi<object>
                {
                    Exitoso = false,
                    Mensaje = ex.Message
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, _configuration.GetSection("MensajeErrorInterno").Value);
                return StatusCode(500, new RespuestaWebApi<object>
                {
                    Exitoso = false,
                    Mensaje = "Ejecucion No Exitosa. Error en la ejecucion del proceso"
                });

            }
        }

        [HttpGet("login")]
        public async Task<IActionResult> Login(DueñoLoginDTO userDto)
        {
            try
            {
                var result = await _authServicios.LoginDueño(userDto);
                return Ok(result);

            }
            catch (ExcepcionPeticionApi ex)
            {
                return StatusCode(ex.CodigoError, new RespuestaWebApi<object>
                {
                    Exitoso = false,
                    Mensaje = ex.Message
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, _configuration.GetSection("MensajeErrorInterno").Value);
                return StatusCode(500, new RespuestaWebApi<object>
                {
                    Exitoso = false,
                    Mensaje = "Ejecucion No Exitosa. Error en la ejecucion del proceso"
                });

            }
        }

        [HttpGet("refresh")]
        public async Task<IActionResult> RefreshToken(RequestTokenCaducadoDTO refresToken)
        {
            try
            {
                var result = await _authServicios.RefreshToken(refresToken);
                return Ok(result);
            }
            catch (ExcepcionPeticionApi ex)
            {
                return StatusCode(ex.CodigoError, new RespuestaWebApi<object>
                {
                    Exitoso = false,
                    Mensaje = ex.Message
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, _configuration.GetSection("MensajeErrorInterno").Value);
                return StatusCode(500, new RespuestaWebApi<object>
                {
                    Exitoso = false,
                    Mensaje = "Ejecucion No Exitosa. Error en la ejecucion del proceso"
                });

            }
        }




    }
}
