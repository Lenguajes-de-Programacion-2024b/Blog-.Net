using Blog_.Net.Filters;
using Blog_.Net.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Blog_.Net.Controllers
{
    public class HomeController : Controller
    {

        [Checksession]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Posts()
        {
            // Lógica para manejar la vista de publicaciones
            return View();
        }

        public ActionResult Listado()
        {
            // Lógica para manejar la vista de "Listado Me Gusta"
            return View();
        }

        public ActionResult logout()
        {
            Session["infouser"] = null;
            return RedirectToAction("Login", "Acceso");
        }

    }
}