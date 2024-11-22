using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VeterinariaOnlineApi.Core.DTOs.DueñoDTOs;
using VeterinariaOnlineApi.Core.Models;

namespace VeterinariaOnlineApi.Infraestructura.Mappers
{
    public class Mapeos : Profile
    {
        public Mapeos() 
        {
            CreateMap<Dueño, DueñoResponseDTO>().ReverseMap();
        }

    }
}
