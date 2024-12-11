using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Runtime.InteropServices;
using VeterinariaOnlineApi.Core.DTOs.CitaDTOs;
using VeterinariaOnlineApi.Infraestructura.HelperDTOs;
using VeterinariaOnlineApi.Infraestructura.Servicios.CitaServices.Interfaces;

namespace VeterinariaOnlineApi.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CitaController : ControllerBase
    {

        private readonly ICitaServicios _citaServicios;
        private readonly IConfiguration _configuration;
        private readonly ILogger<CitaController> _logger;
        public CitaController(ICitaServicios citaServicios, IConfiguration configuration, ILogger<CitaController> logger)
        {
            _citaServicios = citaServicios;
            _configuration = configuration;
            _logger = logger;
        }


        [HttpPost("SolicitarCita")]
        public async Task<IActionResult> SolicitarCita(CitaSolicitudDTO cita)
        {
            try
            {
                var result = await _citaServicios.SolicitarCita(cita);
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
