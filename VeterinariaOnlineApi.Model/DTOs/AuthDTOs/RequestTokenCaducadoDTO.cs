using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VeterinariaOnlineApi.Core.DTOs.AuthDTOs
{
    public class RequestTokenCaducadoDTO
    {
        public string TokenExpirado {  get; set; }
        public string TicketRefreshToken { get; set; }
    }
}
