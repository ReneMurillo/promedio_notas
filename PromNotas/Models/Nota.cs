using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PromNotas.Models
{
    public class Nota
    {
        [Key]
        public int NotaID { get; set; }

        [Display(Name = "Nota 1")]
        [Required(ErrorMessage = "Este campo es obligatorio")]
        public decimal Nota1 { get; set; }

        [Display(Name = "Nota 2")]
        [Required(ErrorMessage = "Este campo es obligatorio")]
        public decimal Nota2 { get; set; }

        [Display(Name = "Nota 3")]
        [Required(ErrorMessage = "Este campo es obligatorio")]
        public decimal Nota3 { get; set; }

        public decimal Promedio { get; set; }

        public string Estado { get; set; }
    }
}