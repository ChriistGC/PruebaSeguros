using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SegurosAPI.Data;
using SegurosAPI.Modelo;
using SegurosAPI.Servicio.DTOs;
using SegurosAPI.Servicio.ManejadorError;
using System.Net;
using static SegurosAPI.Servicio.ClienteServicio.Editar;

namespace SegurosAPI.Servicio.ClienteSeguroServicio
{
    public class ConsultaCliente
    {
        public class ObtenerCliente : IRequest<ClienteDetalladoDTO>
        {
            public string Cedula { get; set; }
        }

        public class ObtenerClienteValidator : AbstractValidator<ObtenerCliente>
        {
            public ObtenerClienteValidator()
            {
                RuleFor(r => r.Cedula).Length(10)
                    .Must(cedula => IsNumeric(cedula))
                    .WithMessage("Debe ser numero.")
                    .When(r => !string.IsNullOrEmpty(r.Cedula));
            }

            private bool IsNumeric(string value)
            {
                return int.TryParse(value, out _);
            }
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
                var Validationresult = await new ObtenerClienteValidator().ValidateAsync(request);
                if (!Validationresult.IsValid)
                {
                    throw new ManejadorException(HttpStatusCode.Unauthorized, new { Cliente = Validationresult.Errors });
                }


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
        }
    }
}
