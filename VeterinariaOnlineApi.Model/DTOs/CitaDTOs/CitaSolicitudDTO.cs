using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VeterinariaOnlineApi.Core.Models;

namespace VeterinariaOnlineApi.Core.DTOs.CitaDTOs
{
    public class CitaSolicitudDTO
    {
        public Guid MascotaId { get; set; }

        public DateTime FechaCita { get; set; }

        public string Descripcion { get; set; }

    }
}
