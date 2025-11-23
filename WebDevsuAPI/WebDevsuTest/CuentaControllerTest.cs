using Microsoft.AspNetCore.Mvc;
using Moq;
using WebDevsuApi.Controllers;
using WebDevsuLogic.Interfaces;
using WebDvpDatabase.Models.DTOs;
using Xunit;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebDevsuDatabase.Models.Consulta;
using WebDevsuDatabase.Models.DTOs;
using WebDevsuDatabase.Models.Entidades;

public class CuentaControllerTests
{
    private readonly Mock<ICuentaService> _mockService;
    private readonly CuentaController _controller;

    public CuentaControllerTests()
    {
        _mockService = new Mock<ICuentaService>();
        _controller = new CuentaController(_mockService.Object);
    }

    // -----------------------------------------------------
    // TEST: GET ALL
    // -----------------------------------------------------
    [Fact]
    public async Task Get_ReturnsOk_WithListOfCuentas()
    {
        // Arrange
        var cuentas = new ResponseDTO<List<CuentasConsulta>>
        {
            Success = true,
            Data = new List<CuentasConsulta>
            {
                new CuentasConsulta { IdCuenta = 1, NumeroCuenta = "1001",Estado= true, 
                    IdCliente = 1, NombreCliente ="Andres Alcivar",SaldoInicial = 0,
                    TipoCuenta = "A" }
            }
        };

        _mockService.Setup(s => s.ObtenerCuentas())
                    .ReturnsAsync(cuentas);

        // Act
        var result = await _controller.Get() as OkObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(200, result.StatusCode);
        Assert.IsType<ResponseDTO<List<CuentasConsulta>>>(result.Value);
    }

    // -----------------------------------------------------
    // TEST: GET BY ID
    // -----------------------------------------------------
    [Fact]
    public async Task Get_ById_ReturnsOk_WhenFound()
    {
        // Arrange
        var cuenta = new ResponseDTO<Cuenta>
        {
            Success = true,
            Data = new Cuenta { IdCuenta = 10, NumeroCuenta = "2002" }
        };

        _mockService.Setup(s => s.ObtenerCuentaPorId(10))
                    .ReturnsAsync(cuenta);

        // Act
        var result = await _controller.Get(10) as OkObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.IsType<ResponseDTO<Cuenta>>(result.Value);
        Assert.Equal(200, result.StatusCode);
    }

    // -----------------------------------------------------
    // TEST: GET SALDO ACTUAL
    // -----------------------------------------------------
    [Fact]
    public async Task ObtieneSaldoActual_ReturnsOk()
    {
        // Arrange
        var saldo = new ResponseDTO<decimal>
        {
            Success = true,
            Data = 500.75m
        };

        _mockService.Setup(s => s.ObtieneSaldoActualCuenta(1))
                    .ReturnsAsync(saldo);

        // Act
        var result = await _controller.ObtieneSaldoActual(1) as OkObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(200, result.StatusCode);
        Assert.IsType<ResponseDTO<decimal>>(result.Value);
    }

    // -----------------------------------------------------
    // TEST: POST
    // -----------------------------------------------------
    [Fact]
    public async Task Post_ReturnsOk()
    {
        // Arrange
        var dto = new CuentaDTO
        {
            NumeroCuenta = "9001",
            TipoCuenta = "A",
            Estado = true,
            IdCliente = 1,
            SaldoInicial = 100
        };

        var created = new ResponseDTO<Cuenta>
        {
            Success = true,
            Data = new Cuenta { IdCuenta = 1, NumeroCuenta = "9001" }
        };

        _mockService.Setup(s => s.CrearCuenta(dto))
                    .ReturnsAsync(created);

        // Act
        var result = await _controller.Post(dto) as OkObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(200, result.StatusCode);
        Assert.IsType<ResponseDTO<Cuenta>>(result.Value);
    }

    // -----------------------------------------------------
    // TEST: PUT
    // -----------------------------------------------------
    [Fact]
    public async Task Put_ReturnsOk_WhenUpdated()
    {
        // Arrange
        var dto = new CuentaDTO
        {
            NumeroCuenta = "8001",
            TipoCuenta = "C",
            Estado = true,
            IdCliente = 1,
            SaldoInicial = 200
        };

        var update = new ResponseDTO<Cuenta>
        {
            Success = true,
            Data = new Cuenta { IdCuenta = 1, NumeroCuenta = "8001" }
        };

        _mockService.Setup(s => s.ActualizarCuenta(dto, 1))
                    .ReturnsAsync(update);

        // Act
        var result = await _controller.Put(dto, 1) as OkObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(200, result.StatusCode);
    }

    // -----------------------------------------------------
    // TEST: DELETE
    // -----------------------------------------------------
    [Fact]
    public async Task Delete_ReturnsOk()
    {
        // Arrange
        var deleted = new ResponseDTO<Cuenta>
        {
            Success = true,
            Data = new Cuenta { IdCuenta = 1 }
        };

        _mockService.Setup(s => s.EliminarCuenta(1))
                    .ReturnsAsync(deleted);

        // Act
        var result = await _controller.Delete(1) as OkObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(200, result.StatusCode);
    }
}
