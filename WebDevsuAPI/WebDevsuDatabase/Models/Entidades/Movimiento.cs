using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebDevsuDatabase.Models.Entidades
{
    public class Movimiento
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("idMovimiento")]
        public int IdMovimiento { get; set; }

        [Required]
        [Column("fecha")]
        public DateTime? Fecha  { get; set; }

        [Required]
        [MaxLength(1)]
        [Column("tipoMovimiento")]
        public string? TipoMovimiento { get; set; }

        [Required]
        [Column("valor")]
        public decimal? Valor { get; set; }

        [Required]
        [Column("idCuenta")]
        public int? IdCuenta { get; set; }

        [Column("saldo")]
        public decimal? Saldo { get; set; }

        [Column("estado")]
        public bool? Estado { get; set; }



    }
}
