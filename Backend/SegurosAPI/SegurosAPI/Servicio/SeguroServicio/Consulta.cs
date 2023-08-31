using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SegurosAPI.Data;
using SegurosAPI.Modelo;
using SegurosAPI.Servicio.DTOs;

namespace SegurosAPI.Servicio.SeguroServicio
{
    public class Consulta
    {
        public class ListaSeguro : IRequest<List<SeguroDTO>> { }

        public class Manejador : IRequestHandler<ListaSeguro, List<SeguroDTO>>
        {
            private readonly ApplicationDbContext _context;
            private readonly IMapper _mapper;
            public Manejador(ApplicationDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }
            public async Task<List<SeguroDTO>> Handle(ListaSeguro request, CancellationToken cancellationToken)
            {
                var seguros = await _context.Seguro.ToListAsync();

                //Mapeo a DTO
                var seguroDTO = _mapper.Map<List<Seguro>, List<SeguroDTO>>(seguros);
                return seguroDTO;
            }
        }
    }
}
