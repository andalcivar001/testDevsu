using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebDevsuDatabase.Models.DTOs;
using WebDevsuDatabase.Models.Entidades;
using WebDvpDatabase.Models.DTOs;

namespace WebDevsuLogic.Interfaces
{
    public interface IClienteService
    {
        Task<ResponseDTO<List<Cliente>>> ObtenerClientes();

        Task<ResponseDTO<Cliente>> ObtenerClientePorId(int idCliente);

        Task<ResponseDTO<Cliente>> CrearCliente(ClienteDTO clienteDTO);

        Task<ResponseDTO<Cliente>> ActualizarCliente(ClienteDTO clienteDTO, int id);

        Task<ResponseDTO<Cliente>> EliminarCliente(int id);
    }
}
