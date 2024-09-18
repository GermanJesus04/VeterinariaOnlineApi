using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace VeterinariaOnlineApi.Infraestructura.Configuraciones
{
    public class RolesConfiguracion : IEntityTypeConfiguration<IdentityRole>
    {
        public void Configure(EntityTypeBuilder<IdentityRole> builder)
        {
            builder.HasData(
                
                new IdentityRole
                {
                    Id= Guid.NewGuid().ToString(),
                    Name = "Dueño",
                    NormalizedName = "DUEÑO"
                },
                new IdentityRole
                {
                    Id= Guid.NewGuid().ToString(),
                    Name = "Admin",
                    NormalizedName = "ADMIN"
                }
                
                );
        }
    }
}
