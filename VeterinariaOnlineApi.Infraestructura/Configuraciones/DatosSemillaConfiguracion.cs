using Microsoft.AspNetCore.Identity;
using VeterinariaOnlineApi.Core.Models;
using VeterinariaOnlineApi.Infraestructura.HelperDTOs.JwtSettingDTOs;

namespace VeterinariaOnlineApi.Infraestructura.Configuraciones
{
    public class DatosSemillaConfiguracion
    {
        public static async Task Inicializar(
             IServiceProvider serviceProvider,
            UserManager<Dueño> _userManager,
            RoleManager<IdentityRole> _roleManager,
            AdminSettings admin
            )
        {
            string[] roles = ["Dueño", "Admin"];
            foreach (var rol in roles)
            {
                if (!await _roleManager.RoleExistsAsync(rol))
                {
                    await _roleManager.CreateAsync(new IdentityRole(rol));
                }
            }

            var emailAdmin = admin.Email;
            var claveAdmin = admin.Password;

            if (await _userManager.FindByEmailAsync(emailAdmin) == null)
            {
                var userAdmin = new Dueño
                {
                    UserName = emailAdmin,
                    Email = emailAdmin,
                    EmailConfirmed = true
                };

                var result = await _userManager.CreateAsync(userAdmin, claveAdmin);
                if (result.Succeeded)
                    await _userManager.AddToRoleAsync(userAdmin, "Admin");
            }

        }
    }
}
