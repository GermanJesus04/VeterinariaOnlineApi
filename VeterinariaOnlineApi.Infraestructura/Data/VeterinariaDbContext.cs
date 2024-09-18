using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using VeterinariaOnlineApi.Core.Models;
using VeterinariaOnlineApi.Infraestructura.Configuraciones;

namespace VeterinariaOnlineApi.Infraestructura.Data
{
    public class VeterinariaDbContext: IdentityDbContext<Dueño, IdentityRole, string>
    {
        public VeterinariaDbContext(DbContextOptions<VeterinariaDbContext> options) : base(options) { }


        public DbSet<Dueño> Dueños { get; set; }
        public DbSet<Mascota> Mascotas { get; set; }
        public DbSet<Cita> Citas { get; set; }
        public DbSet<Tratamiento> Tratamientos {get; set;}
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            
            //entidad relacion
            builder.Entity<Mascota>().HasOne(e => e.Dueño).WithMany(e => e.Mascotas)
                .HasForeignKey(e => e.DueñoId).OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Cita>().HasOne(e => e.Mascota).WithMany(e => e.Citas)
                .HasForeignKey(e=>e.MascotaId).OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Tratamiento>().HasOne(e => e.Mascota).WithMany(e => e.Tratamientos)
                .HasForeignKey(e => e.MascotaId).OnDelete(DeleteBehavior.Cascade);


            //establecer Index
            builder.Entity<Mascota>().HasIndex(e => new { e.Nombre, e.Especie, e.Edad });


            //configuracion de roles
            builder.ApplyConfiguration(new RolesConfiguracion());
        }

    }
}
