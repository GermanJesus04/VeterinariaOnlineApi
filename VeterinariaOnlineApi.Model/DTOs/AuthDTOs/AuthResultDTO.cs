using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VeterinariaOnlineApi.Core.DTOs.AuthDTOs
{
    public class AuthResultDTO
    {
        public string Token { get; set; }
        public string TicketToken { get; set; }
        public bool Exitoso { get; set; }
        public List<string> Errores { get; set; }
    }
}
