using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using VeterinariaOnlineApi.Core.DTOs.DueñoDTOs;
using VeterinariaOnlineApi.Core.Models;
using VeterinariaOnlineApi.Infraestructura.Data;
using VeterinariaOnlineApi.Infraestructura.HelperDTOs;
using VeterinariaOnlineApi.Infraestructura.Servicios.DueñoServices.Interfaces;

namespace VeterinariaOnlineApi.Infraestructura.Servicios.DueñoServices
{
    public class DueñoServicios : IDueñoServicios
    {
        private readonly VeterinariaDbContext _context;
        private readonly UserManager<Dueño> _userManager;
        private readonly IMapper _mapeo;
        public DueñoServicios(VeterinariaDbContext context, IMapper mapeo, UserManager<Dueño> userManager)
        {
            _context = context;
            _mapeo = mapeo;
            _userManager = userManager;
        }

        //dueño
        public async Task<RespuestaWebApi<DueñoResponseDTO>> ObtenerUser(string id)
        {
            try
            {
                var data = await _context.Dueños.Include(x=>x.Mascotas)
                    .FirstOrDefaultAsync(x=>x.Id == id);

                if (data == null)
                    throw new ExcepcionPeticionApi("Usuario no encontrado", 400);

                var Result = _mapeo.Map<DueñoResponseDTO>(data);

                return new RespuestaWebApi<DueñoResponseDTO>
                {
                    Data = Result
                };

            }catch (Exception ex)
            {
                throw;
            }
        }

        //Admin:
        public async Task<RespuestaWebApi<List<DueñoResponseDTO>>> ListarUser(string? id)
        {
            try
            {
                var users = _context.Dueños.AsQueryable();

                if (!string.IsNullOrEmpty(id))
                {
                    users =  users.Where(x => x.Id == id);
                }

                var result = await users.ProjectTo<DueñoResponseDTO>(
                    _mapeo.ConfigurationProvider).ToListAsync();

                if (result == null)
                    throw new ExcepcionPeticionApi("No se encontraron resultados", 204);

                return new RespuestaWebApi<List<DueñoResponseDTO>>
                {
                    Data = result
                };

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //admmin y dueño
        public async Task<RespuestaWebApi<bool>> ActualizarUser(string id, DueñoActualizarDTO dueño)
        {
            try
            {
                //var data = await 
                var user = await _userManager.FindByIdAsync(id);
                if (user is null)
                    throw new ExcepcionPeticionApi("Usuario no encontrado", 400);

                user.PhoneNumber = string.IsNullOrEmpty(dueño.numeroCelular) ? user.PhoneNumber : dueño.numeroCelular;
                user.UserName = string.IsNullOrEmpty(dueño.Nombre)? user.UserName : dueño.Nombre;

                var result = await _userManager.UpdateAsync(user);
                if (!result.Succeeded)
                    throw new ExcepcionPeticionApi("Error al Actualizar Informacion", 500);

                return new RespuestaWebApi<bool>
                {
                    Data = result.Succeeded
                };

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //admin y dueño(limitado)
        public async Task<RespuestaWebApi<string>> SoftDeleteUser(string id)
        {
            try
            {
                var user = await _context.Dueños.FirstOrDefaultAsync(x=>x.Id==id && x.Eliminado == true);
                if (user == null)
                    throw new ExcepcionPeticionApi("Usuario no encontrado", 400);

                user.Eliminado = true;
                var result =  _context.Dueños.Update(user);

                return  new RespuestaWebApi<string>
                {
                    Mensaje = "Eliminado con Exito",
                    Data = null
                };

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<RespuestaWebApi<string>> HardDeleteUser(string id)
        {
            try
            {
                var user = await _context.Dueños.FirstOrDefaultAsync(x => x.Id == id);
                if (user == null)
                    throw new ExcepcionPeticionApi("Usuario no encontrado", 400);

                
                var result = _context.Dueños.Remove(user);

                return new RespuestaWebApi<string>
                {
                    Mensaje = "Eliminado con Exito",
                    Data = null
                };

            }
            catch (Exception ex)
            {
                throw;
            }
        }


    }
}
