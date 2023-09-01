using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SegurosAPI.Data;
using SegurosAPI.Modelo;
using SegurosAPI.Servicio.ManejadorError;
using System.Net;
using static SegurosAPI.Servicio.ClienteServicio.Nuevo;

namespace SegurosAPI.Servicio.SeguroServicio
{
    public class Nuevo
    {
        public class CrearSeguro : IRequest<Unit>
        {
            public string nombre { get; set; }
            public string codigo { get; set; }
            public decimal suma { get; set; }
            public decimal prima { get; set; }
        }

        public class CrearSeguroValidator : AbstractValidator<CrearSeguro>
        {
            public CrearSeguroValidator()
            {
                RuleFor(r => r.nombre).NotEmpty()
                    .Must(nombre => IsString(nombre))
                    .WithMessage("Nombre solo admite letras."); 
                RuleFor(r => r.codigo).NotEmpty();
                RuleFor(r => r.suma).NotEmpty();
                RuleFor(r => r.prima).NotEmpty();
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

        public class Manejador : IRequestHandler<CrearSeguro, Unit>
        {
            private readonly ApplicationDbContext _context;
            public Manejador(ApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<Unit> Handle(CrearSeguro request, CancellationToken cancellationToken)
            {
                //Valida los campos
                var validationresult = await new CrearSeguroValidator().ValidateAsync(request);
                if (!validationresult.IsValid)
                {
                    throw new ManejadorException(HttpStatusCode.Unauthorized, new { Seguro = validationresult.Errors });
                }

                //Comprueba la existencia de Seguro
                var SeguroEx = await _context.Seguro.AnyAsync(u => u.codigo.Equals(request.codigo) || u.nombre.Equals(request.nombre));
                if (!SeguroEx)
                {
                    var seguro = new Seguro
                    {
                        nombre = request.nombre,
                        codigo = request.codigo,
                        suma = request.suma,
                        prima = request.prima
                    };

                    _context.Seguro.Add(seguro);
                    var valor = await _context.SaveChangesAsync();
                    if (valor > 0)
                    {
                        return Unit.Value;
                    }
                    throw new ManejadorException(HttpStatusCode.InternalServerError, new { Seguro = "No se pudo almacenar el Seguro" });
                }

                throw new ManejadorException(HttpStatusCode.BadRequest, new { Seguro = "Seguro existente en la BD" });
            }
        }
    }
}
