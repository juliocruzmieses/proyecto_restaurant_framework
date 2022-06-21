using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Proyecto_Restaurant.Models
{
    public class CarritoModel
    {
        public string id_boleta { get; set; }
        public int id_usuario { get; set; }
        public int id_mesa { get; set; }
        public decimal monto_total { get; set; }
        public DateTime fecha_compra{ get; set; }
        public int estado { get; set; }
    }
}