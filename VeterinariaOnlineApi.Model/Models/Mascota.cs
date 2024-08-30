using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VeterinariaOnlineApi.Core.Models
{
    [Table(name: "MASCOTA")]
    public class Mascota : EntidadBase<Guid>
    {
        [Column(name:"NOMBRE")]
        public string Nombre { get; set; }
        
        [Column(name: "ESPECIE")]
        public string Especie { get; set; }

        [Column(name: "EDAD")]
        public int Edad { get; set; }

        [Column(name: "DUEÑO_ID")]
        public Guid DueñoId { get; set; }


        public Dueño Dueño { get; set; }
        public ICollection<Cita> Citas { get; set; }
    }
}
