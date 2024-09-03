using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using VeterinariaOnlineApi.Core.Models;
using VeterinariaOnlineApi.Infraestructura.Configuraciones;

namespace VeterinariaOnlineApi.Infraestructura.Data
{
    public class VeterinariaDbContext : IdentityDbContext<Dueño, IdentityRole, string>
    {
        public DbSet<Dueño> Dueños { get; set; }
        public DbSet<Mascota> Mascotas { get; set; }
        public DbSet<Cita> Citas { get; set; }
        public DbSet<Tratamiento> Tratamientos {get; set;}

        public VeterinariaDbContext(DbContextOptions<VeterinariaDbContext> opctions): base(opctions)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);


            builder.ApplyConfiguration(new RolesConfiguracion());
        }

    }
}
