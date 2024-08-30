using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace VeterinariaOnlineApi.Core.Models
{
    [Table(name:"DUEÑO")]
    public class Dueño: IdentityUser
    {
        public string Telefono { get; set; }

        [Column("FECHA_CREACION")]
        [DataType(DataType.DateTime)]
        public DateTime FechaCreacion { get; set; }

        [Column("FECHA_ACTUALIZACION")]
        [DataType(DataType.DateTime)]
        public DateTime? FechaActualizacion { get; set; }

        [Column("FECHA_ELIMINACION")]
        [DataType(DataType.DateTime)]
        public DateTime? FechaEliminacion { get; set; }

        [Column("REGISTRO_ELIMINADO")]
        public bool Eliminado { get; set; }


        public ICollection<Mascota> Mascotas { get; set; }
    }
}
