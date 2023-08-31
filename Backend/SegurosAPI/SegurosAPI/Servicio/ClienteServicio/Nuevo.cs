using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SegurosAPI.Data;
using SegurosAPI.Modelo;
using SegurosAPI.Servicio.ManejadorError;
using System.Net;

namespace SegurosAPI.Servicio.ClienteServicio
{
    public class Nuevo
    {
        public class CrearCliente : IRequest<Unit>
        {
            public string Cedula { get; set; }
            public string Nombre { get; set; }
            public string Telefono { get; set; }
            public int Edad { get; set; }
        }

        public class CrearClienteValidator : AbstractValidator<CrearCliente>
        {
            public CrearClienteValidator() {
                RuleFor(r => r.Nombre).NotEmpty();
                RuleFor(r => r.Cedula).NotEmpty()
                    .Length(10)
                    .Must(cedula => IsNumeric(cedula))
                    .WithMessage("Debe ser numero.");
                RuleFor(r=>r.Telefono).NotEmpty()
                    .Length(10)
                    .Must(telefono => IsNumeric(telefono))
                    .WithMessage("Debe ser numero.");
                RuleFor(r => r.Edad).NotEmpty();
            }

            private bool IsNumeric(string value)
            {
                return int.TryParse(value, out _);
            }
        }

        public class Manejador : IRequestHandler<CrearCliente, Unit>
        {
            private readonly ApplicationDbContext _context;
            public Manejador(ApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<Unit> Handle(CrearCliente request, CancellationToken cancellationToken)
            {
                //Valida los campos
                var Validationresult = await new CrearClienteValidator().ValidateAsync(request);
                if (!Validationresult.IsValid)
                {
                    throw new ManejadorException(HttpStatusCode.Unauthorized, new { Cliente = Validationresult.Errors });
                }

                //Comprueba la existencia de Cliente
                var clienteEx = await _context.Cliente.AnyAsync(u => u.cedula.Equals(request.Cedula));
                if (!clienteEx)
                {
                    var cliente = new Cliente
                    {
                        cedula = request.Cedula,
                        nombre = request.Nombre,
                        telefono = request.Telefono,
                        edad = request.Edad
                    };

                    _context.Cliente.Add(cliente);
                    var valor = await _context.SaveChangesAsync();
                    if (valor > 0)
                    {
                        return Unit.Value;
                    }
                    throw new ManejadorException(HttpStatusCode.InternalServerError, new { Cliente = "No se pudo almacenar el Cliente" });
                }

                throw new ManejadorException(HttpStatusCode.BadRequest, new { Cliente = "Cliente existente en la BD" });
            }
        }
    }
}
