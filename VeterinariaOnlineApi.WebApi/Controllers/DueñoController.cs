using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VeterinariaOnlineApi.Core.DTOs.DueñoDTOs;
using VeterinariaOnlineApi.Infraestructura.Servicios.DueñoServices.Interfaces;

namespace VeterinariaOnlineApi.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DueñoController : ControllerBase
    {
        private readonly IDueñoServicios _dueñoServicios;
        public DueñoController(IDueñoServicios dueñoServicios)
        {
            _dueñoServicios = dueñoServicios;
        }

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
