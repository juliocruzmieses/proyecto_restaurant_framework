using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Proyecto_Restaurant.Models
{
    public class DetalleCarritoModel
    {
        [Display(Name = "Cod Boleta")]
        [Required]
        public string id_boleta { get; set; }
        [Display(Name = "Cod Producto")]
        [Required]
        public int id_producto { get; set; }
        [Display(Name = "Producto")]
        [Required]
        public string nomproducto { get; set; }
        [Display(Name = "Cantidad")]
        [Required]
        public int cantidad { get; set; }
        [Display(Name = "Total")]
        public decimal total{ get; set; }
    }
}