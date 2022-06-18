using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Proyecto_Restaurant.Models
{
    public class DetalleCarritoModel
    {
        public int id_boleta { get; set; }
        public int id_producto { get; set; }
        public int cantidad { get; set; }
        public double total{ get; set; }
    }
}