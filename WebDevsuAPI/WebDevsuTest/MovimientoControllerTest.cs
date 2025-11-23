using Microsoft.AspNetCore.Mvc;
using Moq;
using WebDevsuApi.Controllers;
using WebDevsuLogic.Interfaces;
using WebDevsuDatabase.Models.DTOs;
using WebDevsuDatabase.Models.Consulta;
using WebDevsuDatabase.Models.Entidades;
using WebDevsuDatabase.Helper;
using WebDvpDatabase.Models.DTOs;

namespace WebDevsuApi.Tests.Controllers
{
    public class MovimientoControllerTests
    {
        private readonly Mock<IMovimientoService> _mockService;
        private readonly MovimientoController _controller;

        public MovimientoControllerTests()
        {
            _mockService = new Mock<IMovimientoService>();
            _controller = new MovimientoController(_mockService.Object);
        }

        // -------------------------------------------------------------
        // GET: api/movimiento
        // -------------------------------------------------------------
        [Fact]
        public async Task Get_ReturnsOk_WithMovimientos()
        {
            // Arrange
            var movimientos = new List<MovimientosConsulta> {
                new MovimientosConsulta { IdMovimiento = 1, Valor = 100 }
            };

            _mockService
                .Setup(s => s.ObtenerMovimientos())
                .ReturnsAsync(ResponseHelper.Ok(movimientos));

            // Act
            var result = await _controller.Get() as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            var response = Assert.IsType<ResponseDTO<List<MovimientosConsulta>>>(result.Value);
            Assert.True(response.Success);
            Assert.Single(response.Data);
        }

        // -------------------------------------------------------------
        // GET: api/movimiento/{id}
        // -------------------------------------------------------------
        [Fact]
        public async Task GetById_ReturnsOk_WhenExists()
        {
            var movimiento = new Movimiento { IdMovimiento = 1, Valor = 50 };

            _mockService
                .Setup(s => s.ObtenerMovimientoPorId(1))
                .ReturnsAsync(ResponseHelper.Ok(movimiento));

            var result = await _controller.Get(1) as OkObjectResult;

            Assert.NotNull(result);
            var response = Assert.IsType<ResponseDTO<Movimiento>>(result.Value);
            Assert.True(response.Success);
            Assert.Equal(1, response.Data.IdMovimiento);
        }

        // -------------------------------------------------------------
        // GET: api/movimiento/excede-cupo-diario/{idCuenta}/{fecha}
        // -------------------------------------------------------------
        [Fact]
        public async Task GetValidaCupoDiario_ReturnsOk()
        {
            _mockService
                .Setup(s => s.ExcedeCupoDiario(1, It.IsAny<DateTime>()))
                .ReturnsAsync(ResponseHelper.Ok(true));

            var result = await _controller.GetValidaCupoDiario(1, DateTime.Now) as OkObjectResult;

            Assert.NotNull(result);
            var response = Assert.IsType<ResponseDTO<bool>>(result.Value);
            Assert.True(response.Data);
        }

        // -------------------------------------------------------------
        // POST: api/movimiento
        // -------------------------------------------------------------
        [Fact]
        public async Task Post_ReturnsOk_OnCreate()
        {
            var dto = new MovimientoDTO { Valor = 100, TipoMovimiento = "C" };
            var movimiento = new Movimiento { IdMovimiento = 10, Valor = 100 };

            _mockService
                .Setup(s => s.CrearMovimiento(dto))
                .ReturnsAsync(ResponseHelper.Ok(movimiento));

            var result = await _controller.Post(dto) as OkObjectResult;

            Assert.NotNull(result);
            var response = Assert.IsType<ResponseDTO<Movimiento>>(result.Value);
            Assert.True(response.Success);
            Assert.Equal(10, response.Data.IdMovimiento);
        }

        // -------------------------------------------------------------
        // PUT: api/movimiento/{id}
        // -------------------------------------------------------------
        [Fact]
        public async Task Put_ReturnsOk_OnUpdate()
        {
            var dto = new MovimientoDTO { Valor = 200 };
            var movimiento = new Movimiento { IdMovimiento = 1, Valor = 200 };

            _mockService
                .Setup(s => s.ActualizarMovimiento(dto, 1))
                .ReturnsAsync(ResponseHelper.Ok(movimiento));

            var result = await _controller.Put(dto, 1) as OkObjectResult;

            Assert.NotNull(result);
            var response = Assert.IsType<ResponseDTO<Movimiento>>(result.Value);
            Assert.True(response.Success);
            Assert.Equal(200, response.Data.Valor);
        }

        // -------------------------------------------------------------
        // DELETE: api/movimiento/{id}
        // -------------------------------------------------------------
        [Fact]
        public async Task Delete_ReturnsOk_OnDelete()
        {
            var movimiento = new Movimiento { IdMovimiento = 1 };

            _mockService
                .Setup(s => s.EliminarMovimiento(1))
                .ReturnsAsync(ResponseHelper.Ok(movimiento));

            var result = await _controller.Delete(1) as OkObjectResult;

            Assert.NotNull(result);
            var response = Assert.IsType<ResponseDTO<Movimiento>>(result.Value);
            Assert.True(response.Success);
        }

        // -------------------------------------------------------------
        // GET: api/movimiento/consultar-reporte
        // -------------------------------------------------------------
        [Fact]
        public async Task ReporteMovimientos_ReturnsOk()
        {
            var lista = new List<MovimientosReporte>()
            {
                new MovimientosReporte { ValorMovimiento = 100 }
            };

            _mockService
                .Setup(s => s.ReporteMovimientos(1, It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .ReturnsAsync(ResponseHelper.Ok(lista));

            var result = await _controller.ReporteMovimientos(1, DateTime.Now.AddDays(-1), DateTime.Now) as OkObjectResult;

            Assert.NotNull(result);
            var response = Assert.IsType<ResponseDTO<List<MovimientosReporte>>>(result.Value);
            Assert.True(response.Success);
            Assert.Single(response.Data);
        }
    }
}
 