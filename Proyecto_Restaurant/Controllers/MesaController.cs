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
    public class MesaController : Controller
    {
        [ValidarSession]
        // GET: Mesa
        IEnumerable<MesaModel> listaMesas()
        {
            List<MesaModel> lista = new List<MesaModel>();
            SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["cadena"].ConnectionString);
            SqlCommand cmd = new SqlCommand("usp_listarMesa", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            try
            {
                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    MesaModel objMesa = new MesaModel()
                    {
                        idMesa = dr[0].ToString(),
                        descMesa = dr[1].ToString()
                    };
                    lista.Add(objMesa);
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
        public async Task<ActionResult> ListadoMesas()
        {
            return View(await Task.Run(() => listaMesas()));
        }
        // Buscar Mesa
        MesaModel BuscarMesa(string id)
        {
            MesaModel reg = listaMesas().Where(c => c.idMesa == id).FirstOrDefault();
            return reg;
        }
        // Crear Mesa
        public ActionResult Create()
        {
            return View(new MesaModel());
        }
        [HttpPost]
        public ActionResult Create(MesaModel reg)
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
                    SqlCommand cmd = new SqlCommand("usp_Merge_Mesas", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@idmesa", reg.idMesa);
                    cmd.Parameters.AddWithValue("@descmesa", reg.descMesa);

                    int num = cmd.ExecuteNonQuery();
                    mensaje = $"¡Se agrego {num} mesa(s) correctamente!";
                }
                catch (SqlException ex)
                {
                    mensaje = ex.Message.ToString();
                }
                ViewBag.mensaje = mensaje;
                return View(reg);
            }
        }
        // Editar Mesa
        public ActionResult Edit(string id)
        {
            MesaModel reg = BuscarMesa(id);
            if (reg == null)
                return RedirectToAction("ListadoMesas");
            return View(reg);
        }
        [HttpPost]
        public ActionResult Edit(MesaModel reg)
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
                    SqlCommand cmd = new SqlCommand("usp_Merge_Mesas", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@idmesa", reg.idMesa);
                    cmd.Parameters.AddWithValue("@descmesa", reg.descMesa);
                    int num = cmd.ExecuteNonQuery();
                    mensaje = $"Se ha actualizado {num} mesa (s)";
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