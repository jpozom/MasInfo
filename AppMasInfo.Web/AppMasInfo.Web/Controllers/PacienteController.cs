using AppMasInfo.Negocio.DAL.Entities;
using AppMasInfo.Negocio.DAL.Services;
using AppMasInfo.Utils.Utils;
using AppMasInfo.Web.Models;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static AppMasInfo.Web.Models.PacienteViewModel;

namespace AppMasInfo.Web.Controllers
{
    [Authorize/*(Roles = "Administrador")*/]
    public class PacienteController : Controller
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
                var pacienteListDbResponse = this.PacienteServiceModel.GetListaPacienteByEstado(pacienteFiltroObj);

                //se devuelve la consulta
                viewModel.lstPaciente = pacienteListDbResponse.HasValue ? pacienteListDbResponse.Value : new List<PacienteDto>();

                if (pacienteListDbResponse.HasError)
                {
                    TempData["ErrorMessage"] = "Ha ocurrido un Error al Obtener la Lista de Pacientes en DB. Por favor, Inténtelo Nuevamente";
                }

                //se crea un objeto
                var tutorFiltroObj = new TutorDto();
                tutorFiltroObj.FiltroIdEstado = (int)EnumUtils.EstadoEnum.Tutor_Habilitado;
                //se envia el objeto al servicio
                var tutorListDbResponse = this.TutorServiceModel.GetListaTutorByFiltro(tutorFiltroObj);

                //se devuelve la consulta
                viewModel.lstTutor = tutorListDbResponse.HasValue ? tutorListDbResponse.Value : new List<TutorDto>();

                if (pacienteListDbResponse.HasError)
                {
                    TempData["ErrorMessage"] = "Ha ocurrido un Error al Obtener la Lista de Tutores en DB. Por favor, Inténtelo Nuevamente";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Ha ocurrido un error al obtener registros de DB. Por favor, inténtelo nuevamente" + ex;
            }

