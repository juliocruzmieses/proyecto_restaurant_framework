using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Proyecto_Restaurant.Models
{
    public class MesaModel
    {
        [Display(Name = "Código de Mesa")]
        public string idMesa { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        [MinLength(6, ErrorMessage = "Debe contener minimo 6 caracteres")]

        [Display(Name = "N° de Mesa")]
        public string descMesa { get; set; }
    }
}