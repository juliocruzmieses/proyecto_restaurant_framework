using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Proyecto_Restaurant.Models
{
    public class DetalleCarritoModel
    {
        public string id_boleta { get; set; }
        public int id_producto { get; set; }
        public string nomproducto { get; set; }
        public int cantidad { get; set; }
        public decimal total{ get; set; }
    }
}