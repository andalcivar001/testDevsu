using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebDevsuDatabase.Data;
using WebDevsuDatabase.Helper;
using WebDevsuDatabase.Models.DTOs;
using WebDevsuDatabase.Models.Entidades;
using WebDevsuLogic.Interfaces;
using WebDvpDatabase.Models.DTOs;

namespace WebDevsuLogic.Services
{
    public class ClienteService : IClienteService
    {
        private ApplicationDbContext _context;

        public ClienteService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<ResponseDTO<List<Cliente>>> ObtenerClientes()
        {
            try
            {
                var clientes = await this._context.Cliente.ToListAsync();
                return ResponseHelper.Ok(clientes, "Clientes consultados correctamente.");
            }
            catch (Exception ex)
            {
                return ResponseHelper.Error< List<Cliente>> ($"Error al consultar los clientes: {ex.Message}");
            }

        }

        public async Task<ResponseDTO<Cliente>> ObtenerClientePorId(int idCliente)
        {
            try
            {
                var cliente = await this._context.Cliente.Where(x => x.IdCliente == idCliente).FirstOrDefaultAsync();
                
                if (cliente == null)
                    return ResponseHelper.NotFound<Cliente>("El cliente no existe.");


                return ResponseHelper.Ok(cliente, "Cliente consultado correctamente.");

            }
            catch (Exception ex)
            {
                return ResponseHelper.Error<Cliente>($"Error al consultar el cliente: {ex.Message}");

            }
        }

        public async Task<ResponseDTO<Cliente>> CrearCliente(ClienteDTO clienteDTO)
        {
            try
            {
                var clienteToCreate = new Cliente();
                clienteToCreate.Nombre = clienteDTO.Nombre;
                clienteToCreate.Genero = clienteDTO.Genero;
                clienteToCreate.FechaNacimiento = clienteDTO.FechaNacimiento;
                clienteToCreate.NumeroIdentificacion = clienteDTO.NumeroIdentificacion;
                clienteToCreate.Direccion = clienteDTO.Direccion;
                clienteToCreate.Telefono = clienteDTO.Telefono;
                clienteToCreate.Password = clienteDTO.Password;
                clienteToCreate.Estado = clienteDTO.Estado;
                clienteToCreate.FechaCreacion = DateTime.Now;

                this._context.Cliente.Add(clienteToCreate);
                
                await this._context.SaveChangesAsync();
                
                return ResponseHelper.Ok(clienteToCreate, "Cliente creado correctamente.");
            }
            catch (Exception ex)
            {
                return ResponseHelper.Error<Cliente>($"Error al crear el cliente: {ex.Message}");
            };
        }
    

    public async Task<ResponseDTO<Cliente>> ActualizarCliente(ClienteDTO clienteDTO, int id)
        {
            try
            {
                var clienteToUpdate = await this._context.Cliente.Where(x => x.IdCliente == id).FirstOrDefaultAsync();

                if (clienteToUpdate == null)
                    return ResponseHelper.NotFound<Cliente>("El cliente no existe.");


                clienteToUpdate.Nombre = clienteDTO.Nombre;
                clienteToUpdate.Genero = clienteDTO.Genero;
                clienteToUpdate.FechaNacimiento = clienteDTO.FechaNacimiento;
                clienteToUpdate.NumeroIdentificacion = clienteDTO.NumeroIdentificacion;
                clienteToUpdate.Direccion = clienteDTO.Direccion;
                clienteToUpdate.Telefono = clienteDTO.Telefono;
                clienteToUpdate.Password = clienteDTO.Password;
                clienteToUpdate.Estado = clienteDTO.Estado;
                clienteToUpdate.FechaCreacion = DateTime.Now;

                await this._context.SaveChangesAsync();

                return ResponseHelper.Ok(clienteToUpdate, "Cliente actualizado correctamente.");

            }
            catch (Exception ex)
            {
                return ResponseHelper.Error<Cliente>($"Error al actualizar el cliente: {ex.Message}");
            }
            ;
        }

        public async Task<ResponseDTO<Cliente>> EliminarCliente(int id)
        {
            try
            {
                var persona = await this._context.Cliente.Where(x => x.IdCliente == id).FirstOrDefaultAsync();

                if (persona == null)
                    return ResponseHelper.NotFound<Cliente>("El cliente no existe.");

                this._context.Cliente.Remove(persona);
                
                await this._context.SaveChangesAsync();

                return ResponseHelper.Ok(persona, "Cliente eliminado correctamente.");

            }
            catch (Exception ex)
            {
                return ResponseHelper.Error<Cliente>($"Error al eliminar el cliente: {ex.Message}");
            }

        }


    }
}
