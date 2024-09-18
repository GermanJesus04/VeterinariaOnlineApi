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
        public Cita()
        {
            this.Id = Guid.NewGuid();
            this.FechaCreacion = new DateTime();
            this.UsuarioCreacion = "admin";
            this.FechaActualizacion = new DateTime();
            this.UsuarioActualizacion = string.Empty;
            this.UsuarioEliminacion = string.Empty;
            this.FechaEliminacion = new DateTime();
        }

        [ForeignKey("MASCOTA_ID")]
        public Guid MascotaId { get; set; }

        [Column("FECHA")]
        public DateTime Fecha { get; set; }

        [Column("DESCRIPCION")]
        public string Descripcion { get; set; }


        public Mascota Mascota { get; set; }
    }
}