            return View(viewModel);
        }
        #endregion

        #region Create
        [HttpGet]
        public ActionResult Create()
        {
            PacienteCreateViewModel viewModel = new PacienteCreateViewModel();

            try
            {
                // se obtiene una lista completa de roles donde solo sea rol tutor
                var lstRoles = this.RolServiceModel.GetListaRolTutor();
                viewModel.LstCRol = lstRoles.HasValue ? lstRoles.Value : new List<RolDto>();

                // se obtiene una lista completa de la tabla tipo telefono
                var lstTopoTelefono = this.TIpoTelefonoServiceModel.GetListaTipoTelefonoAll();
                viewModel.LstTipoTelefono = lstTopoTelefono.HasValue ? lstTopoTelefono.Value : new List<TipoTelefonoDto>();

            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener listas solicitadas", ex);
            }

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PacienteCreateViewModel p_ViewModel)
        {
            try
            {
                TempData.Clear();

                if (ModelState.IsValid)
                {
                    // condicion que permite validar si el rut que ingresa desde el modelo es valido
                    bool isRutOk = true;
                    if (!string.IsNullOrEmpty(p_ViewModel.Rut))
                    {
                        isRutOk = GlobalMethods.ValidarRut(p_ViewModel.Rut);
                    }

                    if (isRutOk)
                    {
                        //filtro que permite consultar si el username ya existe en BD
                        var UsuarioFiltroObj = new UsuarioDto();
                        UsuarioFiltroObj.FiltroUsername = p_ViewModel.Username;
                        var usuarioDbResponse = UsuarioServiceModel.GetUsuarioByUsername(UsuarioFiltroObj);

                        // si el objeto viene con valor entra en la condicion 
                        if (!usuarioDbResponse.HasValue)
                        {
                            //se crea un objeto 
                            UsuarioDto UsuarioNewDb = new UsuarioDto();
                            UsuarioNewDb.Username = p_ViewModel.Username;
                            UsuarioNewDb.Pass = GlobalMethods.EncryptPass(p_ViewModel.Pass);
                            UsuarioNewDb.IdRol = p_ViewModel.IdRol;

                            //el objeto se envia al servicio para insertar
                            var objRespuesta = UsuarioServiceModel.InsertarUsuario(UsuarioNewDb);

                            if (!objRespuesta.HasError)
                            {
                                var pacFiltroObj = new PacienteDto();
                                pacFiltroObj.FiltroRut = p_ViewModel.Rut;
                                pacFiltroObj.FiltroIdEstado = (int)EnumUtils.EstadoEnum.Paciente_Habilitado;
                                var pacDbResponse = PacienteServiceModel.GetPacienteByRut(pacFiltroObj);

                                if (!pacDbResponse.HasValue)
                                {
                                    PacienteDto objPacienteCreate = new PacienteDto();
                                    objPacienteCreate.Rut = p_ViewModel.Rut;
                                    objPacienteCreate.Nombre = p_ViewModel.Nombre;
                                    objPacienteCreate.IdEstado = (int)EnumUtils.EstadoEnum.Paciente_Habilitado;
                                    objPacienteCreate.ApellidoPaterno = p_ViewModel.ApellidoPaterno;
                                    objPacienteCreate.ApellidoMaterno = p_ViewModel.ApellidoMaterno;
                                    objPacienteCreate.Edad = p_ViewModel.Edad;
                                    objPacienteCreate.UsrCreate = User.Identity.GetUserId();
                                    objPacienteCreate.FchCreate = DateTime.Now;
                                    objPacienteCreate.Direccion = p_ViewModel.Direccion;
                                    objPacienteCreate.NumeroTelefono = p_ViewModel.Telefono;

                                    var objResultadoDb = PacienteServiceModel.CreatePaciente(objPacienteCreate);

                                    if (objResultadoDb.HasValue)
                                    {
                                        if (!string.IsNullOrEmpty(p_ViewModel.RutTutor))
                                        {
                                            isRutOk = GlobalMethods.ValidarRut(p_ViewModel.RutTutor);
                                        }

                                        if (isRutOk)
                                        {
                                            var UserFiltroObj = new UsuarioDto();
                                            UserFiltroObj.FiltroUsername = p_ViewModel.Username;
                                            var userDbResponse = UsuarioServiceModel.GetUsuarioByUsername(UserFiltroObj);

                                            var paciFiltroObj = new PacienteDto();
                                            paciFiltroObj.FiltroRut = p_ViewModel.Rut;
                                            paciFiltroObj.FiltroIdEstado = (int)EnumUtils.EstadoEnum.Paciente_Habilitado;
                                            var paciDbResponse = PacienteServiceModel.GetPacienteByRut(paciFiltroObj);

                                            var tutorFiltroObj = new TutorDto();
                                            tutorFiltroObj.FiltroRut = p_ViewModel.RutTutor;
                                            tutorFiltroObj.FiltroIdEstado = (int)EnumUtils.EstadoEnum.Tutor_Habilitado;
                                            var tutorDbResponse = TutorServiceModel.GetTutorByRut(tutorFiltroObj);
                                            
                                            if (userDbResponse.HasValue)
                                            {
                                                if (paciDbResponse.HasValue)
                                                {
                                                    if (!tutorDbResponse.HasValue)
                                                    {
                                                        TutorDto objTutorCreate = new TutorDto();
                                                        objTutorCreate.Rut = p_ViewModel.RutTutor;
                                                        objTutorCreate.Nombre = p_ViewModel.NombreTutor;
                                                        objTutorCreate.IdEstado = (int)EnumUtils.EstadoEnum.Tutor_Habilitado;
                                                        objTutorCreate.ApellidoPaterno = p_ViewModel.ApellidoPaternoTutor;
                                                        objTutorCreate.ApellidoMaterno = p_ViewModel.ApellidoMaternoTutor;
                                                        objTutorCreate.Edad = p_ViewModel.EdadTutor;
                                                        objTutorCreate.UsrCreate = User.Identity.GetUserId();
                                                        objTutorCreate.FchCreate = DateTime.Now;
                                                        objTutorCreate.Direccion = p_ViewModel.DireccionTutor;
                                                        objTutorCreate.Email = p_ViewModel.Email;
                                                        objTutorCreate.IdUsuario = userDbResponse.Value.Id;
                                                        objTutorCreate.IdPaciente = paciDbResponse.Value.Id;

                                                        var objResultado = TutorServiceModel.InsertarTutor(objTutorCreate);

                                                        if (objResultado.HasValue)
                                                        {
                                                            var UserTelefonoFiltroObj = new UsuarioDto();
                                                            UserTelefonoFiltroObj.FiltroUsername = p_ViewModel.Username;
                                                            var userTelefonoDbResponse = UsuarioServiceModel.GetUsuarioByUsername(UserTelefonoFiltroObj);

                                                            if (userTelefonoDbResponse.HasValue)
                                                            {
                                                                TelefonoDto objtelefonoInsert = new TelefonoDto();
                                                                objtelefonoInsert.NumeroTelefono = p_ViewModel.TelefonoTutor;
                                                                objtelefonoInsert.IdTipoTelefono = p_ViewModel.IdTipoTelefono;
                                                                objtelefonoInsert.IdUsuario = userTelefonoDbResponse.Value.Id;

                                                                var objResutTelefono = TelefonoServiceModel.InsertarTelefono(objtelefonoInsert);

                                                                if (objResutTelefono.HasValue)
                                                                {
                                                                    TempData["SaveOkMessage"] = "Registro Ingresado Correctamente";
                                                                    return RedirectToAction("Index");
                                                                }
                                                                else
                                                                {
                                                                    TempData["ErrorMessage"] = "Ha ocurrido un Error al crear el Registro. Por favor, inténtelo nuevamente";
                                                                }
                                                            }
                                                            else
                                                            {
                                                                TempData["ErrorMessage"] = "Ha ocurrido un Error al crear el Registro. Por favor, inténtelo nuevamente";
                                                            }
                                                        }
                                                        else
                                                        {
                                                            TempData["ErrorMessage"] = "Ha ocurrido un Error al Crear un Tutor. Por favor, inténtelo nuevamente";
                                                        }
                                                    }
                                                    else
                                                    {
                                                        TempData["ErrorMessage"] = "El Rut asociado a el Tutor ya existe en nuestros registros, Intentelo Nuevamente";
                                                    }
                                                }
                                                else
                                                {
                                                    TempData["ErrorMessage"] = "El Rut asociado a el paciente ya existe en nuestros registros, Intentelo Nuevamente";
                                                }
                                            }
                                            else
                                            {
                                                TempData["ErrorMessage"] = "El Username del Tutor existe en BD, Ingrese otro por Favor";
                                            }
                                        }
                                        else
                                        {
                                            TempData["ErrorMessage"] = "El RUT ingresado no es Válido, Intentelo Nuevamente";
                                        }
                                    }
                                    else
                                    {
                                        TempData["ErrorMessage"] = "Ha ocurrido un Error al Crear un Paciente. Por favor, inténtelo nuevamente";
                                    }
                                }
                                else
                                {
                                    TempData["SaveOkMessage"] = "El Rut del Paciente Existe";
                                }
                            }
                            else
                            {
                                TempData["ErrorMessage"] = "Ha ocurrido un Error al Insertar el Usuario. Por favor, inténtelo nuevamente";
                            }
                        }
                        else
                        {
                            TempData["ErrorMessage"] = "El Username existe en Base de Datos, ingrese otro Username por Favor";
                        }
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "El RUT ingresado no es Válido, Intentelo Nuevamente";
                    }
                }
                else
                {
                    TempData["ErrorMessage"] = "Ha ocurrido un Error, faltan datos a Ingresar. Por favor, inténtelo nuevamente";
                }

                CargarDatosCreatePaciente(p_ViewModel);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Ha ocurrido un Error al crear el Paciente. Por favor, inténtelo nuevamente" + ex;
            }

            return View(p_ViewModel);
        }
        private void CargarDatosCreatePaciente(PacienteCreateViewModel p_ViewModel)
        {
            var lstRoles = this.RolServiceModel.GetListaRolTutor();
            p_ViewModel.LstCRol = lstRoles.HasValue ? lstRoles.Value : new List<RolDto>();

            var lstTipoTelefono = this.TIpoTelefonoServiceModel.GetListaTipoTelefonoAll();
            p_ViewModel.LstTipoTelefono = lstTipoTelefono.HasValue ? lstTipoTelefono.Value : new List<TipoTelefonoDto>();
        }
        #endregion

        #region Edit
        [HttpGet]
        public ActionResult Edit(long p_Id)
        {
            PacienteEditViewModel viewModel = new PacienteEditViewModel();

            try
            {
                TempData.Clear();

                var lstRoles = this.RolServiceModel.GetListaRolTutor();
                viewModel.LstCRol = lstRoles.HasValue ? lstRoles.Value : new List<RolDto>();

                var lstTopoTelefono = this.TIpoTelefonoServiceModel.GetListaTipoTelefonoAll();
                viewModel.LstTipoTelefono = lstTopoTelefono.HasValue ? lstTopoTelefono.Value : new List<TipoTelefonoDto>();


                var filtroPaciente = new PacienteDto();
                filtroPaciente.FiltroId = p_Id;
                filtroPaciente.FiltroIdEstado = (int)EnumUtils.EstadoEnum.Paciente_Habilitado;

                var objResultadoDb = this.PacienteServiceModel.GetPacienteById(filtroPaciente);

                if (objResultadoDb.HasValue)
                {
                    viewModel.Id = objResultadoDb.Value.Id;
                    viewModel.Rut = objResultadoDb.Value.Rut;
                    viewModel.Nombre = objResultadoDb.Value.Nombre;
                    viewModel.ApellidoPaterno = objResultadoDb.Value.ApellidoPaterno;
                    viewModel.ApellidoMaterno = objResultadoDb.Value.ApellidoMaterno;                    
                    viewModel.Edad = objResultadoDb.Value.Edad;
                    viewModel.Direccion = objResultadoDb.Value.Direccion;
                    viewModel.Telefono = objResultadoDb.Value.NumeroTelefono;
                }

                if (objResultadoDb.HasError)
                    throw objResultadoDb.Error;

                var filtroTutor = new TutorDto();
                filtroTutor.FiltroIdPaciente = p_Id;
                filtroTutor.FiltroIdEstado = (int)EnumUtils.EstadoEnum.Tutor_Habilitado;

                var objResultTutorDb = this.TutorServiceModel.GetTutorByPaciente(filtroTutor);

                if (objResultTutorDb.HasValue)
                {
                    viewModel.IdTutor = objResultTutorDb.Value.Id;
                    viewModel.RutTutor = objResultTutorDb.Value.Rut;
                    viewModel.NombreTutor = objResultTutorDb.Value.Nombre;
                    viewModel.ApellidoPaternoTutor = objResultTutorDb.Value.ApellidoPaterno;
                    viewModel.ApellidoMaternoTutor = objResultTutorDb.Value.ApellidoMaterno;
                    viewModel.EdadTutor = objResultadoDb.Value.Edad;
                    viewModel.DireccionTutor = objResultTutorDb.Value.Direccion;
                    viewModel.Email = objResultTutorDb.Value.Email;
                    viewModel.Username = objResultTutorDb.Value.DatosUsuario.Username;
                    viewModel.IdRol = objResultTutorDb.Value.DetalleRol.Id;
                }

                if (objResultadoDb.HasError)
                    throw objResultadoDb.Error;

                var filtroTelefono = new TelefonoDto();
                filtroTelefono.FiltroIdUsuario = objResultTutorDb.Value.DatosUsuario.Id;

                var objResultTelefonoDb = this.TelefonoServiceModel.GetTelefonoByIdUsuario(filtroTelefono);

                if (objResultTelefonoDb.HasValue)
                {
                    viewModel.TelefonoTutor = objResultTelefonoDb.Value.NumeroTelefono;
                    viewModel.IdTipoTelefono = objResultTelefonoDb.Value.IdTipoTelefono;
                    viewModel.IdUsuario = objResultTelefonoDb.Value.IdUsuario;
                    viewModel.IdTelefono = objResultTelefonoDb.Value.Id;
                }

                if (objResultadoDb.HasError)
                    throw objResultadoDb.Error;
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Ha ocurrido un error al obtener los datos. Por favor, inténtelo nuevamente" + ex;
                return RedirectToAction("Index");
            }

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PacienteEditViewModel p_ViewModel)
        {
            try
            {
                TempData.Clear();

                if (ModelState.IsValid)
                {
                    bool isRutOk = true;
                    if (!string.IsNullOrEmpty(p_ViewModel.Rut))
                    {
                        isRutOk = GlobalMethods.ValidarRut(p_ViewModel.Rut);
                    }

                    if (isRutOk)
                    {
                        PacienteDto objPacienteEdit = new PacienteDto();
                        objPacienteEdit.Nombre = p_ViewModel.Nombre;
                        objPacienteEdit.IdEstado = (int)EnumUtils.EstadoEnum.Paciente_Habilitado;
                        objPacienteEdit.ApellidoPaterno = p_ViewModel.ApellidoPaterno;
                        objPacienteEdit.ApellidoMaterno = p_ViewModel.ApellidoMaterno;
                        objPacienteEdit.Edad = p_ViewModel.Edad;
                        objPacienteEdit.UsrUpdate = User.Identity.GetUserId();
                        objPacienteEdit.FchUpdate = DateTime.Now;
                        objPacienteEdit.Direccion = p_ViewModel.Direccion;
                        objPacienteEdit.NumeroTelefono = p_ViewModel.Telefono;
                        objPacienteEdit.Id = p_ViewModel.Id;

                        var objResultadoDb = PacienteServiceModel.UpdatePaciente(objPacienteEdit);

                        if (objResultadoDb.HasValue)
                        {
                            UsuarioDto objUsuarioEdit = new UsuarioDto();
                            objUsuarioEdit.Pass = p_ViewModel.Pass == null ? "" : GlobalMethods.EncryptPass(p_ViewModel.Pass);
                            objUsuarioEdit.IdRol = p_ViewModel.IdRol;
                            objUsuarioEdit.Id = p_ViewModel.IdUsuario;

                            var objRespuesta = UsuarioServiceModel.UpdateUsuario(objUsuarioEdit);

                            if (!objRespuesta.HasError)
                            {
                                if (!string.IsNullOrEmpty(p_ViewModel.RutTutor))
                                {
                                    isRutOk = GlobalMethods.ValidarRut(p_ViewModel.RutTutor);
                                }

                                if (isRutOk)
                                {
                                    TutorDto objTutorUpdate = new TutorDto();
                                    objTutorUpdate.Nombre = p_ViewModel.NombreTutor;
                                    objTutorUpdate.IdEstado = (int)EnumUtils.EstadoEnum.Tutor_Habilitado;
                                    objTutorUpdate.ApellidoPaterno = p_ViewModel.ApellidoPaternoTutor;
                                    objTutorUpdate.ApellidoMaterno = p_ViewModel.ApellidoMaternoTutor;
                                    objTutorUpdate.Edad = p_ViewModel.EdadTutor;
                                    objTutorUpdate.UsrUpdate = User.Identity.GetUserId();
                                    objTutorUpdate.FchUpdate = DateTime.Now;
                                    objTutorUpdate.Direccion = p_ViewModel.DireccionTutor;
                                    objTutorUpdate.Email = p_ViewModel.Email;
                                    objTutorUpdate.Id = p_ViewModel.IdTutor;
                                    objTutorUpdate.IdPaciente = p_ViewModel.Id;

                                    var objResultado = TutorServiceModel.UpdateTutor(objTutorUpdate);

                                    if (objResultado.HasValue)
                                    {
                                        TelefonoDto objtelefonoUpdate = new TelefonoDto();
                                        objtelefonoUpdate.NumeroTelefono = p_ViewModel.TelefonoTutor;
                                        objtelefonoUpdate.IdTipoTelefono = p_ViewModel.IdTipoTelefono;
                                        objtelefonoUpdate.IdUsuario = p_ViewModel.IdUsuario;
                                        objtelefonoUpdate.Id = p_ViewModel.IdTelefono;

                                        var objResutTelefono = TelefonoServiceModel.UpdateTelefono(objtelefonoUpdate);

                                        if (objResutTelefono.HasValue)
                                        {
                                            TempData["SaveOkMessage"] = "Registro Actualizado Correctamente";
                                            return RedirectToAction("Index");
                                        }
                                        else
                                        {
                                            TempData["ErrorMessage"] = "Ha ocurrido un Error al Actualizar el Registro. Por favor, Inténtelo Nuevamente";
                                        }
                                    }
                                    else
                                    {
                                        TempData["ErrorMessage"] = "Ha ocurrido un Error al Obtener Datos del Registro. Por favor, Inténtelo Nuevamente";
                                    }
                                }
                                else
                                {
                                    TempData["ErrorMessage"] = "El RUT ingresado no es Válido, Intentelo Nuevamente";
                                }
                            }
                            else
                            {
                                TempData["ErrorMessage"] = "Ha ocurrido un Error al Obtener Datos del Registro. Por favor, Inténtelo Nuevamente";
                            }
                        }
                        else
                        {
                            TempData["ErrorMessage"] = "Ha ocurrido un Error al Obtener Datos del Registro. Por favor, Inténtelo Nuevamente";
                        }
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "El RUT ingresado no es Válido, Inténtelo Nuevamente";
                    }
                }
                else
                {
                    TempData["ErrorMessage"] = "Ha ocurrido un Error al Actualizar el Registro. Por favor, Inténtelo Nuevamente";
                }

                this.CargarDatosEditView(ref p_ViewModel);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Ha ocurrido un Error al Actualizar el Registro. Por favor, Inténtelo Nuevamente" + ex;
            }

            return View(p_ViewModel);
        }

        private void CargarDatosEditView(ref PacienteEditViewModel p_ViewModel)
        {
            var lstRoles = this.RolServiceModel.GetListaRolTutor();
            p_ViewModel.LstCRol = lstRoles.HasValue ? lstRoles.Value : new List<RolDto>();

            var lstTipoTelefono = this.TIpoTelefonoServiceModel.GetListaTipoTelefonoAll();
            p_ViewModel.LstTipoTelefono = lstTipoTelefono.HasValue ? lstTipoTelefono.Value : new List<TipoTelefonoDto>();
        }
        #endregion

        #region Delete
        [HttpPost]
        public string Delete(PacienteDetailViewModel p_ViewModel)
        {
            String jsonResult = String.Empty;

            try
            {
                PacienteDto pacienteDelete = new PacienteDto();
                pacienteDelete.Id = p_ViewModel.IdPaciente;
                pacienteDelete.IdEstado = (int)EnumUtils.EstadoEnum.Paciente_Deshabilitado;
                pacienteDelete.FchUpdate = DateTime.Now;
                pacienteDelete.UsrUpdate = User.Identity.GetUserId();
                pacienteDelete.FiltroId = p_ViewModel.IdPaciente;

                TutorDto tutorDelete = new TutorDto();
                tutorDelete.IdPaciente = p_ViewModel.IdPaciente;
                tutorDelete.IdEstado = (int)EnumUtils.EstadoEnum.Tutor_Deshabilitado;
                tutorDelete.FchUpdate = DateTime.Now;
                tutorDelete.UsrUpdate = User.Identity.GetUserId();
                tutorDelete.FiltroIdPaciente = p_ViewModel.IdPaciente;

                var tutorDB = this.TutorServiceModel.GetTutorByPaciente(tutorDelete);

                UsuarioDto userDelete = new UsuarioDto();
                userDelete.Id = tutorDB.Value.IdUsuario;

                var objRespDB = this.UsuarioServiceModel.Delete(userDelete);

                if (!objRespDB.HasError)
                {
                    var objRespuestaDB = this.TutorServiceModel.Delete(tutorDelete);

                    if (!objRespuestaDB.HasError)
                    {
                        var objRespuesta = this.PacienteServiceModel.Delete(pacienteDelete);

                        if (!objRespuesta.HasError)
                        {
                            jsonResult = JsonConvert.SerializeObject(
                                new
                                {
                                    status = "ok",
                                    message = "Registro eliminado correctamente"
                                });
                        }
                        else
                        {
                            jsonResult = JsonConvert.SerializeObject(
                                    new
                                    {
                                        status = "error",
                                        message = "No puede eliminar al Tutor"
                                    });
                        }

                    }
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
            PacienteDetailViewModel viewModel = new PacienteDetailViewModel();

            try
            {
                PacienteDto filtroPaciente = new PacienteDto();
                filtroPaciente.FiltroId = p_Id;

                var objRespuestaDb = PacienteServiceModel.GetPacienteById(filtroPaciente);

                if (objRespuestaDb.HasValue)
                {
                    viewModel = new PacienteDetailViewModel
                    {
                        DetalleEstado = objRespuestaDb.Value.DetalleEstado,
                        FchCreatePaciente = objRespuestaDb.Value.FchCreate,
                        FchUpdatePaciente = objRespuestaDb.Value.FchUpdate,
                        RutPaciente = objRespuestaDb.Value.Rut,
                        IdPaciente = objRespuestaDb.Value.Id,
                        IdEstado = objRespuestaDb.Value.IdEstado,
                        NombrePaciente = objRespuestaDb.Value.Nombre,
                        ApellidoPaternoPaciente = objRespuestaDb.Value.ApellidoPaterno,
                        ApellidoMaternoPaciente = objRespuestaDb.Value.ApellidoMaterno,
                        Edad = objRespuestaDb.Value.Edad,
                        NumeroTelefono = objRespuestaDb.Value.NumeroTelefono,
                        DireccionPaciente = objRespuestaDb.Value.Direccion,
                        UsrCreatePaciente = objRespuestaDb.Value.UsrCreate,
                        UsrUpdatePaciente = objRespuestaDb.Value.UsrUpdate,
                    };
                }
                else
                {
                    TempData["ErrorMessage"] = "Ha ocurrido un error al obtener los datos. Por favor, inténtelo nuevamente";
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

        #region DetailTutor
        [HttpGet]
        public ActionResult DetailTutor(long p_Id)
        {
            TutorDetailViewModel viewModel = new TutorDetailViewModel();

            try
            {
                TutorDto filtroTutor = new TutorDto();
                filtroTutor.FiltroId = p_Id;

                var objRespuestaDb = TutorServiceModel.GetTutorById(filtroTutor);

                if (objRespuestaDb.HasValue)
                {
                    viewModel = new TutorDetailViewModel
                    {
                        IdUsuario = objRespuestaDb.Value.DatosUsuario.Id,
                        IdTelefono = objRespuestaDb.Value.DetalleTelefono.Id,
                        IdTipoTelefono = objRespuestaDb.Value.DetalleTelefono.IdTipoTelefono,
                        DetalleTipoTelefono = objRespuestaDb.Value.DetalleTipoTelefono,
                        IdPaciente = objRespuestaDb.Value.DetallePaciente.Id,
                        DetalleEstado = objRespuestaDb.Value.DetalleEstado,
                        FchCreateTutor = objRespuestaDb.Value.FchCreate,
                        FchUpdateTutor = objRespuestaDb.Value.FchUpdate,
                        DatosUsuario = objRespuestaDb.Value.DatosUsuario,
                        RutTutor = objRespuestaDb.Value.Rut,
                        IdTutor = objRespuestaDb.Value.Id,
                        IdEstado = objRespuestaDb.Value.IdEstado,
                        NombreTutor = objRespuestaDb.Value.Nombre,
                        ApellidoPaternoTutor = objRespuestaDb.Value.ApellidoPaterno,
                        ApellidoMaternoTutor = objRespuestaDb.Value.ApellidoMaterno,
                        Edad = objRespuestaDb.Value.Edad,
                        DetalleTelefono = objRespuestaDb.Value.DetalleTelefono,
                        DireccionTutor = objRespuestaDb.Value.Direccion,
                        UsrCreateTutor = objRespuestaDb.Value.UsrCreate,
                        UsrUpdateTutor = objRespuestaDb.Value.UsrUpdate
                    };
                }
                else
                {
                    TempData["ErrorMessage"] = "Ha ocurrido un error al obtener los datos. Por favor, inténtelo nuevamente";
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
