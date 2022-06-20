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
    [ValidarSession]
    public class CarritoController : Controller
    {
        IEnumerable<DetalleCarritoModel> List_Detalle(int idmesa)
        {
            List<DetalleCarritoModel> lista = new List<DetalleCarritoModel>();
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["cadena"].ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_list_detalle_boleta", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idmesa", idmesa);
                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    lista.Add(new DetalleCarritoModel()
                    {
                        id_boleta = dr.GetString(0),
                        id_producto = dr.GetInt32(1),
                        cantidad = dr.GetInt32(2),
                        total= dr.GetDecimal(3)
                    });
                }
            }
            return lista;
        }
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
        public ActionResult Valida_Pedido(int idmesa)
        {
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["cadena"].ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_valida_pedido_en_mesa", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idmesa", idmesa);//definir donde colocar el id en la mesa para capturarlo
                cn.Open();
                int validando = Convert.ToInt32(cmd.ExecuteScalar().ToString());
                if (validando !=0)
                {
                    return RedirectToAction("Resumen", "Carrito");
                }
                else
                {
                    return RedirectToAction("IndexCarrito", "Carrito");
                }
            }
                
        }
        public ActionResult Resumen()
        {
            return View();
        }
        public JsonResult ResumenPedido(int idmesa)
        {
            List<DetalleCarritoModel>carrito = (List < DetalleCarritoModel >)List_Detalle(idmesa);
            return Json(carrito,JsonRequestBehavior.AllowGet);
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