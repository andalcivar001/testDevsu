using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using WebDevsuLogic.Interfaces;
using WebDvpDatabase.Models.DTOs;


namespace WebDevsuApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClienteController : Controller
    {
        public IClienteService _clienteService;

        public ClienteController(IClienteService clienteService)
        {
            _clienteService = clienteService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {            
            var result = await this._clienteService.ObtenerClientes();
            return Ok(result);            
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await this._clienteService.ObtenerClientePorId(id);                
            return Ok(result);
        }

        [HttpPost()]
        public async Task<IActionResult> Post([FromBody] ClienteDTO ClienteDTO)
        {
            var result = await this._clienteService.CrearCliente(ClienteDTO);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromBody] ClienteDTO ClienteDTO, int id)
        {
            var result = await this._clienteService.ActualizarCliente(ClienteDTO, id);
            return Ok(result);            
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await this._clienteService.EliminarCliente(id);
            return Ok(result);            
        }
    }
}
