using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using VeterinariaOnlineApi.Core.DTOs.AuthDTOs;
using VeterinariaOnlineApi.Core.Models;
using VeterinariaOnlineApi.Infraestructura.Data;
using VeterinariaOnlineApi.Infraestructura.HelperDTOs.JwtSettingDTOs;

namespace VeterinariaOnlineApi.Infraestructura.HelperDTOs.GestionToken
{
    public static class TokenManager 
    { 

        private static JwtParametros _tokenParametros;
        private static VeterinariaDbContext _dbContext;
        private static UserManager<Dueño> _userManager;

        public static void Inicializar(JwtParametros tokenParametros,
            VeterinariaDbContext context, 
            UserManager<Dueño> userManager)
        {
            _tokenParametros = tokenParametros;
            _dbContext = context;
            _userManager = userManager;
        }

        public static async Task<AuthResultDTO> GenerarToken(Dueño user, IList<string> roles)
        {
            try
            {
                var claimsList = new List<Claim>
                {
                    new("id", user.Id),
                    new(JwtRegisteredClaimNames.Sub, user.Email),
                    new(JwtRegisteredClaimNames.Email, user.Email),
                    new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new(JwtRegisteredClaimNames.Iat,DateTime.UtcNow.ToUniversalTime().ToString())
                };

                claimsList.AddRange(roles.Select(rol =>  new Claim(ClaimTypes.Role,rol)));

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(claimsList),
                    Expires = DateTime.UtcNow.Add(_tokenParametros.tiempoVencimiento),
                    SigningCredentials = new SigningCredentials(
                        new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_tokenParametros.Key)), SecurityAlgorithms.HmacSha256)
                };
                
                var tokenhandler = new JwtSecurityTokenHandler();
                var tokenCreado = tokenhandler.CreateToken(tokenDescriptor);
                string token = tokenhandler.WriteToken(tokenCreado);
                RefreshToken refresh = new()
                {
                    Id = Guid.NewGuid(),
                    JwtId = tokenCreado.Id,
                    UserId = user.Id,
                    TicketToken = GeneradorCadenasAleatorias(23),
                    FechaAgredado = DateTime.UtcNow,
                    FechaVencimiento = DateTime.UtcNow.AddMonths(6),
                    EstaRevocado = false,
                    EstaUsado = false,
                };

                await _dbContext.RefreshTokens.AddAsync(refresh);
                await _dbContext.SaveChangesAsync();

                return new AuthResultDTO
                {
                    Token = token,
                    TicketToken = refresh.TicketToken,
                    Exitoso = true
                };

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public static async Task<AuthResultDTO> ValidarRefrescarToken(RequestTokenCaducadoDTO tokenCaducado,
                                                                 TokenValidationParameters jwtParametros)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
               
                jwtParametros.ClockSkew = TimeSpan.Zero;
                jwtParametros.ValidateLifetime = false;

                var verificacionToken = tokenHandler.ValidateToken(tokenCaducado.TokenExpirado, jwtParametros, out var tokenValidado);

                if (tokenValidado is JwtSecurityToken jwtSecurityToken && !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                {
                    return null;
                }

                var expiraClaim = verificacionToken.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Exp)?.Value;

                if (long.TryParse(expiraClaim, out var exp) && DateTimeOffset.FromUnixTimeSeconds(exp) > DateTimeOffset.UtcNow)
                {
                    throw new ExcepcionPeticionApi("Token no válido", 400);
                }

                var tokenAlmacenado = await _dbContext.RefreshTokens.FirstOrDefaultAsync(x => x.TicketToken == tokenCaducado.TicketRefreshToken);
                if (tokenAlmacenado == null || tokenAlmacenado.EstaUsado || tokenAlmacenado.EstaRevocado)
                    throw new ExcepcionPeticionApi("token no valido", 400);

                var jtiTokenCaducado = verificacionToken.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti)?.Value;
                if (jtiTokenCaducado != tokenAlmacenado.JwtId || tokenAlmacenado.FechaVencimiento > DateTime.UtcNow)
                    throw new ExcepcionPeticionApi("token no valido", 400);

                tokenAlmacenado.EstaUsado = true;
                _dbContext.RefreshTokens.Update(tokenAlmacenado);
                await _dbContext.SaveChangesAsync();

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

        //Metodos Privados
        private static string GeneradorCadenasAleatorias(int tamaño)
        {
            var random = new Random();
            var chars = "ABCDEFGHIJKLMNÑOPQRSTUVWXYZ1234567890abcdefghijklmnñopqrstuvwxyz_";

            //devuelve una cadena aleatoria 
            return new string(Enumerable.Repeat(chars, tamaño)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

    }
}
