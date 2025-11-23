using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace  WebDvpDatabase.Models.DTOs
{
    public class MovimientoDTO
    {
        public int? IdCuenta { get; set; }
        public DateTime Fecha { get; set; }
        public string? TipoMovimiento { get; set; }
        public decimal? Saldo { get; set; }
        public decimal? Valor { get; set; }
        public bool? Estado { get; set; }

    }
}
