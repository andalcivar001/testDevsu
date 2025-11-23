using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebDevsuDatabase.Models.Consulta
{
    public class MovimientosConsulta
    {
        public int? IdMovimiento {  get; set; }
        public int? IdCuenta { get; set; }
        public DateTime? Fecha { get; set; }
        public string? TipoMovimiento { get; set; }
        public decimal? Saldo { get; set; }
        public decimal? Valor { get; set; }
        public string? NumeroCuenta { get; set; }
        public string? NombreCliente { get; set; }
        public decimal? SaldoDisponible { get; set; }
        public bool? Estado { get; set; }

    }
}
