using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SegurosAPI.Data;
using SegurosAPI.Modelo;
using SegurosAPI.Servicio.DTOs;

namespace SegurosAPI.Servicio.ClienteServicio
{
    public class Consulta
    {
        public class ListaCliente : IRequest<List<ClienteDTO>> { }

        public class Manejador : IRequestHandler<ListaCliente, List<ClienteDTO>>
        {
            private readonly ApplicationDbContext _context;
            private readonly IMapper _mapper;
            public Manejador(ApplicationDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }
            public async Task<List<ClienteDTO>> Handle(ListaCliente request, CancellationToken cancellationToken)
            {
                    var clientes = await _context.Cliente.ToListAsync();

                    //Mapeo a DTO
                    var clienteDTO = _mapper.Map<List<Cliente>, List<ClienteDTO>>(clientes);
                    return clienteDTO;
            }
        }
    }
}
