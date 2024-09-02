using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VeterinariaOnlineApi.Core.Models
{
    [Table(name:"CITA")]
    public class Cita:EntidadBase<Guid>
    {
        [ForeignKey("MASCOTA_ID")]
        public Guid MascotaId { get; set; }

        [Column("FECHA")]
        public DateTime Fecha { get; set; }

        [Column("DESCRIPCION")]
        public string Descripcion { get; set; }


        public Mascota Mascota { get; set; }
    }
}
