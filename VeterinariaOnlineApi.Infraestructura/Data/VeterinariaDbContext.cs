using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using VeterinariaOnlineApi.Core.Models;

namespace VeterinariaOnlineApi.Infraestructura.Data
{
    public class VeterinariaDbContext : IdentityDbContext<Dueño>
    {
        public VeterinariaDbContext()
        {
            
        }
    }
}
