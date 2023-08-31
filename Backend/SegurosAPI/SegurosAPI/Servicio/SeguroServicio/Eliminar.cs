using MediatR;
using SegurosAPI.Data;
using SegurosAPI.Servicio.ManejadorError;
using System.Net;

namespace SegurosAPI.Servicio.SeguroServicio
{
    public class Eliminar
    {
        public class EliminarSeguro : IRequest<Unit>
        {
            public int Id { get; set; }
        }
        public class Manejador : IRequestHandler<EliminarSeguro, Unit>
        {
            private readonly ApplicationDbContext _context;
            public Manejador(ApplicationDbContext context)
            {
                _context = context;
            }
            public async Task<Unit> Handle(EliminarSeguro request, CancellationToken cancellationToken)
            {
                var seguro = await _context.Seguro.FindAsync(request.Id);
                if (seguro == null)
                {
                    throw new ManejadorException(HttpStatusCode.NotFound, new { seguro = "No se encontró el seguro a eliminar" });
                }
                // Eliminar conexiones existentes para este seguro
                var segurosAnteriores = _context.ClienteSeguro.Where(cs => cs.seguroid == request.Id);
                if (segurosAnteriores != null)
                {
                    _context.ClienteSeguro.RemoveRange(segurosAnteriores);
                }

                _context.Remove(seguro);
                var resultado = await _context.SaveChangesAsync();

                if (resultado > 0)
                    return Unit.Value;

                throw new ManejadorException(HttpStatusCode.BadRequest, new { seguro = "No se eliminó el seguro" });
            }
        }
    }
}
