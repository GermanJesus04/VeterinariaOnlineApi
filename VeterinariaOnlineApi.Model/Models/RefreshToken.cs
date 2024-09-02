using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VeterinariaOnlineApi.Core.Models
{
    [Table("REFRESH_TOKEN")]
    public class RefreshToken
    {
        [Column("ID")]
        public Guid Id { get; set; }

        [Column("USER_ID")]
        public string UserId { get; set; }

        [Column("JWT_ID")]
        public string JwtId { get; set; }

        [Column("TICKET_TOKEN")]
        public string Ticket { get; set; }

        [Column("TOKEN_USADO")]
        public bool EstaUsado { get; set; }

        [Column("TOKEN_REVOCADO")]
        public bool EstaRevocado { get; set; }

        [Column("FECHA_AGREGADO")]
        public DateTime FechaAgredado { get; set; }

        [Column("FECHA_CADUCIDAD")]
        public DateTime FechaVencimiento { get; set; }
    }
}
