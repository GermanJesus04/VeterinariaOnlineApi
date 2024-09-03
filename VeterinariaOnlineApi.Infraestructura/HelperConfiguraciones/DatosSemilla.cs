using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VeterinariaOnlineApi.Core.Models;
using VeterinariaOnlineApi.Infraestructura.HelperDTOs.JwtDTOs;

namespace VeterinariaOnlineApi.Infraestructura.HelperConfiguraciones
{
    public class DatosSemilla
    {
        public static async Task Inicializar(
            UserManager<Dueño> _userManager,
            RoleManager<IdentityRole> _roleManager,
            AdminSettings admin
            )
        {
            string[] roles = { "User", "Admin" };
            foreach ( var rol in roles )
            {
                if( !await _roleManager.RoleExistsAsync(rol))
                {
                    await _roleManager.CreateAsync(new IdentityRole(rol));
                }
            }

            var emailAdmin = admin.Email;
            var claveAdmin = admin.Password;

            if(await _userManager.FindByEmailAsync(emailAdmin) == null)
            {
                var userAdmin = new Dueño
                {
                    UserName = emailAdmin,
                    Email = emailAdmin,
                    EmailConfirmed = true
                };

                var result = await _userManager.CreateAsync(userAdmin, claveAdmin);
                if (result.Succeeded)
                    await _userManager.AddToRoleAsync(userAdmin,"Admin");
            }

        }
    }
}
