using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VeterinariaOnlineApi.Infraestructura.HelperDTOs.JwtDTOs
{
    internal class AuthResult
    {
        public string Token { get; set; }
        public string ResfreshToken { get; set; }
        public bool Exitoso { get; set; }
        public List<string> Errores { get; set; }
    }
}
