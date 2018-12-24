using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static AppMasInfo.Web.Models.IdentityModels;

namespace AppMasInfo.Web.Controllers
{
    //Describe cómo usar el atributo Authorize para controlar el acceso a las paginas y a sus metodos.
    [Authorize(Roles = "Administrador, Médico, Enfermero/a")]
    public class HomeController : Controller
    {      
        public ActionResult Index()
        {            
            return View();
        }

        public ActionResult Usuario()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}