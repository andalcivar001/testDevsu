using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebDevsuDatabase.Models.Entidades
{
    public class Cuenta
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("idCuenta")]
        public int IdCuenta { get; set; }


        [Required]
        [MaxLength(50)]
        [Column("numeroCuenta")]
        public string? NumeroCuenta { get; set; }

        [Required]
        [MaxLength(1)]
        [Column("tipoCuenta")]
        public string? TipoCuenta { get; set; }

        
        [Required]
        [Column("idCliente")]
        public int? IdCliente { get; set; }

        [Column("saldoInicial")]
        public decimal? SaldoInicial { get; set; }


        [Column("estado")]
        public bool? Estado { get; set; }


    }
}
