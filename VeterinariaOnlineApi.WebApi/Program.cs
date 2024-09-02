using Microsoft.EntityFrameworkCore;
using VeterinariaOnlineApi.Infraestructura.Data;

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

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
