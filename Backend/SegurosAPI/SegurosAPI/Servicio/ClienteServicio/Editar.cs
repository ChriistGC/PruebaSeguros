using FluentValidation;
using MediatR;
using SegurosAPI.Data;
using SegurosAPI.Servicio.ManejadorError;
using System.Net;

namespace SegurosAPI.Servicio.ClienteServicio
{
    public class Editar
    {
        public class EditarCliente : IRequest<Unit>
        {
            public int? id { get; set; }
            public string? Cedula { get; set; }
            public string? Nombre { get; set; }
            public string? Telefono { get; set; }
            public int? Edad { get; set; }
        }

        public class EditarClienteValidator : AbstractValidator<EditarCliente>
        {
            public EditarClienteValidator()
            {
                RuleFor(r => r.Cedula)
                    .Length(10)
                    .Must(cedula => IsNumeric(cedula))
                    .WithMessage("Cédula solo admite número.");
                RuleFor(r => r.Telefono)
                    .Length(10)
                    .Must(telefono => IsNumeric(telefono))
                    .WithMessage("Telefono solo admite número.");
                RuleFor(r => r.Nombre)
                    .NotEmpty()
                    .Must(nombre => IsString(nombre))
                    .WithMessage("Nombre solo admite letras.");
                RuleFor(r => r.Edad).NotEmpty();
            }

            //Valida que solo reciba números
            private bool IsNumeric(string value)
            {
                return int.TryParse(value, out _);
            }

            //Valida que solo reciba letras
            private bool IsString(string input)
            {
                foreach (char c in input)
                {
                    if (!char.IsLetter(c))
                    {
                        return false;
                    }
                }
                return true;
            }
        }

        public class Manejador : IRequestHandler<EditarCliente, Unit>
        {
            private readonly ApplicationDbContext _context;
            public Manejador(ApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<Unit> Handle(EditarCliente request, CancellationToken cancellationToken)
            {

                var Validationresult = await new EditarClienteValidator().ValidateAsync(request);
                if (!Validationresult.IsValid)
                {
                    throw new ManejadorException(HttpStatusCode.Unauthorized, new { Cliente = Validationresult.Errors });
                }
                //Busca el cliente a editar
                var cliente = await _context.Cliente.FindAsync(request.id);
                if(cliente == null)
                {
                    throw new ManejadorException(HttpStatusCode.NotFound, new { Cliente = "El cliente no existe" });
                }
                cliente.cedula = request.Cedula ?? cliente.cedula;
                cliente.nombre = request.Nombre ?? cliente.nombre;
                cliente.telefono = request.Telefono ?? cliente.telefono;
                cliente.edad = request.Edad ?? cliente.edad;

                var resultado = await _context.SaveChangesAsync();

                if (resultado > 0)
                    return Unit.Value;

                throw new ManejadorException(HttpStatusCode.InternalServerError, new { Seguro = "No se actualizo el cliente" });
            }
        }
    }
}
