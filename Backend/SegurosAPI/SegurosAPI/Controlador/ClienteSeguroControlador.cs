using MediatR;
using Microsoft.AspNetCore.Mvc;
using SegurosAPI.Servicio.ClienteSeguroServicio;
using SegurosAPI.Servicio.DTOs;

namespace SegurosAPI.Controlador
{
    [ApiController]
    [Route("api/datos")]
    public class ClienteSeguroControlador : ControllerBase
    {
        private readonly IMediator _mediator;

        public ClienteSeguroControlador(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<ActionResult<Unit>> guardarSeguros(RegistrarSeguro.AsignarCliente data)
        {
            return await _mediator.Send(data);
        }

        [HttpGet("seguros/{cedula}")]
        public async Task<ActionResult<ClienteDetalladoDTO>> obtenerSeguros(string cedula)
        {
            return await _mediator.Send(new ConsultaCliente.ObtenerCliente{Cedula = cedula});
        }
        [HttpGet("clientes/{codigo}")]
        public async Task<ActionResult<SeguroDetalladoDTO>> obtenerClientes(string codigo)
        {
            return await _mediator.Send(new ConsultaSeguro.obtenerSeguro { Codigo = codigo });
        }

    }
}
