using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebDevsuDatabase.Models.Entidades
{
    public class Cliente
    {
        [Key]
        [Column("idCliente")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdCliente { get; set; }

        [MaxLength(300)]
        [Required]
        [Column("nombre")]
        public string? Nombre { get; set; }

        [Required]
        [MaxLength(1)]
        [Column("genero")]
        public string? Genero { get; set; }

        [Required]
        [Column("fechaNacimiento")]
        public DateTime? FechaNacimiento { get; set; }

        [Required]
        [MaxLength(20)]
        [Column("numeroIdentificacion")]
        public string? NumeroIdentificacion { get; set; }


        [MaxLength(500)]
        [Column("direccion")]
        public string? Direccion { get; set; }

        [MaxLength(50)]
        [Column("telefono")]
        public string? Telefono { get; set; }

        [Column("fechaCreacion")]
        public DateTime? FechaCreacion { get; set; }

        [MaxLength(50)]
        [Column("password")]
        public string? Password { get; set; }

        [Column("estado")]
        public bool? Estado { get; set; }
    }
}
