using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebDevsuDatabase.Models.Consulta
{
    public class CuentasConsulta
    {
        public int? IdCuenta { get; set; }
        public string? NumeroCuenta { get; set; }
        public string? TipoCuenta { get; set; }
        public decimal? SaldoInicial { get; set; }
        public bool? Estado { get; set; }
        public int? IdCliente { get; set; }
        public string? NombreCliente { get; set; }
    }
}
