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
        [Required(AllowEmptyStrings = false, ErrorMessage = "El nombre es obligatorio.")]
        [MinLength(3, ErrorMessage = "El nombre no debe contener menos de 3 caracteres.")]
        [MaxLength(50, ErrorMessage = "El nombre no debe contener más de 50 caracteres.")]

        public string nom_usuario { get; set; }
        [Display(Name = "Apellido de Usuario")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "El apellido es obligatorio.")]
        [MinLength(3, ErrorMessage = "El apellido no debe contener menos de 3 caracteres.")]
        [MaxLength(50, ErrorMessage = "El apellido no debe contener más de 50 caracteres.")]

        public string ape_usuario { get; set; }
        [Display(Name = "Username")]
        [Required]
        public string username { get; set; }
       
        [Required]
        public string pass { get; set; }

        [Display(Name = "E-mail")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "El email es requerido.")]
        [RegularExpression(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$", ErrorMessage
          = "Formato incorrecto, coloca un correo valido.")]

        public string email { get; set; }

        [Display(Name = "Telefono")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "El telefono es requerido.")]
       [RegularExpression("([0-9]+$)",ErrorMessage ="Solo se perimte números")]
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