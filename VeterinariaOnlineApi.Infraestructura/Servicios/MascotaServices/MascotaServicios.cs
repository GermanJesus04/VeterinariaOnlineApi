using AutoMapper;
using Azure.Core;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VeterinariaOnlineApi.Core.DTOs.MascotaDTOs;
using VeterinariaOnlineApi.Infraestructura.Data;
using VeterinariaOnlineApi.Infraestructura.HelperDTOs;

namespace VeterinariaOnlineApi.Infraestructura.Servicios.MascotaServices
{
    public class MascotaServicios
    {
        private readonly VeterinariaDbContext _context;
        private readonly IMapper _mapper;
        public MascotaServicios(VeterinariaDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<RespuestaWebApi<object>> RegistrarMascota(MascotaRequestDTO mascotaDto)
        {
            return null;
        }

    }
}
