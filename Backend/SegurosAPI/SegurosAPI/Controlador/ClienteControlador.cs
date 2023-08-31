using MediatR;
using Microsoft.AspNetCore.Mvc;
using SegurosAPI.Servicio.ClienteServicio;
using SegurosAPI.Servicio.DTOs;
using SegurosAPI.Servicio.FileServicio;

namespace SegurosAPI.Controlador
{
    [ApiController]
    [Route("api/cliente")]
    public class ClienteControlador : ControllerBase
    {
        private readonly IMediator _mediator;

        public ClienteControlador(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<List<ClienteDTO>>> Get()
        {
            return await _mediator.Send(new Consulta.ListaCliente());
        }

        [HttpPost]
        public async Task<ActionResult<Unit>> Crear(Nuevo.CrearCliente data)
        {            
            return await _mediator.Send(data);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Unit>> Editar(int id, Editar.EditarCliente data)
        {
            data.id = id;
            return await _mediator.Send(data);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Unit>> Eliminar(int id)
        {
            return await _mediator.Send(new Eliminar.EliminarCliente { Id = id });
        }

        [HttpPost("file")]
        public async Task<ActionResult<Unit>> AddClienteporDocumento(IFormFile file)
        {
            return await _mediator.Send(new AgregarArchivo.AddFile { File =file});
        }
    }
}
