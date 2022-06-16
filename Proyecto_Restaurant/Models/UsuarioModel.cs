using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Proyecto_Restaurant.Models
{
    public class UsuarioModel
    {
        [Display(Name = "Código de Usuario")]
        public int id_usuario { get; set; }
        [Display(Name = "Nombre de Usuario")]
        public string nom_usuario { get; set; }
        [Display(Name = "Apellido de Usuario")]
        public string ape_usuario { get; set; }
        [Display(Name = "Username")]
        public string username { get; set; }
        public string pass { get; set; }
        [Display(Name = "E-mail")]
        public string email { get; set; }
        [Display(Name = "Telefono")]
        public string fono_user { get; set; }
        [Display(Name = "Rol")]
        public int id_rol { get; set; }
        [Display(Name = "Rol")]
        public string nom_rol { get; set; }
        [Display(Name = "Distrito")]
        public int id_distrito { get; set; }
        [Display(Name = "Distrito")]
        public string nom_distrito { get; set; }
        public int estado { get; set; }
    }
}