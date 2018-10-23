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
    [Authorize]
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
                var pacienteListDbResponse = this.PacienteServiceModel.GetPacienteAll(pacienteFiltroObj);

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

        #region Create
        [HttpGet]
        public ActionResult Create()
        {
            return View();
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
                    objPacienteCreate.Telefono = p_ViewModel.Telefono;
                    objPacienteCreate.IdTutor = p_ViewModel.IdTutor;

                    var objResultadoDb = PacienteServiceModel.CreatePaciente(objPacienteCreate);

                    if (objResultadoDb.HasValue)
                    {
                        TempData["SaveOkMessage"] = "Usuario ingresado correctamente";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Ha ocurrido un error al crear el usuario. Por favor, inténtelo nuevamente";
                    }

                    if (objResultadoDb.HasError)
                        throw objResultadoDb.Error;

                }
                else
                {
                    TempData["ErrorMessage"] = "Ha ocurrido un error al crear el usuario. Por favor, inténtelo nuevamente";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Ha ocurrido un error al crear el usuario. Por favor, inténtelo nuevamente";
            }

            return View(p_ViewModel);
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
                    viewModel.Telefono = objResultadoDb.Value.Telefono;
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
        public ActionResult Edit(PacienteEditViewModel p_ViewModel)
        {
            try
            {
                TempData.Clear();

                if (ModelState.IsValid)
                {
                    PacienteDto objPacienteEdit = new PacienteDto();
                    objPacienteEdit.Rut = p_ViewModel.Rut;
                    objPacienteEdit.Nombre = p_ViewModel.Nombre;
                    objPacienteEdit.IdEstado = (int)EnumUtils.EstadoEnum.Paciente_Habilitado;
                    objPacienteEdit.ApellidoPaterno = p_ViewModel.ApellidoPaterno;
                    objPacienteEdit.ApellidoMaterno = p_ViewModel.ApellidoMaterno;
                    objPacienteEdit.Edad = p_ViewModel.Edad;
                    objPacienteEdit.UsrUpdate = User.Identity.GetUserId();
                    objPacienteEdit.FchUpdate = DateTime.Now;
                    objPacienteEdit.Direccion = p_ViewModel.Direccion;
                    objPacienteEdit.Telefono = p_ViewModel.Telefono;
                    objPacienteEdit.IdTutor = p_ViewModel.IdTutor;
                    objPacienteEdit.Id = p_ViewModel.Id;

                    var objResultadoDb = PacienteServiceModel.UpdatePaciente(objPacienteEdit);

                    if (objResultadoDb.HasValue)
                    {
                        TempData["SaveOkMessage"] = "Paciente actualizado correctamente";
                        return RedirectToAction("Index");
                    }


                    if (objResultadoDb.HasError)
                        throw objResultadoDb.Error;
                }
                else
                {
                    TempData["ErrorMessage"] = "Ha ocurrido un error al actualizar el paciente. Por favor, inténtelo nuevamente";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Ha ocurrido un error al actualizar el paciente. Por favor, inténtelo nuevamente";
            }

            return View(p_ViewModel);
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
                pacienteDelete.Id = p_ViewModel.Id;
                pacienteDelete.IdEstado = (int)EnumUtils.EstadoEnum.Paciente_Deshabilitado;
                pacienteDelete.FchUpdate = DateTime.Now;
                pacienteDelete.UsrUpdate = User.Identity.GetUserId();

                var objRespuesta = this.PacienteServiceModel.Delete(pacienteDelete);

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
                        FchCreate = objRespuestaDb.Value.FchCreate,
                        FchUpdate = objRespuestaDb.Value.FchUpdate,
                        Rut = objRespuestaDb.Value.Rut,
                        Id = objRespuestaDb.Value.Id,
                        IdEstado = objRespuestaDb.Value.IdEstado,
                        Nombre = objRespuestaDb.Value.Nombre,
                        ApellidoPaterno = objRespuestaDb.Value.ApellidoPaterno,
                        ApellidoMaterno = objRespuestaDb.Value.ApellidoMaterno,
                        Edad = objRespuestaDb.Value.Edad,
                        Telefono = objRespuestaDb.Value.Telefono,
                        Direccion = objRespuestaDb.Value.Direccion,
                        UsrCreate = objRespuestaDb.Value.UsrCreate,
                        UsrUpdate = objRespuestaDb.Value.UsrUpdate
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