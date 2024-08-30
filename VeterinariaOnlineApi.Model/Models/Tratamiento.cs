using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VeterinariaOnlineApi.Core.Models
{
    [Table(name:"TRATAMIENTO")]
    public class Tratamiento:EntidadBase<Guid>
    {

        public string Nombre { get; set; }
        public string Descripcion { get; set; }
    }
}
