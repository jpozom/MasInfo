using AppMasInfo.Negocio.DAL.Entities;
using AppMasInfo.Negocio.DAL.Services;
using AppMasInfo.Utils.Utils;
using AppMasInfo.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AppMasInfo.Web.Controllers
{
    public class PacienteDatosController : Controller
    {
        #region propiedades privadas
        private IPacienteService PacienteServiceModel
        {
            get
            {
                return PacienteService.GetInstance();
            }
        }

        private IUsuarioService UsuarioServiceModel
        {
            get
            {
                return UsuarioService.GetInstance();
            }
        }

        private IRolService RolServiceModel
        {
            get
            {
                return RolService.GetInstance();
            }
        }

        private ITutorService TutorServiceModel
        {
            get
            {
                return TutorService.GetInstance();
            }
        }

        private ITelefonoService TelefonoServiceModel
        {
            get
            {
                return TelefonoService.GetInstance();
            }
        }

        private ITipoTelefonoService TIpoTelefonoServiceModel
        {
            get
            {
                return TipoTelefonoService.GetInstance();
            }
        }
        #endregion

        // GET: TutorDatosPaciente
        public ActionResult Index()
        {           

            return View();
        }

        
    }
}