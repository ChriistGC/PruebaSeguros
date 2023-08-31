using MediatR;
using SegurosAPI.Data;
using SegurosAPI.Modelo;
using SegurosAPI.Servicio.ManejadorError;
using System.Net;

namespace SegurosAPI.Servicio.ClienteSeguroServicio
{
    public class RegistrarSeguro
    {
        public class AsignarCliente : IRequest<Unit>
        {
            public int Id { get; set; }
            public List<int> ListaSeguros { get; set; }
        }

        public class Manejador : IRequestHandler<AsignarCliente, Unit>
        {
            private readonly ApplicationDbContext _context;
            public Manejador(ApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<Unit> Handle(AsignarCliente request, CancellationToken cancellationToken)
            {
                var cliente = await _context.Cliente.FindAsync(request.Id);
                if (cliente == null)
                {
                    throw new ManejadorException(HttpStatusCode.NotFound, new { ClienteSeguro = "No se encontró el cliente" });
                }

                // Eliminar seguros existentes para este cliente
                var segurosAnteriores = _context.ClienteSeguro.Where(cs => cs.clienteid == request.Id);
                if (segurosAnteriores!= null)
                {
                    _context.ClienteSeguro.RemoveRange(segurosAnteriores);
                }

                if (request.ListaSeguros != null)
                {
                    foreach (var id in request.ListaSeguros)
                    {
                        var clienteSeguro = new ClienteSeguro
                        {
                            clienteid = request.Id,
                            seguroid = id
                        };
                        _context.ClienteSeguro.Add(clienteSeguro);
                    }
                }
                var valor = await _context.SaveChangesAsync();
                if (valor > 0)
                {
                    return Unit.Value;
                }
                throw new ManejadorException(HttpStatusCode.BadGateway, new { ClienteSeguro = "No se pudo asignar el seguro al cliente" });

            }
        }
    }
}
