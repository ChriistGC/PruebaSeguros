//using microsoft.aspnetcore.mvc;
//using microsoft.entityframeworkcore;

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
    public class ConsultaSeguro
    {
        public class obtenerSeguro : IRequest<SeguroDetalladoDTO>
        {
            public string Codigo { get; set; }
        }
        public class Manejador : IRequestHandler<obtenerSeguro, SeguroDetalladoDTO>
        {
            private readonly ApplicationDbContext _context;
            private readonly IMapper _mapper;
            public Manejador(ApplicationDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }
            public async Task<SeguroDetalladoDTO> Handle(obtenerSeguro request, CancellationToken cancellationToken)
            {
                try
                {
                    var seguro = await _context.Seguro
                        .Include(x => x.clienteLink)
                        .ThenInclude(y => y.cliente)
                        .FirstOrDefaultAsync(a => a.codigo == request.Codigo);

                    if (seguro == null)
                    {
                        throw new ManejadorException(HttpStatusCode.NotFound, new { ClienteSeguro = "No se encontró el seguro" });
                    }

                    var seguroDTO = _mapper.Map<Seguro, SeguroDetalladoDTO>(seguro);

                    return seguroDTO;
                }
                catch (Exception ex) {
                    throw new ManejadorException(HttpStatusCode.InternalServerError, new { ClienteSeguro = "Error Interno" });
                }
            }
        }
    }
}
