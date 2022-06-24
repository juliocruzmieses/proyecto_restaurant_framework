using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;

namespace Proyecto_Restaurant.Models
{
    public class CategoriaModel
    {
        [Display(Name = "Código de Categoria")]
        public string idCategoria { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        [MaxLength(100, ErrorMessage = "Categoria nombre no puede tener contener mas de 100 caracteres")]
        [MinLength(3, ErrorMessage = "Categoria nombre no puede contener menos de 3 caracteres")]



        [Display(Name = "Nombre de Categoria")]
        public string nomCategoria { get; set; }
    }
}