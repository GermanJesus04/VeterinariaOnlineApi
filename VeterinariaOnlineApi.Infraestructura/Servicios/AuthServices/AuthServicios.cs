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

                //var result = await GenerarToken(nuevoUser, roles);
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

                //obtengo sus roles
                var roles = await _userManager.GetRolesAsync(emailExiste);

                //genero el token
                var result = await GenerarToken(emailExiste, roles);

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
                var result = await ValidarRefrescarToken(refreshtoken);

                return result == null
                    ? throw new ExcepcionPeticionApi("Token no valido", 400)
                    : new RespuestaWebApi<AuthResultDTO> { Data = result };
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //Metodos Provados

        private async Task<AuthResultDTO> GenerarToken(Dueño user,  IList<string> roles)
        {
            try
            {

                var claimsList = new List<Claim>()
                {
                    new("id", user.Id),
                    new(JwtRegisteredClaimNames.Sub,user.Email),
                    new(JwtRegisteredClaimNames.Email,user.Email),
                    new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new(JwtRegisteredClaimNames.Iat, DateTime.Now.ToUniversalTime().ToString())

                };

                claimsList.AddRange(roles.Select(rol => new Claim(ClaimTypes.Role, rol)));

                //crear el token en un objeto de seguridad con el descriptor
                var tokenDescripto = new SecurityTokenDescriptor()
                {
                    Subject = new ClaimsIdentity(claimsList),
                    Expires = DateTime.UtcNow.Add(TimeSpan.Parse(tiempoVencimiento)),
                    SigningCredentials = new SigningCredentials(_tokenParametros.IssuerSigningKey, 
                                                                SecurityAlgorithms.HmacSha256)
                };

                //guardar el token en una variable string
                JwtSecurityTokenHandler tokenHandler = new();
                var token = tokenHandler.CreateToken(tokenDescripto);
                string tokenJwt = tokenHandler.WriteToken(token);

                //crear nuevo objecto refreshtoken y guadar en bbdd
                //RefreshToken refresh = new()
                //{
                //    Id = Guid.NewGuid(),
                //    JwtId = token.Id,
                //    UserId = user.Id,
                //    TicketToken = GeneradorTickers(23),
                //    FechaAgredado = DateTime.UtcNow,
                //    FechaVencimiento = DateTime.UtcNow.AddMonths(6),
                //    EstaRevocado = false,
                //    EstaUsado = false,
                //};

                //await _dbContext.RefreshTokens.AddAsync(refresh);
                //await _dbContext.SaveChangesAsync();
                
                
                return new AuthResultDTO
                {
                    Token = tokenJwt,
                    TicketToken = "ticket prueba", //refresh.TicketToken,
                    Exitoso = true
                };


            }catch (Exception ex)
            {
                throw;
            }
        }

        private async Task<AuthResultDTO> ValidarRefrescarToken(RequestTokenCaducadoDTO tokenCaducado)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            try
            {
                //validamos los parametros del token
                var verificacionToken = tokenHandler.ValidateToken(tokenCaducado.TokenExpirado,
                                                     _tokenParametros, out var TokenValidado);

                //verificar si el token es del tipo JwtSecurityToken y
                // si el algoritmo no es igual a hmacSha256 devuelve null
                if(
                    TokenValidado is JwtSecurityToken jwtSecurityToken 
                    && !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                                                    StringComparison.InvariantCultureIgnoreCase)
                    )
                {
                    return null;
                }


                var expiraClaim = verificacionToken.Claims.FirstOrDefault(x =>
                                            x.Type == JwtRegisteredClaimNames.Exp)?.Value;

                //Verificar si el token ha expirado

                /// Se verifica si el token venció o aún está vigente a la fecha actual
                if (long.TryParse(expiraClaim, out var exp) &&
                    DateTimeOffset.FromUnixTimeSeconds(exp) > DateTimeOffset.UtcNow)
                {
                    throw new ExcepcionPeticionApi("Token no válido", 400);
                }


                //validar token entrante con el de bbdd
                var tokenAlmacenado = await _dbContext.RefreshTokens.FirstOrDefaultAsync(x => 
                                                        x.TicketToken == tokenCaducado.TicketRefreshToken);

                if (tokenAlmacenado == null || tokenAlmacenado.EstaUsado || tokenAlmacenado.EstaRevocado)
                    throw new ExcepcionPeticionApi("token no valido", 400);

                //validar jti (id del token) sea el mismo y fecha de vencimiento vencida

                var jtiTokenCaducado = verificacionToken.Claims.FirstOrDefault(x=>
                                                        x.Type == JwtRegisteredClaimNames.Jti)?.Value;

                if (jtiTokenCaducado != tokenAlmacenado.JwtId || tokenAlmacenado.FechaVencimiento > DateTime.UtcNow)
                    throw new ExcepcionPeticionApi("token no valido", 400);

                //actualizar refreshtoken usado

                tokenAlmacenado.EstaUsado = true;
                _dbContext.RefreshTokens.Update(tokenAlmacenado);
                await _dbContext.SaveChangesAsync();

                //verificamos que el usuario sea valido
                var user = await _userManager.FindByIdAsync(tokenAlmacenado.UserId);
                if (user == null)
                    throw new ExcepcionPeticionApi("token no valido", 400);

                var roles = await _userManager.GetRolesAsync(user);

                return await GenerarToken(user, roles);

            }
            catch (Exception ex)
            {
                throw new ExcepcionPeticionApi("Error al verificar y generar token: " + ex.Message, 500);
            }
        }

        private static string GeneradorTickers(int tamaño)
        {
            Random random = new Random();
            string chars = "ABCDEFGHIJKLMNÑOPQRSTUVWXYZ1234567890abcdefghijklmnñopqrstuvwxyz_";

            return new string(Enumerable.Repeat(chars, tamaño).Select(s => s[random.Next(s.Length)]).ToArray());
        }

    }
}
