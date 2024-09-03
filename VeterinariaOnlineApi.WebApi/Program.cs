using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using VeterinariaOnlineApi.Core.Models;
using VeterinariaOnlineApi.Infraestructura.Configuraciones;
using VeterinariaOnlineApi.Infraestructura.Data;
using VeterinariaOnlineApi.Infraestructura.HelperConfiguraciones;
using VeterinariaOnlineApi.Infraestructura.HelperDTOs.JwtDTOs;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


const string NombreConexion = "NombreConexion";
var ConfigConeccion = builder.Configuration.GetConnectionString(NombreConexion);
builder.Services.AddDbContext<VeterinariaDbContext>(opc => opc.UseSqlServer(
    ConfigConeccion ?? throw new InvalidOperationException("Cadena de Conexion no encontrada")
    )
);


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



var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();
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

        await DatosSemilla.Inicializar(_userManager, _rolesManager, _adminSetting);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while seeding the database.");
    }

    
}

app.Run();
