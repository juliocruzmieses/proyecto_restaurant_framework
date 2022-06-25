using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Proyecto_Restaurant.Models;

namespace Proyecto_Restaurant.Permisos
{
    public class ValidarSessionAttribute : ActionFilterAttribute
    {
        private RolPermiso id_rol;

        public ValidarSessionAttribute(RolPermiso _id_rol)
        {
            id_rol = _id_rol;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {

            if (HttpContext.Current.Session["usuario"] != null)
            {
                UsuarioModel usuario = HttpContext.Current.Session["usuario"] as UsuarioModel;

                if (usuario.id_rolPermiso != this.id_rol)
                {
                    filterContext.Result = new RedirectResult("~/Home/SinPermiso");
                }

            }

            base.OnActionExecuting(filterContext);
        }

    }
}