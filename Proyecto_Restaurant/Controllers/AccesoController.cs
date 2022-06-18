using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Proyecto_Restaurant.Models;

using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Text;
using System.Security.Cryptography;

namespace Proyecto_Restaurant.Controllers
{
    public class AccesoController : Controller
    {
        
        // GET: Acceso
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(UsuarioModel user)
        {
            user.pass = ConvertirSha256(user.pass);
            using (SqlConnection cn=new SqlConnection(ConfigurationManager.ConnectionStrings["cadena"].ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("usp_ValidarUsuario", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("username", user.username);
                cmd.Parameters.AddWithValue("pass", user.pass);

                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    user.id_usuario = dr.GetInt32(0);
                    user.nom_usuario = dr.GetString(1);
                    user.ape_usuario = dr.GetString(2);
                    user.username = dr.GetString(3);
                    user.email = dr.GetString(4);
                    user.fono_user = dr.GetString(5);
                    user.id_distrito = dr.GetInt32(6);
                    user.nom_distrito = dr.GetString(7);
                }
            }
            if (user.id_usuario!=0)
            {
                Session["usuario"] = user;
                Session["nomuser"] = user.nom_usuario;
                return RedirectToAction("Index","Home") ;
            }
            else
            {
                ViewData["Mensaje"] = "usuario no encontrado";
                return View();
            }
        }

        public ActionResult CerrarSesion()
        {
            Session["usuario"] = null;
            return RedirectToAction("Login","Acceso");
        }

        public static string ConvertirSha256(string texto)
        {
            StringBuilder Sb = new StringBuilder();
            using (SHA256 hash = SHA256Managed.Create())
            {
                Encoding enc = Encoding.UTF8;
                byte[] result = hash.ComputeHash(enc.GetBytes(texto));

                foreach (byte b in result)
                    Sb.Append(b.ToString("x2"));
            }
            return Sb.ToString();
        }
    }
}