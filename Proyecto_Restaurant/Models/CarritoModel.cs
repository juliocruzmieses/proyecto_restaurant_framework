using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Proyecto_Restaurant.Models
{
    public class CarritoModel
    {
        [Display(Name = "Cod Boleta")]
        public string id_boleta { get; set; }
        [Display(Name = "Código de usuario")]
        public int id_usuario { get; set; }
        [Display(Name = "Nombre de usuario")]
        public string nom_usuario { get; set; }
        [Display(Name = "Codigo de Mesa")]
        public int id_mesa { get; set; }
        [Display(Name = "Mesa")]
        public string descr_mesa { get; set; }
        [Display(Name = "Monto total")]
        public decimal monto_total { get; set; }
        [Display(Name = "Fecha de compra")]
        public DateTime fecha_compra{ get; set; }
        [Display(Name = "Estado")]
        public int estado { get; set; }
    }
}