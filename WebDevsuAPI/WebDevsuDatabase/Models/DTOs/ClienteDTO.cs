using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace  WebDvpDatabase.Models.DTOs
{
    public class ClienteDTO
    {
        public string? Nombre { get; set; }
        public string? Genero { get; set; }
        public DateTime? FechaNacimiento { get; set; }
        public string? NumeroIdentificacion { get; set; }
        public string? Direccion { get; set; }
        public string? Telefono { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public string? Password { get; set; }
        public bool? Estado { get; set; }

    }
}
