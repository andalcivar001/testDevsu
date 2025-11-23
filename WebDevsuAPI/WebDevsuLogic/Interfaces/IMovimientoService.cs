using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebDevsuDatabase.Models.Consulta;
using WebDevsuDatabase.Models.DTOs;
using WebDevsuDatabase.Models.Entidades;
using WebDvpDatabase.Models.DTOs;

namespace WebDevsuLogic.Interfaces
{
    public interface IMovimientoService
    {
        Task<ResponseDTO<List<MovimientosConsulta>>> ObtenerMovimientos();

        Task<ResponseDTO<Movimiento>> ObtenerMovimientoPorId(int idMovimiento);

        Task<ResponseDTO<Movimiento>> CrearMovimiento(MovimientoDTO  movimientoDTO);

        Task<ResponseDTO<Movimiento>> ActualizarMovimiento(MovimientoDTO movimientoDTO, int id);

        Task<ResponseDTO<Movimiento>> EliminarMovimiento(int id);

        Task<ResponseDTO<bool>> ExcedeCupoDiario(int idCuenta, DateTime fecha);

        Task<ResponseDTO<List<MovimientosReporte>>> ReporteMovimientos( int idCliente, DateTime fechaDesde, DateTime fechaHasta);
    }
}
