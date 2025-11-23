using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebDevsuDatabase.Models.Consulta
{
    public class MovimientosReporte
    {
        public DateTime? Fecha { get; set; }
        public string? Cliente { get; set; }
        public string? NumeroCuenta { get; set; }
        public string? TipoCuenta { get; set; }
        public decimal? SaldoInicial { get; set; }
        public bool? Estado { get; set; }
        public decimal? ValorMovimiento { get; set; }
        public decimal? SaldoDisponible { get; set; }

    }
}
