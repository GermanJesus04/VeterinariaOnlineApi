using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using VeterinariaOnlineApi.Core.DTOs.DueñoDTOs;
using VeterinariaOnlineApi.Infraestructura.HelperDTOs;
using VeterinariaOnlineApi.Infraestructura.Servicios.DueñoServices.Interfaces;

namespace VeterinariaOnlineApi.WebApi.Controllers
{
    [EnableCors("ReglasCors")]
    [SwaggerTag("Servicio encargado de Gestionar Libros.")]
    [SwaggerResponse(400, "Ejecución no exitosa. No se obtuvieron datos correctos.", typeof(RespuestaWebApi<object>))]
    [SwaggerResponse(500, "Ejecución No exitosa. Fallo al lado del servidor.", typeof(RespuestaWebApi<object>))]
    [Route("[controller]")]
    [ApiController]
    public class DueñoController : ControllerBase
    {
        private readonly IDueñoServicios _dueñoServicios;
        public DueñoController(IDueñoServicios dueñoServicios)
        {
            _dueñoServicios = dueñoServicios;
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, 
            Roles = "Dueño")]
        [HttpGet("ObtenerUser")]
        public async Task<IActionResult> ObtenerUser(string id)
        {
            try
            {
                var result = await _dueñoServicios.ObtenerUser(id);
                return Ok(result);

            }catch (Exception ex)
            {
                throw;
            }
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, 
            Roles = "Admin")]
        [HttpGet("Listar")]
        public async Task<IActionResult> ListarUsers(string? id)
        {
            try
            {
                var result = await _dueñoServicios.ListarUser(id);
                return Ok(result);

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, 
            Roles = "Admin,Dueño")]
        [HttpPut("Actualizar")]
        public async Task<IActionResult> Actualizar(string id, DueñoActualizarDTO dueño)
        {
            try
            {
                var result = await _dueñoServicios.ActualizarUser(id, dueño);
                return Ok(result);

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, 
            Roles = "Admin,Dueño")]
        [HttpPut("Borrar")]
        public async Task<IActionResult> BorrarUser(string id)
        {
            try
            {
                var result = await _dueñoServicios.SoftDeleteUser(id);
                return Ok(result);

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, 
            Roles = "Admin")]
        [HttpDelete("Eliminar")]
        public async Task<IActionResult> EliminarUser(string id)
        {
            try
            {
                var result = await _dueñoServicios.HardDeleteUser(id);
                return Ok(result);

            }
            catch (Exception ex)
            {
                throw;
            }
        }


    }
}
