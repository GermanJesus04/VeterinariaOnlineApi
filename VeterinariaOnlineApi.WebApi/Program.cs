using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using VeterinariaOnlineApi.Core.Models;
using VeterinariaOnlineApi.Infraestructura.Configuraciones;
using VeterinariaOnlineApi.Infraestructura.Data;
using VeterinariaOnlineApi.Infraestructura.HelperDTOs.JwtSettingDTOs;
using VeterinariaOnlineApi.Infraestructura.Servicios.AuthServices;
using VeterinariaOnlineApi.Infraestructura.Servicios.AuthServices.Interfaces;
using VeterinariaOnlineApi.Infraestructura.Servicios.CitaServices;
using VeterinariaOnlineApi.Infraestructura.Servicios.CitaServices.Interfaces;
using VeterinariaOnlineApi.Infraestructura.Servicios.DueñoServices;
using VeterinariaOnlineApi.Infraestructura.Servicios.DueñoServices.Interfaces;

var builder = WebApplication.CreateBuilder(args);


const string nombreConexion = "NombreConexion";
var ConfigConeccion = builder.Configuration.GetConnectionString(nombreConexion);

builder.Services.AddDbContext<VeterinariaDbContext>(opc =>
{
    opc.UseSqlServer(ConfigConeccion);
});


builder.Services.JwtConfigurar(builder.Configuration);

builder.Services.AddIdentity<Dueño, IdentityRole>(opc =>
{
    opc.Password.RequireDigit = true;
    opc.Password.RequiredLength = 8;
    opc.Password.RequireNonAlphanumeric = false;
    opc.Password.RequireUppercase = true;
    opc.Password.RequireLowercase = false;
    opc.SignIn.RequireConfirmedAccount = false;
    opc.SignIn.RequireConfirmedEmail = false;
    opc.Lockout.AllowedForNewUsers = false;
    opc.Lockout.MaxFailedAccessAttempts = 5; 
})
    .AddEntityFrameworkStores<VeterinariaDbContext>()
    .AddDefaultTokenProviders()
    .AddPasswordValidator<PasswordValidator<Dueño>>();



builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.Configure<AdminSettings>(builder.Configuration.GetSection("AdminSettings"));

//Inyecciones de dependencias
builder.Services.AddScoped<IAuthServicios,AuthServicios>();
builder.Services.AddScoped<IDueñoServicios,DueñoServicios>();
builder.Services.AddScoped<ICitaServicios,CitaServicios>();


builder.Services.AddSwaggerGen(
    opciones =>
    {
        opciones.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
        {
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            Scheme = "Bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header, //donde va a ir al bearer token
            Description = "JWT Autorizacion header"
        });

        opciones.AddSecurityRequirement(new OpenApiSecurityRequirement()
        {{
            new OpenApiSecurityScheme ()
            {
                Reference =  new OpenApiReference()
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string []{ }
        }});
    }
);

const string misReglasCors = "ReglasCors";
builder.Services.AddCors(opc =>
{
    opc.AddPolicy(name: misReglasCors, builder =>
    {
        builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(misReglasCors);

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    try
    {
        var _adminSetting = services.GetRequiredService<IOptions<AdminSettings>>().Value;
        var _userManager = services.GetRequiredService<UserManager<Dueño>>();
        var _rolesManager = services.GetRequiredService<RoleManager<IdentityRole>>();

        await DatosSemillaConfiguracion.Inicializar(services, _userManager, _rolesManager, _adminSetting);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while seeding the database.");
    }
    
}

app.Run();
