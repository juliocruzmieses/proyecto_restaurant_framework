using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Proyecto_Restaurant.Models
{
    public class RolModel
    {
        [Display(Name = "Código de Rol")]
        public int idRol { get; set; }
        [Display(Name = "Nombre de Rol")]
        public string nomRol { get; set; }
    }
}