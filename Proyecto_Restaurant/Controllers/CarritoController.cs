using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Proyecto_Restaurant.Models;
using Proyecto_Restaurant.Permisos;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Threading.Tasks;

namespace Proyecto_Restaurant.Controllers
{
    public class CarritoController : Controller
    {
        [ValidarSession]
        // GET: Carrito
        IEnumerable<ProductoModel> Productos()
        {
            List<ProductoModel> lista = new List<ProductoModel>();
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["cadena"].ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spListarProducto", cn);
                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    lista.Add(new ProductoModel()
                    {
                        id_producto = dr.GetInt32(0),
                        nom_producto = dr.GetString(1),
                        precio = dr.GetDecimal(2),
                        ruta_imagen = dr.GetString(3),
                        nombre_imagen = dr.GetString(4),
                        id_categoria = dr.GetInt32(5),
                        nom_categoria = dr.GetString(6),
                        stock = dr.GetInt32(7),
                        activo = dr.GetBoolean(8)
                    });
                }
            }
            return lista;
        }

        IEnumerable<MesaModel> Mesas()
        {
            List<MesaModel> lista = new List<MesaModel>();
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["cadena"].ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("usp_listarMesa", cn);
                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    lista.Add(new MesaModel()
                    {
                        idMesa = dr[0].ToString(),
                        descMesa = dr[1].ToString()
                    });
                }
            }
            return lista;
        }


        public async Task<ActionResult> IndexCarrito()
        {
            return View(await Task.Run(() => Mesas()));
        }
        public ActionResult SeleccionProductos()
        {
            return View();
        }
        public ActionResult AgregarCarrito()
        {
            return View();
        }
        public ActionResult FinalizarCompra()
        {
            return View();
        }
        public ActionResult Delete()
        {
            return View();
        }
    }
}