using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace  WebDvpDatabase.Models.DTOs
{
    public class CuentaDTO
    {
        public string? NumeroCuenta { get; set; }
        public string? TipoCuenta { get; set; }
        public decimal? SaldoInicial { get; set; }
        public bool? Estado { get; set; }
        public int? IdCliente { get; set; }
    }
}
