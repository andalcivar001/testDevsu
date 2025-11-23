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
    public interface ICuentaService
    {
        Task<ResponseDTO<List<CuentasConsulta>>> ObtenerCuentas();

        Task<ResponseDTO<Cuenta>> ObtenerCuentaPorId(int idCuenta);

        Task<ResponseDTO<Cuenta>> CrearCuenta(CuentaDTO cuentaDTO);

        Task<ResponseDTO<Cuenta>> ActualizarCuenta(CuentaDTO cuentaDTO, int id);

        Task<ResponseDTO<Cuenta>> EliminarCuenta(int id);

        Task<ResponseDTO<decimal>> ObtieneSaldoActualCuenta(int id);
    }
}
