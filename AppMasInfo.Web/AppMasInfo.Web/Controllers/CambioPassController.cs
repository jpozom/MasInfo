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
    [Authorize(Roles = "Administrador, Médico, Enfermero/a, Técnico, Auxiliar, Tutor")]
    public class CambioPassController : Controller
    {
        #region propiedades privadas
        private ITrabajadorService TrabajadorServiceModel
        {
            get
            {
                return TrabajadorService.GetInstance();
            }
        }

        private IUsuarioService UsuarioServiceModel
        {
            get
            {
                return UsuarioService.GetInstance();
            }
        }

        private ITutorService TutorServiceModel
        {
            get
            {
                return TutorService.GetInstance();
            }
        }

        private IRolService RolServiceModel
        {
            get
            {
                return RolService.GetInstance();
            }
        }

        #endregion

        #region Index
        // GET: CambioPass
        public ActionResult Index()
        {
            CambioPassViewModel viewModel = new CambioPassViewModel();

            try
            {
                var RolActual = (((System.Security.Claims.ClaimsIdentity)User.Identity).Claims.Where(c => c.Type == System.Security.Claims.ClaimTypes.Role).Select(c => c.Value).FirstOrDefault());

                if (RolActual == "Médico" || RolActual == "Enfermero/a" || RolActual == "Técnico" || RolActual == "Auxiliar")
                {
                    var usuarioFiltroObj = new UsuarioDto();
                    var usr = User.Identity.GetUserId();
                    usuarioFiltroObj.FiltroUsername = usr;
                    var usuarioDbResponse = UsuarioServiceModel.GetUsuarioByUsername(usuarioFiltroObj);

                    if (usuarioDbResponse.HasValue)
                    {
                        var trabajadorFiltro = new TrabajadorDto();
                        trabajadorFiltro.FiltroIdUsuario = usuarioDbResponse.Value.Id;
                        trabajadorFiltro.FiltroIdEstado = (int)EnumUtils.EstadoEnum.Trabajador_Habilitado;

                        var trabajadorDB = this.TrabajadorServiceModel.GetTrabajadorByUsuarioId(trabajadorFiltro);

                        if (trabajadorDB.HasValue)
                        {
                            viewModel.Username = usuarioDbResponse.Value.Username;
                            viewModel.NombreTrabajador = trabajadorDB.Value.Nombre;
                            viewModel.ApellidoPaternoTrabajador = trabajadorDB.Value.ApellidoPaterno;
                            viewModel.ApellidoMaternoTrabajador = trabajadorDB.Value.ApellidoMaterno;
                            viewModel.DetalleRol = trabajadorDB.Value.DetalleRol;
                            viewModel.IdUsuario = usuarioDbResponse.Value.Id;
                            viewModel.IdTrabajador = trabajadorDB.Value.Id;
                            viewModel.IdRol = usuarioDbResponse.Value.IdRol;
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
                else
                {
                    var usuarioFiltroObj = new UsuarioDto();
                    var usr = User.Identity.GetUserId();
                    usuarioFiltroObj.FiltroUsername = usr;
                    var usuarioDbResponse = UsuarioServiceModel.GetUsuarioByUsername(usuarioFiltroObj);

                    if (usuarioDbResponse.HasValue)
                    {
                        var tutorFiltro = new TutorDto();
                        tutorFiltro.FiltroIdUsuario = usuarioDbResponse.Value.Id;
                        tutorFiltro.FiltroIdEstado = (int)EnumUtils.EstadoEnum.Tutor_Habilitado;

                        var tutorDB = this.TutorServiceModel.GetTutorByUsuarioId(tutorFiltro);

                        if (tutorDB.HasValue)
                        {
                            viewModel.Username = usuarioDbResponse.Value.Username;
                            viewModel.NombreTrabajador = tutorDB.Value.Nombre;
                            viewModel.ApellidoPaternoTrabajador = tutorDB.Value.ApellidoPaterno;
                            viewModel.ApellidoMaternoTrabajador = tutorDB.Value.ApellidoMaterno;
                            viewModel.DetalleRol = tutorDB.Value.DetalleRol;
                            viewModel.IdUsuario = usuarioDbResponse.Value.Id;
                            viewModel.IdTrabajador = tutorDB.Value.Id;
                            viewModel.IdRol = usuarioDbResponse.Value.IdRol;
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
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Ha ocurrido un error al obtener registros de DB. Por favor, inténtelo nuevamente" + ex;
            }

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(CambioPassViewModel p_ViewModel)
        {
            try
            {
                UsuarioDto objUsuarioEdit = new UsuarioDto();
                objUsuarioEdit.Pass = p_ViewModel.Pass == null ? "" : GlobalMethods.EncryptPass(p_ViewModel.Pass);
                objUsuarioEdit.IdRol = p_ViewModel.IdRol;
                objUsuarioEdit.Id = p_ViewModel.IdUsuario;

                var objRespuesta = UsuarioServiceModel.UpdateUsuario(objUsuarioEdit);

                var RolActual = (((System.Security.Claims.ClaimsIdentity)User.Identity).Claims.Where(c => c.Type == System.Security.Claims.ClaimTypes.Role).Select(c => c.Value).FirstOrDefault());

                if (RolActual == "Médico" || RolActual == "Enfermero/a" || RolActual == "Técnico" || RolActual == "Auxiliar")
                {
                    if (!objRespuesta.HasError)
                    {
                        TempData["SaveOkMessage"] = "Contraseña Actualizada Correctamente";
                        return RedirectToAction("Index", "SeleccionPaciente");
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Intente nuevamente";
                    }
                }
                else
                {
                    if (!objRespuesta.HasError)
                    {
                        TempData["SaveOkMessage"] = "Contraseña Actualizada Correctamente";
                        return RedirectToAction("Index", "PacienteDatos");
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Intente nuevamente";
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Ha ocurrido un error al actualizar el Trabajador. Por favor, inténtelo nuevamente" + ex;
            }

            return View(p_ViewModel);
        }
        #endregion
    }
}
