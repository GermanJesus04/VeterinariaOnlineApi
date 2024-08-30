using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
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
