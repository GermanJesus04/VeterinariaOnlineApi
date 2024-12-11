using AutoMapper;
using VeterinariaOnlineApi.Core.DTOs.CitaDTOs;
using VeterinariaOnlineApi.Core.DTOs.DueñoDTOs;
using VeterinariaOnlineApi.Core.DTOs.MascotaDTOs;
using VeterinariaOnlineApi.Core.Models;

namespace VeterinariaOnlineApi.Infraestructura.Mappers
{
    public class Mapeos : Profile
    {
        public Mapeos() 
        {
            CreateMap<Dueño, DueñoResponseDTO>().ReverseMap();
            CreateMap<Cita, CitaSolicitudDTO>().ReverseMap();
            CreateMap<Mascota, MascotaRequestDTO>().ReverseMap();
        }

    }
}
