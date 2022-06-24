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
        IEnumerable<DetalleCarritoModel> List_Detalle(int idmesa,string numboleta)
        {
            List<DetalleCarritoModel> lista = new List<DetalleCarritoModel>();
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["cadena"].ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_list_detalle_boleta", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idmesa", idmesa);
                cmd.Parameters.AddWithValue("@numboleta", numboleta);
                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    lista.Add(new DetalleCarritoModel()
                    {
                        id_boleta = dr.GetString(0),
                        id_producto = dr.GetInt32(1),
                        nomproducto = dr.GetString(2),
                        cantidad = dr.GetInt32(3),
                        total= dr.GetDecimal(4)
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
        IEnumerable<CarritoModel>ListCarrito(int id)
        {
            List<CarritoModel> lista = new List<CarritoModel>();
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["cadena"].ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_listar_boleta", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@estado", id);
                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    lista.Add(new CarritoModel()
                    {
                        id_boleta = dr.GetString(0),
                        nom_usuario = dr.GetString(1),
                        descr_mesa = dr.GetString(2),
                        monto_total = dr.GetDecimal(3),
                        fecha_compra = dr.GetDateTime(4)
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
                Session["boleta"] = null;
                Session["mensaje"] = null;
                Session["idmesa"] = null;
                CarritoModel boleta = new CarritoModel();
                SqlCommand cmd = new SqlCommand("sp_valida_pedido_en_mesa", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idmesa", idmesa);
                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                int cant = 0;
                while (dr.Read())
                {
                    boleta.id_boleta = dr.GetString(0);
                    boleta.id_usuario = dr.GetInt32(1);
                    boleta.id_mesa = dr.GetInt32(2);
                    boleta.estado = dr.GetInt32(3);
                    cant++;
                }
                if (cant != 0)
                {
                    Session["idmesa"] = idmesa;
                    Session["boleta"] = boleta;
                    return RedirectToAction("Resumen", "Carrito");
                }
                else
                {
                    Session["idmesa"] = idmesa;
                    return RedirectToAction("CrearBoleta", "Carrito");
                }
            }
                
        }
        public ActionResult BuscarBoleta(string codboleta)
        {
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["cadena"].ConnectionString))
            {
                CarritoModel carrito = new CarritoModel();
                SqlCommand cmd = new SqlCommand("sp_buscar_buscar_boleta", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@codboleta", codboleta);
                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    carrito.id_boleta = dr.GetString(0);
                    carrito.nom_usuario = dr.GetString(1);
                    carrito.descr_mesa = dr.GetString(2);
                    carrito.id_mesa = dr.GetInt32(3);
                    carrito.monto_total = dr.GetDecimal(4);
                    carrito.fecha_compra = dr.GetDateTime(5);
                    carrito.estado = dr.GetInt32(6);
                }
                Session["boleta"] = carrito;
            }
            return RedirectToAction("FinalizarCompra");
        }
        public async Task<ActionResult> Resumen()
        {
            CarritoModel boleta=(CarritoModel)Session["boleta"];
            int mesa = (int)Session["idmesa"];
            ViewBag.numboleta = boleta.id_boleta;
            ViewBag.mesas = new SelectList(await Task.Run(() => Mesas()), "idMesa", "descMesa", mesa);
            ViewBag.productos = new SelectList(await Task.Run(() => Productos()), "id_producto", "nom_producto");

            return View();
        }
        public JsonResult ResumenPedido(int idmesa)
        {
            CarritoModel boleta = (CarritoModel)Session["boleta"];
            List<DetalleCarritoModel>carrito = (List < DetalleCarritoModel >)List_Detalle(idmesa,boleta.id_boleta);
            return Json(carrito,JsonRequestBehavior.AllowGet);
        }
        public ActionResult CrearBoleta()
        {
            UsuarioModel user = (UsuarioModel)Session["usuario"];
            int idmesa = (int)Session["idmesa"];

            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["cadena"].ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_create_boleta", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idusuario", user.id_usuario);
                cmd.Parameters.AddWithValue("@idmesa", idmesa);
                cn.Open();
                cmd.ExecuteNonQuery();
            }

            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["cadena"].ConnectionString))
            {
                CarritoModel boleta = new CarritoModel();
                SqlCommand cmd = new SqlCommand("sp_devuelve_ultima_boleta", cn);
                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    boleta.id_boleta = dr.GetString(0);
                    boleta.id_usuario = dr.GetInt32(1);
                    boleta.id_mesa = dr.GetInt32(2);
                    boleta.monto_total = dr.GetDecimal(3);
                    boleta.fecha_compra = dr.GetDateTime(4);
                    boleta.estado = dr.GetInt32(5);
                }
                Session["boleta"] = boleta;
            }
            return RedirectToAction("Resumen","Carrito");
        }
        public ActionResult AgregarCarrito(DetalleCarritoModel detallecarrito)
        {
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["cadena"].ConnectionString))
            {
                CarritoModel boleta = (CarritoModel)Session["boleta"];

                SqlCommand cmd = new SqlCommand("sp_add_product_on_detailboleta", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idboleta", boleta.id_boleta);
                cmd.Parameters.AddWithValue("idproducto", detallecarrito.id_producto);
                cmd.Parameters.AddWithValue("cant", detallecarrito.cantidad);
                cn.Open();
                cmd.ExecuteNonQuery();
            }
            return RedirectToAction("Resumen");
        }
        public async Task<ActionResult> ListaPedidos()
        {
            int estado;
            if (TempData["estado"]== null)
            {
                estado = 2;
            }
            else
            {
                estado = (int)TempData["estado"];
            }
            return View(await Task.Run(() => ListCarrito(estado)));
        }
        [HttpPost]
        public ActionResult ListadoPedidos(int estado)
        {
            TempData["estado"] = estado;
            return RedirectToAction("ListaPedidos");
        }
        public ActionResult Delete(int id)
        {
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["cadena"].ConnectionString))
            {
                CarritoModel boleta = (CarritoModel)Session["boleta"];

                SqlCommand cmd = new SqlCommand("sp_delete_product_on_detailboleta", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idboleta", boleta.id_boleta);
                cmd.Parameters.AddWithValue("@idproducto", id);
                cn.Open();
                cmd.ExecuteNonQuery();
            }
            return RedirectToAction("Resumen", "Carrito");
        }
        public ActionResult FinalizarPedido(int Nestate,decimal montototal)
        {
            string mensaje = "";
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["cadena"].ConnectionString))
            {
                CarritoModel boleta = (CarritoModel)Session["boleta"];
                SqlCommand cmd = new SqlCommand("sp_actualizar_estado_boleta", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idboleta", boleta.id_boleta);
                cmd.Parameters.AddWithValue("@estado_nuevo", Nestate);
                cmd.Parameters.AddWithValue("@monttotal", montototal);
                cn.Open();
                cmd.ExecuteNonQuery();
                if (Nestate==2)
                {
                    mensaje = "Acercarse a facturacion a para finalizar la compra";
                }
                else
                {
                    mensaje = "Gracias por su compra, vuelva pronto";
                }
            }
            ViewBag.mensaje = mensaje;
            return View();
        }
        public ActionResult FinalizarCompra()
        {
            CarritoModel carrito = (CarritoModel)Session["boleta"];
            ViewBag.idboleta = carrito.id_boleta;
            ViewBag.mozo = carrito.nom_usuario;
            ViewBag.fechapedido = carrito.fecha_compra;
            ViewBag.mesa = carrito.descr_mesa;
            ViewBag.idmesa = carrito.id_mesa;
            ViewBag.montototal = carrito.monto_total;
            return View();
        }
    }
}