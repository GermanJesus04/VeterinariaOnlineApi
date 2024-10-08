using System.ComponentModel.DataAnnotations.Schema;

namespace VeterinariaOnlineApi.Core.Models
{
    [Table(name: "MASCOTA")]
    public class Mascota : EntidadBase<Guid>
    {
        public Mascota()
        {
            this.Id = Guid.NewGuid();
            this.FechaCreacion = new DateTime();
            this.UsuarioCreacion = "admin";
            this.FechaActualizacion = new DateTime();
            this.UsuarioActualizacion = string.Empty;
            this.UsuarioEliminacion = string.Empty;
            this.FechaEliminacion = new DateTime();
        }

        [Column(name:"NOMBRE")]
        public string Nombre { get; set; }
        
        [Column(name: "ESPECIE")]
        public string Especie { get; set; }

        [Column(name: "EDAD")]
        public int Edad { get; set; }

        [ForeignKey("ID_DUEÑO")]
        public string DueñoId { get; set; }


        public Dueño Dueño { get; set; }
        public ICollection<Cita> Citas { get; set; }
        public ICollection<Tratamiento> Tratamientos { get; set; }
    }
}
