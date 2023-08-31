using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SegurosAPI.Data;
using SegurosAPI.Modelo;
using SegurosAPI.Servicio.DTOs;
using SegurosAPI.Servicio.ManejadorError;
using System.Net;

namespace SegurosAPI.Servicio.ClienteSeguroServicio
{
    public class ConsultaCliente
    {
        public class ObtenerCliente : IRequest<ClienteDetalladoDTO>
        {
            public string Cedula { get; set; }
        }
        public class Manejador : IRequestHandler<ObtenerCliente, ClienteDetalladoDTO>
        {
            private readonly ApplicationDbContext _context;
            private readonly IMapper _mapper;
            public Manejador(ApplicationDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }
            public async Task<ClienteDetalladoDTO> Handle(ObtenerCliente request, CancellationToken cancellationToken)
            {
                try
                {
                    var cliente = await _context.Cliente
                        .Include(x => x.segurosLink)
                        .ThenInclude(y => y.seguro)
                        .FirstOrDefaultAsync(a => a.cedula == request.Cedula);

                    if (cliente == null)
                    {
                        throw new ManejadorException(HttpStatusCode.NotFound, new { ClienteSeguro = "No se encontró el cliente" });
                    }

                    var clienteDTO = _mapper.Map<Cliente, ClienteDetalladoDTO>(cliente);

                    return clienteDTO;
                }
                catch (Exception ex)
                {
                    throw new ManejadorException(HttpStatusCode.InternalServerError, new { ClienteSeguro = "Error Interno" });
                }
            }
        }
    }
}
