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
    //Describe cómo usar el atributo Authorize para controlar el acceso a las paginas y a sus metodos.
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
            TrabajadorViewModel viewModel = new TrabajadorViewModel();

            try
            {              
                var lstRoles = this.RolServiceModel.GetListaRolAll();
                viewModel.LstRoles = lstRoles.HasValue ? lstRoles.Value : new List<RolDto>();

                var lstTrabajador = this.TrabajadorServiceModel.GetListaTrabajadorAll();
                viewModel.LstTrabajador.Value = lstTrabajador.HasValue ? lstTrabajador.Value : new List<TrabajadorDto>();

                var grupoUsuarioFiltroObj = new UsuarioDto();
                grupoUsuarioFiltroObj.FiltroUsername = viewModel.FiltroUsername;
                grupoUsuarioFiltroObj.FiltroIdRol = viewModel.FiltroIdRol;
                var usuarioListDbResponse = this.UsuarioServiceModel.GetListaUsuariobyFiltro(grupoUsuarioFiltroObj);

                viewModel.LstUsuario = usuarioListDbResponse.HasValue ? usuarioListDbResponse.Value : new List<UsuarioDto>();

                if (usuarioListDbResponse.HasError)
                {
                    TempData["ErrorMessage"] = "Ha ocurrido un error al obtener la lista de Grupos de Trabajo. Por favor, inténtelo nuevamente";                    
                }

                var trabajadorFiltroObj = new TrabajadorDto(1, 10);
                trabajadorFiltroObj.FiltroIdEstado = (int)EnumUtils.EstadoEnum.Trabajador_Habilitado;
                var trabajadorListDbResponse = TrabajadorServiceModel.GetListaTrabajadorbyFiltro(trabajadorFiltroObj);

                if (!trabajadorListDbResponse.HasError)
                {
                    viewModel.LstTrabajador = trabajadorListDbResponse;
                }

                if (trabajadorListDbResponse.HasError)
                {
                    TempData["ErrorMessage"] = "Ha ocurrido un error al obtener la lista de Usuarios. Por favor, inténtelo nuevamente";
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener una lista de Usuarios", ex);
            }

            return View(viewModel);
        }

        [HttpPost]
        public string Index(TrabajadorViewModel p_ViewModel)
        {
            string jsonResult = string.Empty;

            try
            {
                var usuarioFiltroObj = new TrabajadorDto(p_ViewModel.FiltroPaginado.PaginaActual, p_ViewModel.FiltroPaginado.TamanoPagina);
                usuarioFiltroObj.FiltroUsername = p_ViewModel.FiltroUsername;               
                usuarioFiltroObj.FiltroIdRol = p_ViewModel.FiltroIdRol;           

                var usuarioListDbResponse = TrabajadorServiceModel.GetListaTrabajadorbyFiltro(usuarioFiltroObj);

                if (usuarioListDbResponse.HasValue)
                    jsonResult = JsonConvert.SerializeObject(usuarioListDbResponse);

                if (usuarioListDbResponse.HasError)
                    throw usuarioListDbResponse.Error;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener el filtro de búsqueda ", ex);
            }

            return jsonResult;
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
                viewModel.LstCRol = lstRoles.HasValue ? lstRoles.Value : new List<RolDto>();

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
                    UsuarioDto UsuarioNewDb = new UsuarioDto();
                    UsuarioNewDb.Username = p_ViewModel.Username;
                    UsuarioNewDb.Pass = GlobalMethods.EncryptPass(p_ViewModel.Pass);
                    UsuarioNewDb.IdRol = p_ViewModel.IdRol;

                    var objRespuesta = UsuarioServiceModel.InsertarUsuario(UsuarioNewDb);

                    if (!objRespuesta.HasError)
                    {
                        TempData["SaveOkMessage"] = "Usuario ingresado correctamente";
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Intente nuevamente";
                    }

                    var UsuarioFiltroObj = new UsuarioDto();
                    UsuarioFiltroObj.FiltroUsername = p_ViewModel.Username;
                    var usuarioDbResponse = UsuarioServiceModel.GetUsuarioByUsername(UsuarioFiltroObj);

                    if (!usuarioDbResponse.HasValue)
                    {
                        TrabajadorDto TrabajadorNewDb = new TrabajadorDto();
                        TrabajadorNewDb.Nombre = p_ViewModel.Nombre;
                        TrabajadorNewDb.ApellidoPaterno = p_ViewModel.ApellidoPaterno;
                        TrabajadorNewDb.ApellidoMaterno = p_ViewModel.ApellidoMaterno;
                        TrabajadorNewDb.IdCargo = p_ViewModel.IdCargo == 0 ? null : p_ViewModel.IdCargo;//si viene un valor 0 que lo cambie a null
                        TrabajadorNewDb.IdCargoFuncion = p_ViewModel.IdCargoFuncion;               //sino que envie el valor ingresado en la vista
                        TrabajadorNewDb.UsrCreate = User.Identity.GetUserId();
                        TrabajadorNewDb.FchCreate = DateTime.Now;
                        TrabajadorNewDb.Email = p_ViewModel.Email;
                        TrabajadorNewDb.IdEstado = (int)EnumUtils.EstadoEnum.Trabajador_Habilitado;
                        TrabajadorNewDb.IdUsuario = usuarioDbResponse.Value.Id;

                        var objTrabajadorResponse = this.TrabajadorServiceModel.InsertarTrabajador(TrabajadorNewDb);

                        if (!objTrabajadorResponse.HasError)
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
                        TempData["SaveOkMessage"] = "Usuario Existente";                        
                    }
                }

                CargarDatosCreatePaciente(p_ViewModel);
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
            p_ViewModel.LstCRol = lstRoles.HasValue ? lstRoles.Value : new List<RolDto>();

            var lstCargos = this.CargoServiceModel.GetListaCargoAll();
            p_ViewModel.LstCargo = lstCargos.HasValue ? lstCargos.Value : new List<CargoDto>();

            var lstCargoFuncion = this.CargoFuncionServiceModel.GetListaCargoFuncionAll();
            p_ViewModel.LstCargoFuncion = lstCargoFuncion.HasValue ? lstCargoFuncion.Value : new List<CargoFuncionDto>();
        }
        #endregion

        #region Edit
        [HttpGet]
        public ActionResult Edit(long p_Id)
        {
            TrabajadorEditViewModel viewModel = new TrabajadorEditViewModel();

            try
            {
                var lstRoles = this.RolServiceModel.GetListaRolAll();
                viewModel.LstRol = lstRoles.HasValue ? lstRoles.Value : new List<RolDto>();

                var lstCargos = this.CargoServiceModel.GetListaCargoAll();
                viewModel.LstCargo = lstCargos.HasValue ? lstCargos.Value : new List<CargoDto>();

                var lstCargoFuncion = this.CargoFuncionServiceModel.GetListaCargoFuncionAll();
                viewModel.LstCargoFuncion = lstCargoFuncion.HasValue ? lstCargoFuncion.Value : new List<CargoFuncionDto>();

                var filtroTrabajador = new TrabajadorDto();
                filtroTrabajador.FiltroId = p_Id;
                filtroTrabajador.FiltroIdEstado = (int)EnumUtils.EstadoEnum.Trabajador_Habilitado;

                var objResultadoDb = this.TrabajadorServiceModel.GetTrabajadorById(filtroTrabajador);

                if (objResultadoDb.HasValue)
                {
                    viewModel.IdTrabajador = objResultadoDb.Value.Id;
                    viewModel.Nombre = objResultadoDb.Value.Nombre;
                    viewModel.ApellidoPaterno = objResultadoDb.Value.ApellidoPaterno;
                    viewModel.ApellidoMaterno = objResultadoDb.Value.ApellidoMaterno;
                    viewModel.Email = objResultadoDb.Value.Email;
                    viewModel.DatosUsuario = new UsuarioDto();
                    viewModel.IdUsuario = objResultadoDb.Value.DatosUsuario.Id;
                    viewModel.IdRol = objResultadoDb.Value.DatosUsuario.IdRol;
                    viewModel.Username = objResultadoDb.Value.DatosUsuario.Username;
                    viewModel.IdCargo = objResultadoDb.Value.IdCargo;
                    viewModel.IdCargoFuncion = objResultadoDb.Value.IdCargoFuncion;


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
                if (ModelState.IsValid)
                {
                    UsuarioDto objUsuarioEdit = new UsuarioDto();                   
                    objUsuarioEdit.Pass = p_ViewModel.Pass == null ? "" : GlobalMethods.EncryptPass(p_ViewModel.Pass);
                    objUsuarioEdit.IdRol = p_ViewModel.IdRol;
                    objUsuarioEdit.Id = p_ViewModel.IdUsuario;

                    var objRespuesta = UsuarioServiceModel.UpdateUsuario(objUsuarioEdit);

                    if (!objRespuesta.HasError)
                    {
                        TempData["SaveOkMessage"] = "Usuario ingresado correctamente";
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Intente nuevamente";
                    }

                  
                        TrabajadorDto objTrabajadorEdit = new TrabajadorDto();
                        objTrabajadorEdit.Nombre = p_ViewModel.Nombre;
                        objTrabajadorEdit.IdEstado = (int)EnumUtils.EstadoEnum.Trabajador_Habilitado;
                        objTrabajadorEdit.ApellidoPaterno = p_ViewModel.ApellidoPaterno;
                        objTrabajadorEdit.ApellidoMaterno = p_ViewModel.ApellidoMaterno;
                        objTrabajadorEdit.IdCargo = p_ViewModel.IdCargo == 0 ? null : p_ViewModel.IdCargo; ;
                        objTrabajadorEdit.IdCargoFuncion = p_ViewModel.IdCargoFuncion;
                        objTrabajadorEdit.UsrUpdate = User.Identity.GetUserId();
                        objTrabajadorEdit.FchUpdate = DateTime.Now;                       
                        objTrabajadorEdit.Email = p_ViewModel.Email;
                        objTrabajadorEdit.Id = p_ViewModel.IdTrabajador;

                        var objResultadoDb = TrabajadorServiceModel.UpdateTrabajador(objTrabajadorEdit);

                        if (objResultadoDb.HasValue)
                        {
                            TempData["SaveOkMessage"] = "Usuario Actualizado Correctamente";
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            TempData["ErrorMessage"] = "Ha ocurrido un error al actualizar el Usuario. Por favor, inténtelo nuevamente";
                            this.CargarDatosEditView(ref p_ViewModel);
                        }

                        if (objResultadoDb.HasError)
                            throw objResultadoDb.Error;
                  
                }

                this.CargarDatosEditView(ref p_ViewModel);

            }
            catch (Exception ex)
            {
                this.CargarDatosEditView(ref p_ViewModel);
                TempData["ErrorMessage"] = "Ha ocurrido un error al actualizar el Usuario. Por favor, inténtelo nuevamente";
            }

            return View(p_ViewModel);
        }

        private void CargarDatosEditView(ref TrabajadorEditViewModel p_ViewModel)
        {
            var lstRoles = this.RolServiceModel.GetListaRolAll();
            p_ViewModel.LstRol = lstRoles.HasValue ? lstRoles.Value : new List<RolDto>();

            var lstCargos = this.CargoServiceModel.GetListaCargoAll();
            p_ViewModel.LstCargo = lstCargos.HasValue ? lstCargos.Value : new List<CargoDto>();

            var lstCargoFuncion = this.CargoFuncionServiceModel.GetListaCargoFuncionAll();
            p_ViewModel.LstCargoFuncion = lstCargoFuncion.HasValue ? lstCargoFuncion.Value : new List<CargoFuncionDto>();
        }
        #endregion

        #region Delete
        [HttpPost]
        public string Delete(TrabajadorDetailViewModel p_ViewModel)
        {
            String jsonResult = String.Empty;

            try
            {
                TrabajadorDto userDelete = new TrabajadorDto();
                userDelete.Id = p_ViewModel.Id;
                userDelete.IdEstado = (int)EnumUtils.EstadoEnum.Trabajador_Deshabilitado;
                userDelete.FchUpdate = DateTime.Now;
                userDelete.UsrUpdate = User.Identity.GetUserId();

                User.Identity.GetUserId();

                var objRespuesta = this.TrabajadorServiceModel.Delete(userDelete);

                if (!objRespuesta.HasError)
                {
                    jsonResult = JsonConvert.SerializeObject(
                        new
                        {
                            status = "ok",
                            message = "Usuario eliminado correctamente"
                        });
                }
                else
                {
                    jsonResult = JsonConvert.SerializeObject(
                            new
                            {
                                status = "error",
                                message = "No puede eliminar al usuario"
                            });
                }
            }
            catch (Exception ex)
            {
                jsonResult = JsonConvert.SerializeObject(new { status = "error", message = "Error al intentar eliminar el usuario", ex });
            }

            return jsonResult;
        }
        #endregion

        #region Detail
        [HttpGet]
        public ActionResult Detail(long p_Id)
        {
            TrabajadorDetailViewModel viewModel = new TrabajadorDetailViewModel();

            try
            {
                TrabajadorDto filtroTrabajador = new TrabajadorDto();
                filtroTrabajador.FiltroId = p_Id;

                var objRespuestaDb = TrabajadorServiceModel.GetTrabajadorById(filtroTrabajador);

                if (objRespuestaDb.HasValue)
                {
                    viewModel = new TrabajadorDetailViewModel
                    {
                        DetalleEstado = objRespuestaDb.Value.DetalleEstado,
                        FchCreate = objRespuestaDb.Value.FchCreate,
                        FchUpdate = objRespuestaDb.Value.FchUpdate,
                        Id = objRespuestaDb.Value.Id,
                        Email = objRespuestaDb.Value.Email,
                        IdEstado = objRespuestaDb.Value.IdEstado,
                        Nombre = objRespuestaDb.Value.Nombre,
                        ApellidoPaterno = objRespuestaDb.Value.ApellidoPaterno,
                        ApellidoMaterno = objRespuestaDb.Value.ApellidoMaterno,
                        UsrUpdate = objRespuestaDb.Value.UsrUpdate,
                        UsrCreate = objRespuestaDb.Value.UsrCreate,
                        DatosUsuario = objRespuestaDb.Value.DatosUsuario,
                        DetalleRol = objRespuestaDb.Value.DetalleRol,
                        DetalleCargo = objRespuestaDb.Value.DetalleCargo,
                        DetalleFuncion = objRespuestaDb.Value.DetalleFuncion
                    };
                }

                if (objRespuestaDb.HasError)
                    throw objRespuestaDb.Error;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return View(viewModel);
        }
        #endregion

        #endregion
    }
}