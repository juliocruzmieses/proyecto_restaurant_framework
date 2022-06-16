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
        [Display(Name = "Nombre de Categoria")]
        public string nomCategoria { get; set; }
    }
}