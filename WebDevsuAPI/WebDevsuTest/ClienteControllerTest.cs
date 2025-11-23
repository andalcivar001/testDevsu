using Moq;
using Xunit;
using Microsoft.AspNetCore.Mvc;

using WebDevsuApi.Controllers;
using WebDevsuLogic.Interfaces;
using WebDvpDatabase.Models.DTOs;
using WebDevsuDatabase.Models.Entidades;
using WebDevsuDatabase.Models.DTOs;

namespace WebDevsuApi.Tests
{
    public class ClienteControllerTests
    {
        private readonly Mock<IClienteService> _mockService;
        private readonly ClienteController _controller;

        public ClienteControllerTests()
        {
            _mockService = new Mock<IClienteService>();
            _controller = new ClienteController(_mockService.Object);
        }

        // ======================================================
        // 1️⃣ GET: ObtenerClientes()
        // ======================================================
        [Fact]
        public async Task Get_ReturnsOk_WithResponseDTOList()
        {
            // Arrange
            var responseMock = new ResponseDTO<List<Cliente>>
            {
                Success = true,
                Message = "Clientes consultados correctamente.",
                Data = new List<Cliente>
                {
                    new Cliente { IdCliente = 1, Nombre = "Andres Alcivar",NumeroIdentificacion = "09999"
                    ,Direccion="Centro GYE",Telefono ="0989889",FechaNacimiento= Convert.ToDateTime("1990-01-01")
                    ,Genero="M",Password="1231456", Estado = true, FechaCreacion = DateTime.Now },
                    new Cliente { IdCliente = 2, Nombre = "Kimberly Alejandro", NumeroIdentificacion = "09999"
                    ,Direccion="Centro GYE",Telefono ="0989889",FechaNacimiento= Convert.ToDateTime("1990-01-01")
                    ,Genero="F",Password="1231456", Estado = true, FechaCreacion = DateTime.Now}
                }
            };

            _mockService
                .Setup(s => s.ObtenerClientes())
                .ReturnsAsync(responseMock);

            // Act
            var result = await _controller.Get();

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ResponseDTO<List<Cliente>>>(ok.Value);

            Assert.True(response.Success);
            Assert.Equal(2, response.Data.Count);
        }

        // ======================================================
        // 2️⃣ GET/{id}: ObtenerClientePorId()
        // ======================================================
        [Fact]
        public async Task GetById_ReturnsOk_WithCliente()
        {
            // Arrange
            var responseMock = new ResponseDTO<Cliente>
            {
                Success = true,
                Message = "Cliente consultado correctamente.",
                Data = new Cliente { IdCliente = 5, Nombre = "Juan" }
            };

            _mockService
                .Setup(s => s.ObtenerClientePorId(5))
                .ReturnsAsync(responseMock);

            // Act
            var result = await _controller.Get(5);

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ResponseDTO<Cliente>>(ok.Value);

            Assert.True(response.Success);
            Assert.Equal(5, response.Data.IdCliente);
        }

        // ======================================================
        // 3️⃣ POST: CrearCliente()
        // ======================================================
        [Fact]
        public async Task Post_ReturnsOk_WhenClienteCreated()
        {
            // Arrange
            var dto = new ClienteDTO
            {
                Nombre = "Andres Alcivar",
                NumeroIdentificacion = "09999",
                Direccion = "Centro GYE",
                Telefono = "0989889",
                FechaNacimiento = Convert.ToDateTime("1990-01-01"),
                Genero = "M",
                Password = "1231456",
                Estado = true,
                FechaCreacion = DateTime.Now
            };

            var responseMock = new ResponseDTO<Cliente>
            {
                Success = true,
                Message = "Cliente creado correctamente.",
                Data = new Cliente
                {
                    IdCliente = 1,
                    Nombre = "Andres Alcivar",
                    NumeroIdentificacion = "09999",
                    Direccion = "Centro GYE",
                    Telefono = "0989889",
                    FechaNacimiento = Convert.ToDateTime("1990-01-01"),
                    Genero = "M",
                    Password = "1231456",
                    Estado = true,
                    FechaCreacion = DateTime.Now
                }
        };

            _mockService
                .Setup(s => s.CrearCliente(dto))
                .ReturnsAsync(responseMock);

            // Act
            var result = await _controller.Post(dto);

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ResponseDTO<Cliente>>(ok.Value);

            Assert.True(response.Success);
            Assert.Equal("Cliente Nuevo", response.Data.Nombre);
        }

        // ======================================================
        // 4️⃣ PUT: ActualizarCliente()
        // ======================================================
        [Fact]
        public async Task Put_ReturnsOk_WhenClienteUpdated()
        {
            // Arrange
            var dto = new ClienteDTO { Nombre = "Actualizado" };

            var responseMock = new ResponseDTO<Cliente>
            {
                Success = true,
                Message = "Cliente actualizado correctamente.",
                Data = new Cliente { IdCliente = 3, Nombre = "Actualizado" }
            };

            _mockService
                .Setup(s => s.ActualizarCliente(dto, 3))
                .ReturnsAsync(responseMock);

            // Act
            var result = await _controller.Put(dto, 3);

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ResponseDTO<Cliente>>(ok.Value);

            Assert.True(response.Success);
            Assert.Equal("Actualizado", response.Data.Nombre);
        }

        // ======================================================
        // 5️⃣ DELETE: EliminarCliente()
        // ======================================================
        [Fact]
        public async Task Delete_ReturnsOk_WhenClienteDeleted()
        {
            // Arrange
            var responseMock = new ResponseDTO<Cliente>
            {
                Success = true,
                Message = "Cliente eliminado correctamente.",
                Data = new Cliente { IdCliente = 1, Nombre = "Eliminado" }
            };

            _mockService
                .Setup(s => s.EliminarCliente(7))
                .ReturnsAsync(responseMock);

            // Act
            var result = await _controller.Delete(7);

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ResponseDTO<Cliente>>(ok.Value);

            Assert.True(response.Success);
            Assert.Equal(7, response.Data.IdCliente);
        }
    }
}
