using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VeterinariaOnlineApi.Infraestructura.HelperDTOs.JwtDTOs;

namespace VeterinariaOnlineApi.Infraestructura.Configuraciones
{
    public static class JwtConfiguracion
    {
        public static void JwtConfigurar(this IServiceCollection services, IConfiguration _configuration)
        {
            var parametrosSetting = new JwtParametros();
            _configuration.Bind("JsonWebTokenKeys", parametrosSetting);

            var ParametrosValidacionToken = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = parametrosSetting.KeyFirmaEsValida,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(parametrosSetting.Key)),
                ValidateIssuer = parametrosSetting.EmisorEsValido,
                ValidIssuer = parametrosSetting.Emisor,
                ValidateAudience = parametrosSetting.AudienciaEsValida,
                ValidAudience = parametrosSetting.Audiencia,
                RequireExpirationTime = parametrosSetting.TiempoCaducidadEsValido,
                ValidateLifetime = parametrosSetting.TiempoVidaEsValido,
                ClockSkew = TimeSpan.Zero
            };

            services.AddSingleton(parametrosSetting);
            services.AddSingleton(ParametrosValidacionToken);

            services.AddAuthentication(opc =>
            {
                opc.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                opc.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opc.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(opc =>
                {
                    opc.SaveToken = true;
                    opc.TokenValidationParameters = ParametrosValidacionToken;
                });

        }
    }
}
