using AppMasInfo.Negocio.DAL.Entities;
using AppMasInfo.Negocio.DAL.Services;
using AppMasInfo.Utils.Utils;
using AppMasInfo.Web.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AppMasInfo.Web.Controllers
{
    [Authorize(Roles = "Administrador, Tutor")]
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
            PacienteDatosViewModel viewModel = new PacienteDatosViewModel();

            try
            {
                var usuarioFiltroObj = new UsuarioDto();
                var usr = User.Identity.GetUserId();
                usuarioFiltroObj.FiltroUsername = usr;
                var usuarioDbResponse = UsuarioServiceModel.GetUsuarioByUsername(usuarioFiltroObj);

                if (usuarioDbResponse.HasValue)
                {
                    var tutorFiltro = new TutorDto();
                    tutorFiltro.FiltroIdUsuario = usuarioDbResponse.Value.Id;

                    var tutorDB = this.TutorServiceModel.GetTutorByUsuarioId(tutorFiltro);

                    if (tutorDB.HasValue)
                    {
                        viewModel.Username = usuarioDbResponse.Value.Username;                        
                        viewModel.NombreTutor = tutorDB.Value.Nombre;                      
                        viewModel.DireccionTutor = tutorDB.Value.Direccion;
                        viewModel.NombreTutor = tutorDB.Value.Nombre;
                        viewModel.RutTutor = tutorDB.Value.Rut;
                        viewModel.ApellidoPaternoTutor = tutorDB.Value.ApellidoPaterno;
                        viewModel.ApellidoMaternoTutor = tutorDB.Value.ApellidoMaterno;                       
                        viewModel.NombreTutor = tutorDB.Value.Nombre;
                        viewModel.ApellidoPaternoTutor = tutorDB.Value.ApellidoPaterno;
                        viewModel.DetalleRol = tutorDB.Value.DetalleRol;
                        viewModel.DetalleEstado = tutorDB.Value.DetalleEstado;
                        viewModel.DetallePaciente = tutorDB.Value.DetallePaciente;
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Ha ocurrido un Error al Obtener Registros de Pacientes en DB. Por favor, Inténtelo Nuevamente";
                    }
                }
                else
                {
                    TempData["ErrorMessage"] = "Ha ocurrido un Error al Obtener Registros del Usuario en Sesión. Por favor, Inténtelo Nuevamente";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Ha ocurrido un error al obtener registros de DB. Por favor, inténtelo nuevamente";
            }

            return View(viewModel);

        }
    }
}