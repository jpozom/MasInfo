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
    public class EquipoPacienteController : Controller
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

        private IEquipoPacienteService EquipoPacienteServiceModel
        {
            get
            {
                return EquipoPacienteService.GetInstance();
            }
        }

        #endregion

        #region metodos publicos

        #region Index
        // GET: Paciente
        public ActionResult Index()
        {
            PacienteViewModel viewModel = new PacienteViewModel();

            try
            {
                //se crea un objeto
                var pacienteFiltroObj = new PacienteDto();
                pacienteFiltroObj.FiltroIdEstado = (int)EnumUtils.EstadoEnum.Paciente_Habilitado;
                //se envia el objeto al servicio
                var pacienteListDbResponse = this.PacienteServiceModel.GetListaPacienteAll(pacienteFiltroObj);

                //se devuelve la consulta
                viewModel.lstPaciente = pacienteListDbResponse.HasValue ? pacienteListDbResponse.Value : new List<PacienteDto>();

                if (pacienteListDbResponse.HasError)
                {
                    TempData["ErrorMessage"] = "Ha ocurrido un error al obtener la lista de Grupos de Trabajo. Por favor, inténtelo nuevamente";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Ha ocurrido un error al obtener la lista de pacientes. Por favor, inténtelo nuevamente";
            }

            return View(viewModel);

        }
        #endregion

        #region Edit
        [HttpGet]
        public ActionResult Edit(long p_Id)
        {
            EquipoPacienteViewModel viewModel = new EquipoPacienteViewModel();

            try
            {
                TempData.Clear();

                var filtroPaciente = new PacienteDto();
                filtroPaciente.FiltroId = p_Id;
                
                var oDb = this.EquipoPacienteServiceModel.GetEquipoPacienteByIdPaciente(filtroPaciente);
                if (oDb.HasValue)
                {
                    viewModel.Equipo = oDb.Value;
                    EquipoPacienteDto single = oDb.Value.SingleOrDefault();
                    viewModel.IdPaciente = single.IdPaciente;
                    viewModel.Paciente = single.Paciente;

                }
                
                
                if (oDb.HasError)
                    throw oDb.Error;
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Ha ocurrido un error al obtener los datos. Por favor, inténtelo nuevamente";

                return RedirectToAction("Index");
            }

            return View(viewModel);
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit(PacienteEditViewModel p_ViewModel)
        //{
        //    try
        //    {
        //        TempData.Clear();

        //        if (ModelState.IsValid)
        //        {
        //            PacienteDto objPacienteEdit = new PacienteDto();
        //            objPacienteEdit.Rut = p_ViewModel.Rut;
        //            objPacienteEdit.Nombre = p_ViewModel.Nombre;
        //            objPacienteEdit.IdEstado = (int)EnumUtils.EstadoEnum.Paciente_Habilitado;
        //            objPacienteEdit.ApellidoPaterno = p_ViewModel.ApellidoPaterno;
        //            objPacienteEdit.ApellidoMaterno = p_ViewModel.ApellidoMaterno;
        //            objPacienteEdit.Edad = p_ViewModel.Edad;
        //            objPacienteEdit.UsrUpdate = User.Identity.GetUserId();
        //            objPacienteEdit.FchUpdate = DateTime.Now;
        //            objPacienteEdit.Direccion = p_ViewModel.Direccion;
        //            objPacienteEdit.NumeroTelefono = p_ViewModel.Telefono;
        //            objPacienteEdit.Id = p_ViewModel.Id;

        //            var objResultadoDb = PacienteServiceModel.UpdatePaciente(objPacienteEdit);

        //            if (objResultadoDb.HasValue)
        //            {
        //                TempData["SaveOkMessage"] = "Paciente actualizado correctamente";
        //                return RedirectToAction("Index");
        //            }

        //            if (objResultadoDb.HasError)
        //                throw objResultadoDb.Error;
        //        }
        //        else
        //        {
        //            TempData["ErrorMessage"] = "Ha ocurrido un error al actualizar el paciente. Por favor, inténtelo nuevamente";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        TempData["ErrorMessage"] = "Ha ocurrido un error al actualizar el paciente. Por favor, inténtelo nuevamente";
        //    }

        //    return View(p_ViewModel);
        //}
        //#endregion


        //#region Create
        //[HttpGet]
        //public ActionResult Create()
        //{
        //    PacienteCreateViewModel viewModel = new PacienteCreateViewModel();

        //    try
        //    {
        //        var lstRoles = this.RolServiceModel.GetListaRolTutor();
        //        viewModel.LstCRol = lstRoles.HasValue ? lstRoles.Value : new List<RolDto>();

        //        var lstTopoTelefono = this.TIpoTelefonoServiceModel.GetListaTipoTelefonoAll();
        //        viewModel.LstTipoTelefono = lstTopoTelefono.HasValue ? lstTopoTelefono.Value : new List<TipoTelefonoDto>();

        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("Error al obtener listas solicitadas", ex);
        //    }

        //    return View(viewModel);
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create(PacienteCreateViewModel p_ViewModel)
        //{
        //    try
        //    {
        //        TempData.Clear();

        //        if (ModelState.IsValid)
        //        {
        //            bool isRutOk = true;
        //            if (!string.IsNullOrEmpty(p_ViewModel.Rut))
        //            {
        //                isRutOk = GlobalMethods.ValidarRut(p_ViewModel.Rut);
        //            }

        //            if (isRutOk)
        //            {
        //                var pacFiltroObj = new PacienteDto();
        //                pacFiltroObj.FiltroRut = p_ViewModel.Rut;
        //                pacFiltroObj.FiltroIdEstado = (int)EnumUtils.EstadoEnum.Paciente_Habilitado;
        //                var pacDbResponse = PacienteServiceModel.GetPacienteByRut(pacFiltroObj);

        //                if (!pacDbResponse.HasValue)
        //                {
        //                    PacienteDto objPacienteCreate = new PacienteDto();
        //                    objPacienteCreate.Rut = p_ViewModel.Rut;
        //                    objPacienteCreate.Nombre = p_ViewModel.Nombre;
        //                    objPacienteCreate.IdEstado = (int)EnumUtils.EstadoEnum.Paciente_Habilitado;
        //                    objPacienteCreate.ApellidoPaterno = p_ViewModel.ApellidoPaterno;
        //                    objPacienteCreate.ApellidoMaterno = p_ViewModel.ApellidoMaterno;
        //                    objPacienteCreate.Edad = p_ViewModel.Edad;
        //                    objPacienteCreate.UsrCreate = User.Identity.GetUserId();
        //                    objPacienteCreate.FchCreate = DateTime.Now;
        //                    objPacienteCreate.Direccion = p_ViewModel.Direccion;
        //                    objPacienteCreate.NumeroTelefono = p_ViewModel.Telefono;

        //                    var objResultadoDb = PacienteServiceModel.CreatePaciente(objPacienteCreate);

        //                    if (objResultadoDb.HasValue)
        //                    {
        //                        var UsuarioFiltroObj = new UsuarioDto();
        //                        UsuarioFiltroObj.FiltroUsername = p_ViewModel.Username;
        //                        var usuarioDbResponse = UsuarioServiceModel.GetUsuarioByUsername(UsuarioFiltroObj);

        //                        if (!usuarioDbResponse.HasValue)
        //                        {
        //                            UsuarioDto UsuarioNewDb = new UsuarioDto();
        //                            UsuarioNewDb.Username = p_ViewModel.Username;
        //                            UsuarioNewDb.Pass = GlobalMethods.EncryptPass(p_ViewModel.Pass);
        //                            UsuarioNewDb.IdRol = p_ViewModel.IdRol;

        //                            var objRespuesta = UsuarioServiceModel.InsertarUsuario(UsuarioNewDb);

        //                            if (!objRespuesta.HasError)
        //                            {
        //                                if (!string.IsNullOrEmpty(p_ViewModel.RutTutor))
        //                                {
        //                                    isRutOk = GlobalMethods.ValidarRut(p_ViewModel.RutTutor);
        //                                }

        //                                if (isRutOk)
        //                                {
        //                                    var UserFiltroObj = new UsuarioDto();
        //                                    UserFiltroObj.FiltroUsername = p_ViewModel.Username;
        //                                    var userDbResponse = UsuarioServiceModel.GetUsuarioByUsername(UserFiltroObj);

        //                                    var paciFiltroObj = new PacienteDto();
        //                                    paciFiltroObj.FiltroRut = p_ViewModel.Rut;
        //                                    paciFiltroObj.FiltroIdEstado = (int)EnumUtils.EstadoEnum.Paciente_Habilitado;
        //                                    var paciDbResponse = PacienteServiceModel.GetPacienteByRut(paciFiltroObj);

        //                                    var tutorFiltroObj = new TutorDto();
        //                                    tutorFiltroObj.FiltroRut = p_ViewModel.RutTutor;
        //                                    tutorFiltroObj.FiltroIdEstado = (int)EnumUtils.EstadoEnum.Tutor_Habilitado;
        //                                    var tutorDbResponse = TutorServiceModel.GetTutorByRut(tutorFiltroObj);

        //                                    if (userDbResponse.HasValue)
        //                                    {
        //                                        if (paciDbResponse.HasValue)
        //                                        {
        //                                            if (!tutorDbResponse.HasValue)
        //                                            {
        //                                                TutorDto objTutorCreate = new TutorDto();
        //                                                objTutorCreate.Rut = p_ViewModel.RutTutor;
        //                                                objTutorCreate.Nombre = p_ViewModel.NombreTutor;
        //                                                objTutorCreate.IdEstado = (int)EnumUtils.EstadoEnum.Tutor_Habilitado;
        //                                                objTutorCreate.ApellidoPaterno = p_ViewModel.ApellidoPaternoTutor;
        //                                                objTutorCreate.ApellidoMaterno = p_ViewModel.ApellidoMaternoTutor;
        //                                                objTutorCreate.UsrCreate = User.Identity.GetUserId();
        //                                                objTutorCreate.FchCreate = DateTime.Now;
        //                                                objTutorCreate.Direccion = p_ViewModel.DireccionTutor;
        //                                                objTutorCreate.Email = p_ViewModel.Email;
        //                                                objTutorCreate.IdUsuario = userDbResponse.Value.Id;
        //                                                objTutorCreate.IdPaciente = paciDbResponse.Value.Id;

        //                                                var objResultado = TutorServiceModel.InsertarTutor(objTutorCreate);

        //                                                if (objResultado.HasValue)
        //                                                {
        //                                                    var UserTelefonoFiltroObj = new UsuarioDto();
        //                                                    UserTelefonoFiltroObj.FiltroUsername = p_ViewModel.Username;
        //                                                    var userTelefonoDbResponse = UsuarioServiceModel.GetUsuarioByUsername(UserTelefonoFiltroObj);

        //                                                    if (userTelefonoDbResponse.HasValue)
        //                                                    {
        //                                                        TelefonoDto objtelefonoInsert = new TelefonoDto();
        //                                                        objtelefonoInsert.NumeroTelefono = p_ViewModel.TelefonoTutor;
        //                                                        objtelefonoInsert.IdTipoTelefono = p_ViewModel.IdTipoTelefono;
        //                                                        objtelefonoInsert.IdUsuario = userTelefonoDbResponse.Value.Id;

        //                                                        var objResutTelefono = TelefonoServiceModel.InsertarTelefono(objtelefonoInsert);

        //                                                        if (objResutTelefono.HasValue)
        //                                                        {
        //                                                            TempData["SaveOkMessage"] = "Registro Ingresado Correctamente";
        //                                                            return RedirectToAction("Index");
        //                                                        }
        //                                                        else
        //                                                        {
        //                                                            TempData["ErrorMessage"] = "Ha ocurrido un Error al crear el Registro. Por favor, inténtelo nuevamente";
        //                                                        }
        //                                                    }
        //                                                    else
        //                                                    {
        //                                                        TempData["ErrorMessage"] = "Ha ocurrido un Error al crear el Registro. Por favor, inténtelo nuevamente";
        //                                                    }
        //                                                }
        //                                                else
        //                                                {
        //                                                    TempData["ErrorMessage"] = "El Rut de este Tutor ya existe en nuestros registros, Intentelo Nuevamente";
        //                                                }
        //                                            }
        //                                            else
        //                                            {
        //                                                TempData["ErrorMessage"] = "Ha ocurrido un Error al crear el Registro. Por favor, inténtelo nuevamente";
        //                                            }
        //                                        }
        //                                        else
        //                                        {
        //                                            TempData["ErrorMessage"] = "El Rut asociado a el paciente ya existe en nuestros registros, Intentelo Nuevamente";
        //                                        }
        //                                    }
        //                                }
        //                                else
        //                                {
        //                                    TempData["ErrorMessage"] = "El RUT ingresado no es Válido, Intentelo Nuevamente";
        //                                }
        //                            }
        //                            else
        //                            {
        //                                TempData["ErrorMessage"] = "Ha ocurrido un Error al crear el Registro. Por favor, inténtelo nuevamente";
        //                            }
        //                        }
        //                        else
        //                        {
        //                            TempData["SaveOkMessage"] = "Usuario Existente";
        //                        }
        //                    }
        //                    else
        //                    {
        //                        TempData["ErrorMessage"] = "Ha ocurrido un Error, faltan datos a Ingresar. Por favor, inténtelo nuevamente";
        //                    }
        //                }
        //                else
        //                {
        //                    TempData["ErrorMessage"] = "El Rut de este Paciente ya existe en nuestros registros, Intentelo Nuevamente";
        //                }
        //            }
        //            else
        //            {
        //                TempData["ErrorMessage"] = "El RUT ingresado no es Válido, Intentelo Nuevamente";
        //            }
        //        }
        //        else
        //        {
        //            TempData["ErrorMessage"] = "Ha ocurrido un Error, faltan datos a Ingresar. Por favor, inténtelo nuevamente";
        //        }

        //    }

        //    catch (Exception ex)
        //    {
        //        TempData["ErrorMessage"] = "Ha ocurrido un Error al crear el Paciente. Por favor, inténtelo nuevamente";
        //    }

        //    CargarDatosCreatePaciente(p_ViewModel);

        //    return View(p_ViewModel);
        //}
        //private void CargarDatosCreatePaciente(PacienteCreateViewModel p_ViewModel)
        //{
        //    var lstRoles = this.RolServiceModel.GetListaRolTutor();
        //    p_ViewModel.LstCRol = lstRoles.HasValue ? lstRoles.Value : new List<RolDto>();

        //    var lstTipoTelefono = this.TIpoTelefonoServiceModel.GetListaTipoTelefonoAll();
        //    p_ViewModel.LstTipoTelefono = lstTipoTelefono.HasValue ? lstTipoTelefono.Value : new List<TipoTelefonoDto>();
        //}
        //#endregion



        //#region Delete
        //[HttpPost]
        //public string Delete(PacienteDetailViewModel p_ViewModel)
        //{
        //    String jsonResult = String.Empty;

        //    try
        //    {
        //        PacienteDto pacienteDelete = new PacienteDto();
        //        pacienteDelete.Id = p_ViewModel.Id;
        //        pacienteDelete.IdEstado = (int)EnumUtils.EstadoEnum.Paciente_Deshabilitado;
        //        pacienteDelete.FchUpdate = DateTime.Now;
        //        pacienteDelete.UsrUpdate = User.Identity.GetUserId();

        //        var objRespuesta = this.PacienteServiceModel.Delete(pacienteDelete);

        //        if (!objRespuesta.HasError)
        //        {
        //            jsonResult = JsonConvert.SerializeObject(
        //                new
        //                {
        //                    status = "ok",
        //                    message = "Usuario eliminado correctamente"
        //                });
        //        }
        //        else
        //        {
        //            jsonResult = JsonConvert.SerializeObject(
        //                    new
        //                    {
        //                        status = "error",
        //                        message = "No puede eliminar al usuario"
        //                    });
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        jsonResult = JsonConvert.SerializeObject(new { status = "error", message = "Error al intentar eliminar el usuario", ex });
        //    }

        //    return jsonResult;
        //}
        //#endregion

        //#region Detail
        //[HttpGet]
        //public ActionResult Detail(long p_Id)
        //{
        //    PacienteDetailViewModel viewModel = new PacienteDetailViewModel();

        //    try
        //    {
        //        PacienteDto filtroPaciente = new PacienteDto();
        //        filtroPaciente.FiltroId = p_Id;

        //        var objRespuestaDb = PacienteServiceModel.GetPacienteById(filtroPaciente);

        //        if (objRespuestaDb.HasValue)
        //        {
        //            viewModel = new PacienteDetailViewModel
        //            {
        //                DetalleEstado = objRespuestaDb.Value.DetalleEstado,
        //                FchCreate = objRespuestaDb.Value.FchCreate,
        //                FchUpdate = objRespuestaDb.Value.FchUpdate,
        //                Rut = objRespuestaDb.Value.Rut,
        //                Id = objRespuestaDb.Value.Id,
        //                IdEstado = objRespuestaDb.Value.IdEstado,
        //                Nombre = objRespuestaDb.Value.Nombre,
        //                ApellidoPaterno = objRespuestaDb.Value.ApellidoPaterno,
        //                ApellidoMaterno = objRespuestaDb.Value.ApellidoMaterno,
        //                Edad = objRespuestaDb.Value.Edad,
        //                Telefono = objRespuestaDb.Value.NumeroTelefono,
        //                Direccion = objRespuestaDb.Value.Direccion,
        //                UsrCreate = objRespuestaDb.Value.UsrCreate,
        //                UsrUpdate = objRespuestaDb.Value.UsrUpdate
        //            };
        //        }

        //        if (objRespuestaDb.HasError)
        //            throw objRespuestaDb.Error;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }

        //    return View(viewModel);
        //}
        //#endregion

        //#region IngresarDatosTutor
        //[HttpGet]
        //public ActionResult IngresarTutor()
        //{
        //    PacienteCreateViewModel viewModel = new PacienteCreateViewModel();

        //    return View(viewModel);
        //}

        //#endregion

        #endregion

        #endregion
    }
}