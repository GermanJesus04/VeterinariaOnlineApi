using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using VeterinariaOnlineApi.Core.DTOs.AuthDTOs;
using VeterinariaOnlineApi.Core.DTOs.DueñoDTOs;
using VeterinariaOnlineApi.Core.Models;
using VeterinariaOnlineApi.Infraestructura.Data;
using VeterinariaOnlineApi.Infraestructura.HelperDTOs;
using VeterinariaOnlineApi.Infraestructura.HelperDTOs.GestionToken;
using VeterinariaOnlineApi.Infraestructura.HelperDTOs.JwtSettingDTOs;
using VeterinariaOnlineApi.Infraestructura.Servicios.AuthServices.Interfaces;

namespace VeterinariaOnlineApi.Infraestructura.Servicios.AuthServices
{
    public class AuthServicios : IAuthServicios
    {
        private readonly UserManager<Dueño> _userManager;
        private readonly VeterinariaDbContext _dbContext;
        private readonly TokenValidationParameters _tokenParametros;
        private readonly JwtParametros _jwtParametros;
        private readonly IConfiguration _config;
        private string tiempoVencimiento => _config["tiempoVencimiento"] ?? 
                                throw new InvalidOperationException("Variable de entorno no encontrada");

        public AuthServicios(UserManager<Dueño> userManager,
            TokenValidationParameters tokenParametros,
            IConfiguration config,
            VeterinariaDbContext dbContext,
            JwtParametros jwtParametros)
        {
            _userManager = userManager;
            _tokenParametros = tokenParametros;
            _config = config;
            _dbContext = dbContext;
            _jwtParametros = jwtParametros;
            TokenManager.Inicializar(jwtParametros, dbContext, userManager);
        }

        public async Task<RespuestaWebApi<AuthResultDTO>> RegistrarDueño(DueñoRegistrarDTO dueñoDto)
        {
            try
            {
                var emailExiste = await _userManager.FindByEmailAsync(dueñoDto.Email);
                if (emailExiste != null)
                    throw new ExcepcionPeticionApi("Email ya existe, inicie sesion", 400);

                var nuevoUser = new Dueño
                {
                    Email = dueñoDto.Email,
                    PhoneNumber = dueñoDto.PhoneNumber,
                    UserName = dueñoDto.UserName
                };

                var isCreated = await _userManager.CreateAsync(nuevoUser, dueñoDto.Password);

                if (!isCreated.Succeeded)
                    throw new ExcepcionPeticionApi(isCreated.Errors.FirstOrDefault()?.Description ??
                        "Error en la creacion de usuario", 400);

                await _userManager.AddToRoleAsync(nuevoUser, "Dueño");

                var roles = await _userManager.GetRolesAsync(nuevoUser);

                var result = await TokenManager.GenerarToken(nuevoUser, roles);

                return new RespuestaWebApi<AuthResultDTO>
                {
                    Data=result,
                    Mensaje = "Usuario Registrado con exito"
                };

            }catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<RespuestaWebApi<AuthResultDTO>> LoginDueño(DueñoLoginDTO loginDTO)
        {
            try
            {
                //validar email
                var emailExiste = await _userManager.FindByEmailAsync(loginDTO.Email);

                if (emailExiste == null)
                    throw new ExcepcionPeticionApi("Credenciales no validas", 400);

                //verificar convinacion
                var credencialValida = await _userManager.CheckPasswordAsync(emailExiste, loginDTO.Password);

                if (credencialValida is false)
                    throw new ExcepcionPeticionApi("Credenciales no validas", 400);
                
                var refreshToken = await _dbContext.RefreshTokens.FirstOrDefaultAsync(x 
                                   => x.UserId == emailExiste.Id && x.EstaUsado == false && x.EstaRevocado == false);
                
                if(refreshToken != null)
                {
                    _dbContext.RefreshTokens.Remove(refreshToken);
                    await _dbContext.SaveChangesAsync();
                }
                
                //obtengo sus roles
                var roles = await _userManager.GetRolesAsync(emailExiste);

                //genero el token
                var result = await TokenManager.GenerarToken(emailExiste, roles);

                return new RespuestaWebApi<AuthResultDTO> {
                    Data = result
                };

            }catch (Exception)
            {
                throw;
            }
        }

        public async Task<RespuestaWebApi<AuthResultDTO>> RefreshToken(RequestTokenCaducadoDTO refreshtoken)
        {
            try
            {
                var result = await TokenManager.ValidarRefrescarToken(refreshtoken, _tokenParametros);

                return result == null
                    ? throw new ExcepcionPeticionApi("Token no valido", 400)
                    : new RespuestaWebApi<AuthResultDTO> { Data = result };
            }
            catch (Exception ex)
            {
                throw;

            }
        }

       
        
    }
}
