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
    public class MovimientoService : IMovimientoService
    {
        private ApplicationDbContext _context;
        private decimal limiteCupo = 1000;

        public MovimientoService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<ResponseDTO<List<MovimientosConsulta>>> ObtenerMovimientos()
        {
            try
            {
                var movimientos = await (from m in _context.Movimiento
                                         join c in _context.Cuenta
                                         on m.IdCuenta equals c.IdCuenta
                                         join e in _context.Cliente
                                         on c.IdCliente equals e.IdCliente
                                         select new MovimientosConsulta
                                         {
                                             IdMovimiento = m.IdMovimiento,
                                             TipoMovimiento = m.TipoMovimiento,
                                             Fecha = m.Fecha,
                                             Valor = m.Valor,
                                             Saldo = m.Saldo,
                                             NumeroCuenta = c.NumeroCuenta,
                                             NombreCliente = e.Nombre,
                                             Estado =  m.Estado
                                         }
                                         ).ToListAsync();
                return ResponseHelper.Ok(movimientos, "Movimientos consultados correctamente.");

            }
            catch (Exception ex)
            {
                return ResponseHelper.Error<List<MovimientosConsulta>>($"Error al consultar los movimientos: {ex.Message}");
                
            }

        }

        public async Task<ResponseDTO<Movimiento>> ObtenerMovimientoPorId(int idMovimiento)
        {
            try
            {
                var movimiento = await this._context.Movimiento.Where(x => x.IdMovimiento == idMovimiento).FirstOrDefaultAsync();
                
                if (movimiento == null)
                    return ResponseHelper.NotFound<Movimiento>("El movimiento no existe.");

                return ResponseHelper.Ok(movimiento, "Movimiento consultado correctamente.");

            }
            catch (Exception ex)
            {
                return ResponseHelper.Error<Movimiento>($"Error al consultar el mmovimiento: {ex.Message}");

            }
        }

        public async Task<ResponseDTO<Movimiento>> CrearMovimiento(MovimientoDTO movimientoDTO)
        {
            try
            {
                var movimientoToCreate = new Movimiento();
                movimientoToCreate.IdCuenta = movimientoDTO.IdCuenta;
                movimientoToCreate.Fecha = movimientoDTO.Fecha;
                movimientoToCreate.TipoMovimiento = movimientoDTO.TipoMovimiento;
                movimientoToCreate.Saldo = movimientoDTO.Saldo;
                movimientoToCreate.Valor = movimientoDTO.Valor;
                movimientoToCreate.Estado = movimientoDTO.Estado;

                this._context.Movimiento.Add(movimientoToCreate);
                await this._context.SaveChangesAsync();
                return ResponseHelper.Ok(movimientoToCreate, "Movimiento creado correctamente.");
            }
            catch (Exception ex)
            {
                return ResponseHelper.Error<Movimiento>($"Error al crear el mmovimiento: {ex.Message}");
            }
        }

        public async Task<ResponseDTO<Movimiento>> ActualizarMovimiento(MovimientoDTO movimientoDTO, int id)
        {
            try
            {
                var movimientoToUpdate = await this._context.Movimiento.Where(x => x.IdMovimiento == id).FirstOrDefaultAsync();

                if (movimientoToUpdate == null)
                    return ResponseHelper.NotFound<Movimiento>("El movimiento no existe.");

                movimientoToUpdate.IdCuenta = movimientoDTO.IdCuenta;
                movimientoToUpdate.IdCuenta = movimientoDTO.IdCuenta;
                movimientoToUpdate.Fecha = movimientoDTO.Fecha;
                movimientoToUpdate.TipoMovimiento = movimientoDTO.TipoMovimiento;
                movimientoToUpdate.Saldo = movimientoDTO.Saldo;
                movimientoToUpdate.Valor = movimientoDTO.Valor;
                movimientoToUpdate.Estado = movimientoDTO.Estado;


                await this._context.SaveChangesAsync();

                return ResponseHelper.Ok(movimientoToUpdate, "Movimiento actualizado correctamente.");

            }
            catch (Exception ex)
            {
                return ResponseHelper.Error<Movimiento>($"Error al crear el mmovimiento: {ex.Message}");
            }
            }

        public async Task<ResponseDTO<Movimiento>> EliminarMovimiento(int id)
        {

            try
            {
                var movimiento = await this._context.Movimiento.Where(x => x.IdMovimiento == id).FirstOrDefaultAsync();

                if (movimiento == null)
                    return ResponseHelper.NotFound<Movimiento>("El movimiento no existe.");

                this._context.Movimiento.Remove(movimiento);
                await this._context.SaveChangesAsync();

                return ResponseHelper.Ok(movimiento, "Movimiento eliminado correctamente.");
            }
            catch (Exception ex)
            {
                return ResponseHelper.Error<Movimiento>($"Error al eliminar el mmovimiento: {ex.Message}");
            }
            
        }

        public async Task<ResponseDTO<bool>> ExcedeCupoDiario(int idCuenta, DateTime fecha)
        {
            try
            {

                decimal sumMovimientosDiario = await _context.Movimiento
                .Where(x =>
                    x.IdCuenta == idCuenta
                    && x.Fecha.HasValue
                    && x.Fecha.Value.Date == fecha.Date
                    && x.TipoMovimiento == "D")
                .SumAsync(x => x.Valor) ?? 0;

                bool excede = false;
                if(sumMovimientosDiario > this.limiteCupo)
                {
                    excede = true;
                }                

                return ResponseHelper.Ok(excede, "Cupo diario consultado correctamente.");
            }
            catch (Exception ex)
            {
                return ResponseHelper.Error<bool>($"Error al validar el cupo diario: {ex.Message}");

            }
        }
        public async Task<ResponseDTO<List<MovimientosReporte>>> ReporteMovimientos(int idCliente,DateTime fechaDesde, DateTime fechaHasta)
        {
            try
            {
                var movimientos = await (from m in _context.Movimiento
                                         join c in _context.Cuenta
                                         on m.IdCuenta equals c.IdCuenta
                                         join e in _context.Cliente
                                         on c.IdCliente equals e.IdCliente
                                         where m.Fecha!.Value.Date >= fechaDesde.Date
                                         && m.Fecha.Value.Date <= fechaHasta.Date
                                         && e.IdCliente == idCliente
                                         select new MovimientosReporte
                                         {
                                             Fecha = m.Fecha,
                                             NumeroCuenta = c.NumeroCuenta,
                                             Cliente = e.Nombre,
                                             TipoCuenta = c.TipoCuenta,
                                             SaldoInicial = c.SaldoInicial,
                                             Estado = m.Estado,
                                             ValorMovimiento = m.TipoMovimiento == "C" ? m.Valor : m.Valor * -1,
                                             SaldoDisponible = c.SaldoInicial - (m.TipoMovimiento == "C" ? m.Valor : m.Valor * -1)
                                         }
                                         ).OrderBy(x=> x.Fecha ).ToListAsync();
                return ResponseHelper.Ok(movimientos, "Movimientos consultados correctamente.");

            }
            catch (Exception ex)
            {
                return ResponseHelper.Error<List<MovimientosReporte>>($"Error al consultar los movimientos: {ex.Message}");

            }

        }



    }
}
