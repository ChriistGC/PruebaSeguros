using FluentValidation;
using MediatR;
using SegurosAPI.Data;
using SegurosAPI.Modelo;
using SegurosAPI.Servicio.ManejadorError;
using System.Net;
using static SegurosAPI.Servicio.ClienteServicio.Editar;

namespace SegurosAPI.Servicio.SeguroServicio
{
    public class Editar
    {
        public class EditarSeguro : IRequest<Unit>
        {
            public int id { get; set; }
            public string? nombre { get; set; }
            public string? codigo { get; set; }
            public decimal? suma { get; set; }
            public decimal? prima { get; set; }
        }

        public class EditarSeguroValidator : AbstractValidator<EditarSeguro>
        {
            public EditarSeguroValidator()
            {
                RuleFor(r => r.nombre)
                    .NotEmpty()
                    .Must(nombre => IsString(nombre))
                    .WithMessage("Nombre solo admite letras.");
                RuleFor(r => r.codigo)
                    .NotEmpty();
                RuleFor(r => r.suma)
                    .NotEmpty();
                RuleFor(r => r.prima)
                    .NotEmpty();
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

        public class Manejador : IRequestHandler<EditarSeguro, Unit>
        {
            private readonly ApplicationDbContext _context;
            public Manejador(ApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<Unit> Handle(EditarSeguro request, CancellationToken cancellationToken)
            {
                var Validationresult = await new EditarSeguroValidator().ValidateAsync(request);
                if (!Validationresult.IsValid)
                {
                    throw new ManejadorException(HttpStatusCode.Unauthorized, new { Seguro = Validationresult.Errors });
                }
                //Busca el seguro a editar
                var seguro = await _context.Seguro.FindAsync(request.id);
                if (seguro == null)
                {
                    throw new ManejadorException(HttpStatusCode.NotFound, new { Seguro = "El Seguro no existe" });
                }
                seguro.nombre = request.nombre ?? seguro.nombre;
                seguro.codigo = request.codigo ?? seguro.codigo;
                seguro.suma = request.suma ?? seguro.suma;
                seguro.prima = request.prima ?? seguro.prima;

                var resultado = await _context.SaveChangesAsync();

                if (resultado > 0)
                    return Unit.Value;

                throw new ManejadorException(HttpStatusCode.InternalServerError, new { Seguro = "No se actualizo el seguro" });
            }
        }
    }
}
