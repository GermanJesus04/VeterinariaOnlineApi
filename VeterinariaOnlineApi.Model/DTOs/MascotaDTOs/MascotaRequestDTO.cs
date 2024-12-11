using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VeterinariaOnlineApi.Core.DTOs.MascotaDTOs
{
    public class MascotaRequestDTO
    {
        public string Nombre { get; set; }
        public string Especie { get; set; }
        public int Edad { get; set; }
        public string DueñoId { get; set; }
    }
}
