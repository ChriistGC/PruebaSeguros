using AutoMapper;
using SegurosAPI.Modelo;
using SegurosAPI.Servicio.DTOs;

namespace SegurosAPI.Servicio
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Cliente, ClienteDTO>();
            CreateMap<Cliente, ClienteDetalladoDTO>()
                .ForMember(x => x.seguros, y => y.MapFrom(z => z.segurosLink.Select(a => a.seguro).ToList()));
            CreateMap<Seguro, SeguroDTO>();
            CreateMap<Seguro, SeguroDetalladoDTO>()
                .ForMember(s => s.clientes, t => t.MapFrom(u => u.clienteLink.Select(v => v.cliente).ToList()));
            CreateMap<ClienteSeguro, ClienteSeguroDTO>();
        }

    }
}
