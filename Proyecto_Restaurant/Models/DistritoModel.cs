using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Proyecto_Restaurant.Models
{
    public class DistritoModel
    {
        [Display(Name = "Código de Distrito")]
        public int idDistrito { get; set; }
        [Display(Name = "Nombre de Distrito")]
        public string nomDistrito { get; set; }
    }
}