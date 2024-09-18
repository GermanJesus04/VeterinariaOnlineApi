using System.ComponentModel.DataAnnotations.Schema;

namespace VeterinariaOnlineApi.Core.Models
{
    [Table(name:"TRATAMIENTO")]
    public class Tratamiento:EntidadBase<Guid>
    {
        public Tratamiento()
        {
            this.Id = Guid.NewGuid();
            this.FechaCreacion = new DateTime();
            this.UsuarioCreacion = "admin";
            this.FechaActualizacion = new DateTime();
            this.UsuarioActualizacion = string.Empty;
            this.UsuarioEliminacion = string.Empty;
            this.FechaEliminacion = new DateTime();
        }

        [Column("NOMBRE")]
        public string Nombre { get; set; }

        [Column("DESCRIPCION")]
        public string Descripcion { get; set; }

        [ForeignKey("MASCOTA_ID")]
        public Guid MascotaId { get; set; }


        public Mascota Mascota { get; set; }

    }
}
