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
    public class ProductoController : Controller
    {
        // GET: Producto
        IEnumerable<CategoriaModel> listaCategorias()
        {
            List<CategoriaModel> lista = new List<CategoriaModel>();
            SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["cadena"].ConnectionString);
            SqlCommand cmd = new SqlCommand("usp_listarCategoria", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            try
            {
                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    CategoriaModel objCategoria = new CategoriaModel()
                    {
                        idCategoria = dr[0].ToString(),
                        nomCategoria = dr[1].ToString()
                    };
                    lista.Add(objCategoria);
                }
                dr.Close();
                cn.Close();
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            return lista;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        ///
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

        public async Task<ActionResult> ListadoProductos()
        {
            return View(await Task.Run(() => Productos()));
        }

        ProductoModel BuscarProductos(int id)
        {
            ProductoModel reg = Productos().Where(c => c.id_producto == id).FirstOrDefault();
            return reg;
        }

        public async Task<ActionResult> Create()
        {
            ViewBag.categorias = new SelectList(await Task.Run(() => listaCategorias()), "idCategoria", "nomCategoria");
            return View(new ProductoModel());
        }

        [HttpPost]

        public async Task<ActionResult> Create(ProductoModel reg)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.categorias = new SelectList(await Task.Run(() => listaCategorias()), "idCategoria", "nomCategoria", reg.id_categoria);
                return View(reg);
            }
            string mensaje = string.Empty;
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["cadena"].ConnectionString))
            {
                try
                {
                    cn.Open();
                    SqlCommand cmd = new SqlCommand("SPRegistrarProducto", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@nomproducto", reg.nom_producto);
                    cmd.Parameters.AddWithValue("@precio", reg.precio);
                    cmd.Parameters.AddWithValue("@rutaimag", reg.ruta_imagen);
                    cmd.Parameters.AddWithValue("@nomimag", reg.nombre_imagen);
                    cmd.Parameters.AddWithValue("@idcategoria", reg.id_categoria);
                    cmd.Parameters.AddWithValue("@stock", reg.stock);
                    cmd.Parameters.AddWithValue("@activo", reg.activo);
                    int num = cmd.ExecuteNonQuery();
                    mensaje = $"Se ha insertado {num} producto (s)";
                }
                catch (SqlException ex)
                {
                    mensaje = ex.Message.ToString();
                }
                ViewBag.mensaje = mensaje;
                ViewBag.categorias = new SelectList(await Task.Run(() => listaCategorias()), "idCategoria", "nomCategoria", reg.id_categoria);
                return View(reg);
            }
        }

        public async Task<ActionResult> Edit(int id)
        {
            ProductoModel reg = BuscarProductos(id);
            if (reg == null)
                return RedirectToAction("ListadoProductos");
            ViewBag.categorias = new SelectList(await Task.Run(() => listaCategorias()), "idCategoria", "nomCategoria", reg.id_categoria);
            return View(reg);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(ProductoModel reg)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.categorias = new SelectList(await Task.Run(() => listaCategorias()), "idCategoria", "nomCategoria", reg.id_categoria);
                return View(reg);
            }
            string mensaje = string.Empty;
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["cadena"].ConnectionString))
            {
                try
                {
                    cn.Open();
                    SqlCommand cmd = new SqlCommand("spactualizarproducto", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@idproducto", reg.id_producto);
                    cmd.Parameters.AddWithValue("@nomproducto", reg.nom_producto);
                    cmd.Parameters.AddWithValue("@precio", reg.precio);
                    cmd.Parameters.AddWithValue("@rutaimag", reg.ruta_imagen);
                    cmd.Parameters.AddWithValue("@nomimag", reg.nombre_imagen);
                    cmd.Parameters.AddWithValue("@idcategoria", reg.id_categoria);
                    cmd.Parameters.AddWithValue("@stock", reg.stock);
                    cmd.Parameters.AddWithValue("@activo", reg.activo);
                    int num = cmd.ExecuteNonQuery();
                    mensaje = $"Se ha actualizado {num} producto (s)";
                }
                catch (SqlException ex)
                {
                    mensaje = ex.Message.ToString();
                }
                ViewBag.mensaje = mensaje;
                ViewBag.categorias = new SelectList(await Task.Run(() => listaCategorias()), "idCategoria", "nomCategoria", reg.id_categoria);
                return View(reg);
            }
        }
    }
}