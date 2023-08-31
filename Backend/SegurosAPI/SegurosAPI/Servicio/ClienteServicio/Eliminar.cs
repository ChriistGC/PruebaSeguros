using MediatR;
using SegurosAPI.Data;
using SegurosAPI.Modelo;
using SegurosAPI.Servicio.ManejadorError;
using System.Net;

namespace SegurosAPI.Servicio.ClienteServicio
{
    public class Eliminar
    {
        public class EliminarCliente : IRequest<Unit>
        {
            public int Id { get; set; }
        }
        public class Manejador : IRequestHandler<EliminarCliente, Unit>
        {
            private readonly ApplicationDbContext _context;
            public Manejador(ApplicationDbContext context)
            {
                _context = context;
            }
            public async Task<Unit> Handle(EliminarCliente request, CancellationToken cancellationToken)
            {
                var cliente = await _context.Cliente.FindAsync(request.Id);
                if (cliente == null)
                {
                    throw new ManejadorException(HttpStatusCode.NotFound, new { Cliente = "No se encontró el cliente a eliminar" });
                }

                // Eliminar conexiones existentes para este cliente
                var segurosAnteriores = _context.ClienteSeguro.Where(cs => cs.clienteid == request.Id);
                if (segurosAnteriores != null)
                {
                    _context.ClienteSeguro.RemoveRange(segurosAnteriores);
                }

                _context.Remove(cliente);
                var resultado = await _context.SaveChangesAsync();

                if (resultado > 0)
                    return Unit.Value;

                throw new ManejadorException(HttpStatusCode.BadRequest, new { Cliente = "No se eliminó el cliente" });
            }
        }
    }
}
