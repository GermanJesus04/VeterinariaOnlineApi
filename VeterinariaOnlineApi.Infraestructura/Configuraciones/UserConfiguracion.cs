using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VeterinariaOnlineApi.Core.Configuraciones
{
    public class UserConfiguracion : IEntityTypeConfiguration<IdentityUser>
    {
        public void Configure(EntityTypeBuilder<IdentityUser> builder)
        {
            builder.ToTable("USUARIO");
            builder.HasKey(c => c.Id);
            builder.Property(c => c.UserName).HasMaxLength(20).HasColumnType("VARCHAR").HasColumnName("NOMBRE_USER");
            builder.Property(c => c.PhoneNumber).HasColumnType("int").HasColumnName("NUMERO_CELULAR");
           
        }
    }
    {

    }
}
