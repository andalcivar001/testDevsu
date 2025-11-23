using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebDevsuDatabase.Data;
using WebDevsuDatabase.Helper;
using WebDevsuDatabase.Models.Consulta;
using WebDevsuDatabase.Models.DTOs;
using WebDevsuDatabase.Models.Entidades;
using WebDevsuLogic.Interfaces;
using WebDvpDatabase.Models.DTOs;

namespace WebDevsuLogic.Services
{
    public class CuentaService : ICuentaService
    {
        private ApplicationDbContext _context;

        public CuentaService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<ResponseDTO<List<CuentasConsulta>>> ObtenerCuentas()
        {
            try
            {
                var cuentas = await (from c in _context.Cuenta
                               join l in _context.Cliente
                               on c.IdCliente equals l.IdCliente
                               select new CuentasConsulta
                               {
                                   IdCliente = c.IdCliente,
                                   NumeroCuenta = c.NumeroCuenta,
                                   TipoCuenta = c.TipoCuenta,
                                   Estado = c.Estado,
                                   SaldoInicial = c.SaldoInicial,
                                   NombreCliente = l.Nombre,
                                   IdCuenta = c.IdCuenta                                   
                               }

                               ).ToListAsync();

                return ResponseHelper.Ok(cuentas, "Cuentas consultadas correctamente.");
            }
            catch (Exception ex)
            {
                return ResponseHelper.Error<List<CuentasConsulta>>($"Error al consultar las cuentas: {ex.Message}");
            }

        }

        public async Task<ResponseDTO<Cuenta>> ObtenerCuentaPorId(int idCuenta)
        {
            try
            {
                var cuenta = await this._context.Cuenta.Where(x => x.IdCuenta == idCuenta).FirstOrDefaultAsync();

                if (cuenta == null)
                    return ResponseHelper.NotFound<Cuenta>("La cuenta no existe.");


                return ResponseHelper.Ok(cuenta, "Cuenta consultada correctamente.");

            }
            catch (Exception ex)
            {
                return ResponseHelper.Error<Cuenta>($"Error al consultar la cuenta: {ex.Message}");

            }
        }

        public async Task<ResponseDTO<Cuenta>> CrearCuenta(CuentaDTO cuentaDTO)
        {
            try
            {
                var cuentaToCreate = new Cuenta();
                cuentaToCreate.NumeroCuenta = cuentaDTO.NumeroCuenta;
                cuentaToCreate.TipoCuenta = cuentaDTO.TipoCuenta;
                cuentaToCreate.SaldoInicial = cuentaDTO.SaldoInicial;
                cuentaToCreate.Estado = cuentaDTO.Estado;
                cuentaToCreate.IdCliente = cuentaDTO.IdCliente;
                this._context.Cuenta.Add(cuentaToCreate);
                await this._context.SaveChangesAsync();

                return ResponseHelper.Ok(cuentaToCreate, "Cuenta creada correctamente.");
            }
            catch (Exception ex)
            {                
                return ResponseHelper.Error<Cuenta>($"Error al crear la cuenta: {ex.Message}");
            }
        }

        public async Task<ResponseDTO<Cuenta>> ActualizarCuenta(CuentaDTO cuentaDTO, int id)
        {            
            try
            {
                var cuenta = await this._context.Cuenta.Where(x => x.IdCuenta == id).FirstOrDefaultAsync();

                if (cuenta == null)
                    return ResponseHelper.NotFound<Cuenta>("La cuenta no existe.");

                cuenta.NumeroCuenta = cuentaDTO.NumeroCuenta;
                cuenta.TipoCuenta = cuentaDTO.TipoCuenta;
                cuenta.SaldoInicial = cuentaDTO.SaldoInicial;
                cuenta.Estado = cuentaDTO.Estado;
                cuenta.IdCliente = cuentaDTO.IdCliente;

                await _context.SaveChangesAsync();

                return ResponseHelper.Ok(cuenta, "Cuenta actualizada correctamente.");
            }
            catch (Exception ex)
            {
                return ResponseHelper.Error<Cuenta>($"Error al actualizar la cuenta: {ex.Message}");
            }
        }

        public async Task<ResponseDTO<Cuenta>> EliminarCuenta(int id)
        {
            try
            {
                var cuenta = await this._context.Cuenta.Where(x => x.IdCuenta == id).FirstOrDefaultAsync();
                
                if (cuenta == null)
                    return ResponseHelper.NotFound<Cuenta>("La cuenta no existe.");
                
                this._context.Cuenta.Remove(cuenta);
                await this._context.SaveChangesAsync();
                return ResponseHelper.Ok(cuenta, "Cuenta eliminada correctamente.");
            }
            catch (Exception ex)
            {
                return ResponseHelper.Error<Cuenta>($"Error al actualizar la cuenta: {ex.Message}");
            }
        }

        public async Task<ResponseDTO<decimal>> ObtieneSaldoActualCuenta(int id)
        {
            try
            {
                decimal cuenta = await this._context.Cuenta.Where(x => x.IdCuenta == id).Select(x=>x.SaldoInicial).FirstOrDefaultAsync() ?? 0;
                decimal movimientos = await this._context.Movimiento.SumAsync(x => x.TipoMovimiento == "C" ? x.Valor : x.Valor * -1) ?? 0;
                decimal saldoACtual = cuenta + movimientos;
                return ResponseHelper.Ok(saldoACtual, "Saldo actual obtenido correctamente.");
            }
            catch (Exception ex)
            {
                return ResponseHelper.Error<decimal>($"Error al obtener el saldo la cuenta: {ex.Message}");
            }
        }
    }
}
