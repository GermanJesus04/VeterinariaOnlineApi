using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using VeterinariaOnlineApi.Core.DTOs.AuthDTOs;
using VeterinariaOnlineApi.Core.DTOs.DueñoDTOs;
using VeterinariaOnlineApi.Core.Models;
using VeterinariaOnlineApi.Infraestructura.Data;
using VeterinariaOnlineApi.Infraestructura.HelperDTOs;

namespace VeterinariaOnlineApi.Infraestructura.Servicios.AuthServices
{
    public class AuthServicios
    {
        private readonly UserManager<Dueño> _userManager;
        private readonly VeterinariaDbContext _dbContext;
        private readonly TokenValidationParameters _tokenParametros;
        private readonly IConfiguration _config;
        private string tiempoVencimiento => _config["tiempoVencimiento"] ?? 
                                throw new InvalidOperationException("Variable de entorno no encontrada");

        public AuthServicios(UserManager<Dueño> userManager,
            TokenValidationParameters tokenParametros,
            IConfiguration config,
            VeterinariaDbContext dbContext)
        {
            _userManager = userManager;
            _tokenParametros = tokenParametros;
            _config = config;
            _dbContext = dbContext;
        }

        public async Task<RespuestaWebApi<AuthResultDTO>> RegistrarDueño(DueñoRegistrarDTO dueñoDto)
        {
            try
            {
                //verificar si el correo ya existe
                var emailExiste = await _userManager.FindByEmailAsync(dueñoDto.Email);
                if (emailExiste != null)
                    throw new ExcepcionPeticionApi("Email ya existe, inicie sesion", 400);

                //crear el nuevo user
                var nuevoUser = new Dueño();
                nuevoUser.Email = dueñoDto.Email;
                nuevoUser.PhoneNumber = dueñoDto.PhoneNumber;
                nuevoUser.UserName = dueñoDto.UserName;

                var isCreated = await _userManager.CreateAsync(nuevoUser, dueñoDto.Password);

                if (!isCreated.Succeeded)
                    throw new ExcepcionPeticionApi(isCreated.Errors.FirstOrDefault()?.Description??
                        "Error en la creacion de usuario", 400);

                //agregar rol al user
                await _userManager.AddToRoleAsync(nuevoUser, "Dueño");

                //Generar token
                var roles = await _userManager.GetRolesAsync(nuevoUser);

                var result = await GenerarToken(nuevoUser, roles);
                
                //GenerarToken();

                return null;
            }catch (Exception ex)
            {
                throw;
            }
        }



        //Metodos Provados

        public async Task<AuthResultDTO> GenerarToken(Dueño user,  IList<string> roles)
        {
            try
            {

                var claimsList = new List<Claim>()
                {
                    new Claim("id", user.Id),
                    new Claim(JwtRegisteredClaimNames.Sub,user.Email),
                    new Claim(JwtRegisteredClaimNames.Email,user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTime.Now.ToUniversalTime().ToString())

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
                RefreshToken refresh = new()
                {
                    Id = Guid.NewGuid(),
                    JwtId = token.Id,
                    UserId = user.Id,
                    TicketToken = GeneradorTickers(23),
                    FechaAgredado = DateTime.UtcNow,
                    FechaVencimiento = DateTime.UtcNow.AddMonths(6),
                    EstaRevocado = false,
                    EstaUsado = false,
                };

                await _dbContext.RefreshTokens.AddAsync(refresh);
                await _dbContext.SaveChangesAsync();
                
                //enviar token con refreshtoken (ticket del token)
                return new AuthResultDTO
                {
                    Token = tokenJwt,
                    TicketToken = refresh.TicketToken,
                    Exitoso = true
                };


            }catch (Exception ex)
            {
                throw;
            }
        }


        public string GeneradorTickers(int tamaño)
        {
            Random random = new Random();
            string chars = "ABCDEFGHIJKLMNÑOPQRSTUVWXYZ1234567890abcdefghijklmnñopqrstuvwxyz_";

            return new string(Enumerable.Repeat(chars, tamaño).Select(s => s[random.Next(s.Length)]).ToArray());
        }

    }
}
