using AppMasInfo.Negocio.DAL.Entities;
using AppMasInfo.Negocio.DAL.Services;
using AppMasInfo.Utils.Utils;
using AppMasInfo.Web.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

namespace AppMasInfo.Web.Controllers
{
    [Authorize]
    public class TrabajadorController : Controller
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

        private ICargoService CargoServiceModel
        {
            get
            {
                return CargoService.GetInstance();
            }
        }

        private IRolService RolServiceModel
        {
            get
            {
                return RolService.GetInstance();
            }
        }

        private ICargoFuncionService CargoFuncionServiceModel
        {
            get
            {
                return CargoFuncionService.GetInstance();
            }
        }
        #endregion

        #region Metodos Publicos

        #region Index
        [HttpGet]
        public ActionResult Index()
        {
            TempData.Clear();

            TrabajadorViewModel viewModel = new TrabajadorViewModel();

            try
            {
                var lstRoles = this.RolServiceModel.GetListaRolAll();
                viewModel.LstRoles = lstRoles.HasValue ? lstRoles.Value : new List<RolDto>();

                var trabajadorFiltroObj = new TrabajadorDto();
                trabajadorFiltroObj.FiltroIdEstado = (int)EnumUtils.EstadoEnum.Trabajador_Habilitado;
                var trabajadorListDbResponse = this.TrabajadorServiceModel.GetListaTrabajadorbyFiltro(trabajadorFiltroObj);

                if (trabajadorListDbResponse != null)
                    viewModel.LstTrabajador = trabajadorListDbResponse;

                if (trabajadorListDbResponse == null)
                {
                    TempData["MessageError"] = "Ha ocurrido un error al obtener la lista de trabajadores. Por favor, inténtelo nuevamente";
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener una lista de trabajadores", ex);
            }

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(TrabajadorViewModel p_ViewModel)
        {
            if (ModelState.IsValid)
            {

            }
            return View(p_ViewModel);
        }
        #endregion

        #region Create
        [HttpGet]
        public ActionResult Create()
        {
            TrabajadorCreateViewModel viewModel = new TrabajadorCreateViewModel();

            try
            {
                var lstRoles = this.RolServiceModel.GetListaRolAll();
                viewModel.LstRol = lstRoles.HasValue ? lstRoles.Value : new List<RolDto>();

                var lstCargos = this.CargoServiceModel.GetListaCargoAll();
                viewModel.LstCargo = lstCargos.HasValue ? lstCargos.Value : new List<CargoDto>();

                var lstCargoFuncion = this.CargoFuncionServiceModel.GetListaCargoFuncionAll();
                viewModel.LstCargoFuncion = lstCargoFuncion.HasValue ? lstCargoFuncion.Value : new List<CargoFuncionDto>();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener listas solicitadas", ex);
            }

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TrabajadorCreateViewModel p_ViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    TrabajadorDto TrabajadorNewDb = new TrabajadorDto();
                    TrabajadorNewDb.Nombre = p_ViewModel.Nombre;
                    TrabajadorNewDb.ApellidoPaterno = p_ViewModel.ApellidoPaterno;
                    TrabajadorNewDb.ApellidoMaterno = p_ViewModel.ApellidoMaterno;
                    TrabajadorNewDb.DatosUsuario = new UsuarioDto();
                    TrabajadorNewDb.DatosUsuario.Username = p_ViewModel.DatosUsuario.Username;
                    TrabajadorNewDb.DatosUsuario.Pass = GlobalMethods.EncryptPass(p_ViewModel.DatosUsuario.Pass);
                    TrabajadorNewDb.DatosUsuario.IdRol = p_ViewModel.DatosUsuario.IdRol;
                    TrabajadorNewDb.IdCargo = p_ViewModel.IdCargo;
                    TrabajadorNewDb.idc
                    TrabajadorNewDb.UsrCreate = User.Identity.GetUserId();
                    TrabajadorNewDb.FchCreate = DateTime.Now;
                    TrabajadorNewDb.Email = p_ViewModel.Email;
                    TrabajadorNewDb.IdEstado = (int)EnumUtils.EstadoEnum.Trabajador_Habilitado;
                    TrabajadorNewDb.IdUsuario = p_ViewModel.IdUsuario;


                    var objRespuesta = this.TrabajadorServiceModel.InsertarTrabajador(TrabajadorNewDb);

                    if (!objRespuesta.HasError)
                    {
                        TempData["SaveOkMessage"] = "Trabajador ingresado correctamente";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Intente nuevamente";
                    }
                }
                else
                {
                    CargarDatosCreatePaciente(p_ViewModel);

                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al insertar usuario", ex);
            }

            return View(p_ViewModel);
        }

        private void CargarDatosCreatePaciente(TrabajadorCreateViewModel p_ViewModel)
        {
            var lstRoles = this.RolServiceModel.GetListaRolAll();
            p_ViewModel.LstRol = lstRoles.HasValue ? lstRoles.Value : new List<RolDto>();

            var lstCargos = this.CargoServiceModel.GetListaCargoAll();
            p_ViewModel.LstCargo = lstCargos.HasValue ? lstCargos.Value : new List<CargoDto>();
        }
        #endregion

        #region Edit
        [HttpGet]
        public ActionResult Edit(long p_Id)
        {
            TrabajadorEditViewModel viewModel = new TrabajadorEditViewModel();

            try
            {
                TempData.Clear();

                var lstRoles = this.RolServiceModel.GetListaRolAll();
                viewModel.LstRol = lstRoles.HasValue ? lstRoles.Value : new List<RolDto>();

                var lstCargos = this.CargoServiceModel.GetListaCargoAll();
                viewModel.LstCargo = lstCargos.HasValue ? lstCargos.Value : new List<CargoDto>();

                var filtroTrabajador = new TrabajadorDto();
                filtroTrabajador.FiltroId = p_Id;
                filtroTrabajador.FiltroIdEstado = (int)EnumUtils.EstadoEnum.Trabajador_Habilitado;

                var objResultadoDb = this.TrabajadorServiceModel.GetTrabajadorById(filtroTrabajador);

                if (objResultadoDb.HasValue)
                {
                    viewModel.Id = objResultadoDb.Value.Id;
                    viewModel.Nombre = objResultadoDb.Value.Nombre;
                    viewModel.ApellidoPaterno = objResultadoDb.Value.ApellidoPaterno;
                    viewModel.ApellidoMaterno = objResultadoDb.Value.ApellidoMaterno;
                    viewModel.Email = objResultadoDb.Value.Email;
                    viewModel.DatosUsuario = new UsuarioDto();
                    viewModel.DatosUsuario.Id = objResultadoDb.Value.DatosUsuario.Id;
                    viewModel.DatosUsuario.IdRol = objResultadoDb.Value.DatosUsuario.IdRol;
                    viewModel.DatosUsuario.Username = objResultadoDb.Value.DatosUsuario.Username;
                    viewModel.IdCargo = (int)objResultadoDb.Value.IdCargo;
                }

                if (objResultadoDb.HasError)
                    throw objResultadoDb.Error;
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Ha ocurrido un error al obtener los datos del usuario. Por favor, inténtelo nuevamente";

                return RedirectToAction("Index");
            }

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(TrabajadorEditViewModel p_ViewModel)
        {
            try
            {
                TempData.Clear();

                if (ModelState.IsValid)
                {
                    TrabajadorDto objTrabajadorEdit = new TrabajadorDto();
                    objTrabajadorEdit.Nombre = p_ViewModel.Nombre;
                    objTrabajadorEdit.IdEstado = (int)EnumUtils.EstadoEnum.Trabajador_Habilitado;
                    objTrabajadorEdit.ApellidoPaterno = p_ViewModel.ApellidoPaterno;
                    objTrabajadorEdit.ApellidoMaterno = p_ViewModel.ApellidoMaterno;
                    objTrabajadorEdit.IdCargo = p_ViewModel.IdCargo;
                    objTrabajadorEdit.UsrUpdate = User.Identity.GetUserId();
                    objTrabajadorEdit.FchUpdate = DateTime.Now;
                    objTrabajadorEdit.DatosUsuario = new UsuarioDto();
                    objTrabajadorEdit.DatosUsuario.Id = p_ViewModel.DatosUsuario.Id;
                    objTrabajadorEdit.DatosUsuario.IdRol = p_ViewModel.DatosUsuario.IdRol;
                    objTrabajadorEdit.DatosUsuario.Username = p_ViewModel.DatosUsuario.Username;
                    objTrabajadorEdit.DatosUsuario.Pass = p_ViewModel.DatosUsuario.Pass==null?"":GlobalMethods.EncryptPass(p_ViewModel.DatosUsuario.Pass);
                    objTrabajadorEdit.Email = p_ViewModel.Email;
                    objTrabajadorEdit.Id = p_ViewModel.Id;

                    var objResultadoDb = TrabajadorServiceModel.UpdateTrabajador(objTrabajadorEdit);

                    if (objResultadoDb.HasValue)
                    {
                        TempData["SaveOkMessage"] = "Trabajador actualizado correctamente";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Ha ocurrido un error al actualizar el Trabajador. Por favor, inténtelo nuevamente";
                        this.CargarDatosEditView(ref p_ViewModel);
                    }

                    if (objResultadoDb.HasError)
                        throw objResultadoDb.Error;
                }
                else
                {
                    this.CargarDatosEditView(ref p_ViewModel);
                }
            }
            catch (Exception ex)
            {
                this.CargarDatosEditView(ref p_ViewModel);
                TempData["ErrorMessage"] = "Ha ocurrido un error al actualizar el Trabajador. Por favor, inténtelo nuevamente";
            }

            return View(p_ViewModel);
        }

        private void CargarDatosEditView(ref TrabajadorEditViewModel p_ViewModel)
        {
            var lstRoles = this.RolServiceModel.GetListaRolAll();
            p_ViewModel.LstRol = lstRoles.HasValue ? lstRoles.Value : new List<RolDto>();

            var lstCargos = this.CargoServiceModel.GetListaCargoAll();
            p_ViewModel.LstCargo = lstCargos.HasValue ? lstCargos.Value : new List<CargoDto>();
        }
        #endregion
        
        ////#region Delete
        ////[HttpPost]
        ////public string Delete(UsuarioDetailViewModel p_ViewModel)
        ////{
        ////    String jsonResult = String.Empty;

        //    //try
        //    //{
        //    //    //UsuarioDto userDelete = new UsuarioDto();
        //    //    //userDelete.Id = p_ViewModel.Id;
        //    //    //userDelete.IdEstado = (int)EnumUtils.EstadoEnum.Usuario_Deshabilitado;
        //    //    //userDelete.FchUpdate = DateTime.Now;
        //    //    //userDelete.UsrUpdate = User.Identity.GetUserId();

        //    //    //User.Identity.GetUserId();

        //    //    //var objRespuesta = this.UsuarioServiceModel.Delete(userDelete);

        //    //    //if (!objRespuesta.HasError)
        //    //    //{
        //    //    //    jsonResult = JsonConvert.SerializeObject(
        //    //    //        new
        //    //    //        {
        //    //    //            status = "ok",
        //    //    //            message = "Usuario eliminado correctamente"
        //    //    //        });
        //    //    //}
        //    //    else
        //    //    {
        //    //        jsonResult = JsonConvert.SerializeObject(
        //    //                new
        //    //                {
        //    //                    status = "error",
        //    //                    message = "No puede eliminar al usuario"
        //    //                });
        //    //    }
        //    //}
        //    //catch (Exception ex)
        //    //{
        //    //    jsonResult = JsonConvert.SerializeObject(new { status = "error", message = "Error al intentar eliminar el usuario", ex });
        //    //}

        //    //return jsonResult;
        //}
        #endregion

    }
}