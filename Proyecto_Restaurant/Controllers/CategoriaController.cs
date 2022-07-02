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
    [ValidarSession(RolPermiso.Administrador)]
    [Authorize]
    public class CategoriaController : Controller
    {
        // GET: Categoria
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
        public async Task<ActionResult> ListadoCategorias()
        {
            return View(await Task.Run(() => listaCategorias()));
        }
        // Buscar Categoria
        CategoriaModel BuscarCategoria(string id)
        {
            CategoriaModel reg = listaCategorias().Where(c => c.idCategoria == id).FirstOrDefault();
            return reg;
        }
        // Crear Categoria
        public ActionResult Create()
        {
            return View(new CategoriaModel());
        }
        [HttpPost]
        public ActionResult Create(CategoriaModel reg)
        {
            if (!ModelState.IsValid)
            {
                return View(reg);
            }
            string mensaje = string.Empty;
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["cadena"].ConnectionString))
            {
                try
                {
                    cn.Open();
                    SqlCommand cmd = new SqlCommand("usp_Merge_Categoria", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@idcategoria", reg.idCategoria);
                    cmd.Parameters.AddWithValue("@nomcategoria", reg.nomCategoria);

                    int num = cmd.ExecuteNonQuery();
                    mensaje = $"¡Se agrego {num} categoria(s) correctamente!";
                }
                catch (SqlException ex)
                {
                    mensaje = ex.Message.ToString();
                }
                ViewBag.mensaje = mensaje;
                return View(reg);
            }
        }
        // Editar Categoria
        public ActionResult Edit(string id)
        {
            CategoriaModel reg = BuscarCategoria(id);
            if (reg == null)
                return RedirectToAction("ListadoCategorias");
            return View(reg);
        }
        [HttpPost]
        public ActionResult Edit(CategoriaModel reg)
        {
            if (!ModelState.IsValid)
            {
                return View(reg);
            }
            string mensaje = string.Empty;
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["cadena"].ConnectionString))
            {
                try
                {
                    cn.Open();
                    SqlCommand cmd = new SqlCommand("usp_Merge_Categoria", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@idcategoria", reg.idCategoria);
                    cmd.Parameters.AddWithValue("@nomcategoria", reg.nomCategoria);
                    int num = cmd.ExecuteNonQuery();
                    mensaje = $"Se ha actualizado {num} categoria (s)";
                }
                catch (SqlException ex)
                {
                    mensaje = ex.Message.ToString();
                }
                ViewBag.mensaje = mensaje;
                return View(reg);
            }
        }
    }
}