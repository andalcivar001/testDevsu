using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using WebDevsuLogic.Interfaces;
using WebDvpDatabase.Models.DTOs;


namespace WebDevsuApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CuentaController : Controller
    {
        public ICuentaService _CuentaService;

        public CuentaController(ICuentaService CuentaService)
        {
            _CuentaService = CuentaService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {            
            var result = await this._CuentaService.ObtenerCuentas();
            return Ok(result);            
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await this._CuentaService.ObtenerCuentaPorId(id);                
            return Ok(result);
        }

        [HttpGet("obtiene-saldo-actual/{id}")]
        public async Task<IActionResult> ObtieneSaldoActual(int id)
        {
            var result = await this._CuentaService.ObtieneSaldoActualCuenta(id);
            return Ok(result);
        }


        [HttpPost()]
        public async Task<IActionResult> Post([FromBody] CuentaDTO CuentaDTO)
        {
            var result = await this._CuentaService.CrearCuenta(CuentaDTO);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromBody] CuentaDTO CuentaDTO, int id)
        {
            var result = await this._CuentaService.ActualizarCuenta(CuentaDTO, id);
            return Ok(result);            
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await this._CuentaService.EliminarCuenta(id);
            return Ok(result);            
        }
    }
}
