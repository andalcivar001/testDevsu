using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using QuestPDF.Fluent;
using WebDevsuLogic.Interfaces;
using WebDevsuLogic.Services;
using WebDvpDatabase.Models.DTOs;


namespace WebDevsuApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovimientoController : Controller
    {
        public IMovimientoService _MovimientoService;

        public MovimientoController(IMovimientoService MovimientoService)
        {
            _MovimientoService = MovimientoService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {            
            var result = await this._MovimientoService.ObtenerMovimientos();
            return Ok(result);            
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await this._MovimientoService.ObtenerMovimientoPorId(id);                
            return Ok(result);
        }


        [HttpGet("excede-cupo-diario/{idCuenta}/{fecha}")]
        public async Task<IActionResult> GetValidaCupoDiario(int idCuenta, DateTime fecha)
        {
            var result = await this._MovimientoService.ExcedeCupoDiario(idCuenta, fecha);
            return Ok(result);
        }

        [HttpPost()]
        public async Task<IActionResult> Post([FromBody] MovimientoDTO MovimientoDTO)
        {
            var result = await this._MovimientoService.CrearMovimiento(MovimientoDTO);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromBody] MovimientoDTO MovimientoDTO, int id)
        {
            var result = await this._MovimientoService.ActualizarMovimiento(MovimientoDTO, id);
            return Ok(result);            
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await this._MovimientoService.EliminarMovimiento(id);
            return Ok(result);            
        }

        [HttpGet("consultar-reporte")]
        public async Task<IActionResult> ReporteMovimientos(int idCliente, DateTime fechaDesde, DateTime fechaHasta)
        {
            var result = await this._MovimientoService.ReporteMovimientos(idCliente, fechaDesde, fechaHasta);            return Ok(result);
        }

        [HttpGet("exportar-movimientos")]
        public async Task<IActionResult> ExportarMovimientos(
        int idCliente,
        DateTime fechaDesde,
        DateTime fechaHasta)
        {
            try
            {


                var respuesta = await this._MovimientoService.ReporteMovimientos(idCliente, fechaDesde, fechaHasta);

                if (!respuesta.Success)
                    return BadRequest(respuesta);

                var documento = new ReporteMovimientosDocument(respuesta.Data, fechaDesde, fechaHasta);

                //byte[] pdfBytes = documento.GeneratePdf();
                byte[] pdfBytes;
                try
                {
                    pdfBytes = documento.GeneratePdf();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("ERROR EN PDF: " + ex.ToString());
                    throw; // vuelve a lanzar la excepción
                }

                return Ok(new
                {
                    archivoBase64 = Convert.ToBase64String(pdfBytes),
                    nombreArchivo = "ReporteMovimientos.pdf",
                    contentType = "application/pdf"
                });
            }
            catch  (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        
    }
}
