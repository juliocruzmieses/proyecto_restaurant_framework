using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Proyecto_Restaurant.Models
{
    public class ProductoModel
    {
		[Display(Name = "Código de Producto")]
		public int id_producto { get; set; }

		[Required(ErrorMessage = "Este campo es obligatorio")]
		[MaxLength(30, ErrorMessage = "Categoria no puede tener contener mas de 30 caracteres")]
		[MinLength(3, ErrorMessage = "Categoria no puede contener menos de 3 caracteres")]

		[Display(Name = "Nombre de Producto")]
		public string nom_producto { get; set; }

		[Required(ErrorMessage = "Este campo es obligatorio")]
		[Range(0, 999.99, ErrorMessage = "Ingresa Precio Correcto")]
		[Display(Name = "Precio")]
		public decimal precio { get; set; }
		[Display(Name = "Ruta de img")]
		public string ruta_imagen { get; set; }
		[Display(Name = "Nombre de img")]
		public string nombre_imagen { get; set; }
		[Display(Name = "Categoria")]
		public string nom_categoria { get; set; }
		public int id_categoria { get; set; }
		[Display(Name = "Stock")]
		public int stock { get; set; }
		[Display(Name = "Estado")]
		public Boolean activo { get; set; }
	}
}