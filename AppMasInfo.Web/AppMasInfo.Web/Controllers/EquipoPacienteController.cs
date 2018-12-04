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
        private ITrabajadorService TrabajadorServiceModel
        {
            get
            {
                return TrabajadorService.GetInstance();
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
                    TempData["ErrorMessage"] = "Ha ocurrido un error al obtener la lista de Grupos de Trabajo. Por favor, inténtelo nuevamente";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Ha ocurrido un error al obtener la lista de pacientes. Por favor, inténtelo nuevamente" + ex;
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
                
                //oDb = variable que contiene el ojeto enviado al servicio para la consulta
                var oDb = this.EquipoPacienteServiceModel.GetEquipoPacienteByIdPaciente(filtroPaciente);
                // contiene el número de elementos que puede contener el objeto, en este caso se almacenan las id del paciente.
                var ids = new HashSet<long>();
                if (oDb.HasValue)
                {
                    viewModel.Paciente = new PacienteDto();
                    BaseDto<PacienteDto> paciente = this.PacienteServiceModel.GetPacienteById(filtroPaciente);
                    if (paciente.HasValue) {
                        viewModel.Paciente = paciente.Value;                        
                    }
                    viewModel.Equipo = oDb.Value;
                    ids = new HashSet<long>(oDb.Value.Select(x => x.Idtrabajador));
                }
                var tDb = this.TrabajadorServiceModel.GetListaTrabajadorAll().Value ;
                viewModel.LstTrabajadores = new List<TrabajadorDto>();
                //.Contains() metodo Determina si un objeto HashSet<T> contiene el elemento especificado.
                viewModel.LstTrabajadores = tDb.Where(t=> !ids.Contains(t.Id)).ToList();
                viewModel.Idtrabajador = 0;
                viewModel.IdPaciente = p_Id;
                if (oDb.HasError)
                    throw oDb.Error;
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Ha ocurrido un error al obtener los datos. Por favor, inténtelo nuevamente" + ex;
                return RedirectToAction("Index");
            }

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Edit(EquipoPacienteViewModel viewModel)
        {
            try
            {
                TempData.Clear();

                //guardar asignacion
                EquipoPacienteDto equipo = new EquipoPacienteDto();
                equipo.IdPaciente = viewModel.IdPaciente;
                equipo.Idtrabajador= viewModel.Idtrabajador;               
                BaseDto<bool> res = this.EquipoPacienteServiceModel.AddEquipoPaciente(equipo);

                if (res.Value)
                {
                    TempData["SaveOkMessage"] = "Trabajador asignado correctamente";

                }
                else
                {
                    TempData["ErrorMessage"] = "Ha ocurrido un error al asignar. Por favor, inténtelo nuevamente";

                }
                var filtroPaciente = new PacienteDto();
                filtroPaciente.FiltroId = viewModel.IdPaciente;

                var oDb = this.EquipoPacienteServiceModel.GetEquipoPacienteByIdPaciente(filtroPaciente);
                var ids = new HashSet<long>();
                if (oDb.HasValue)
                {
                    viewModel.Paciente = new PacienteDto();
                    BaseDto<PacienteDto> paciente = this.PacienteServiceModel.GetPacienteById(filtroPaciente);
                    if (paciente.HasValue)
                    {
                        viewModel.Paciente = paciente.Value;
                    }
                    viewModel.Equipo = oDb.Value;
                    ids = new HashSet<long>(oDb.Value.Select(x => x.Idtrabajador));
                }
                var tDb = this.TrabajadorServiceModel.GetListaTrabajadorAll().Value;
                viewModel.LstTrabajadores = new List<TrabajadorDto>();
                viewModel.LstTrabajadores = tDb.Where(t => !ids.Contains(t.Id)).ToList();
                viewModel.Idtrabajador = 0;
                viewModel.IdPaciente = viewModel.IdPaciente;
                if (oDb.HasError)
                    throw oDb.Error;
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Ha ocurrido un error al obtener los datos. Por favor, inténtelo nuevamente" + ex;
                return RedirectToAction("Index");
            }

            return View(viewModel);
        }
        #endregion

        #region Delete
        [HttpPost]
        public string Delete(EquipoPacienteViewModel p_ViewModel)
        {
            String jsonResult = String.Empty;

            try
            {
                EquipoPacienteDto equipoDelete = new EquipoPacienteDto();
                equipoDelete.Id = p_ViewModel.Id;

                var objRespuesta = this.EquipoPacienteServiceModel.Delete(equipoDelete);

                if (!objRespuesta.HasError)
                {
                    jsonResult = JsonConvert.SerializeObject(
                        new
                        {
                            status = "ok",
                            message = "Asignación Eliminada Correctamente"
                        });
                }
                else
                {
                    jsonResult = JsonConvert.SerializeObject(
                            new
                            {
                                status = "error",
                                message = "No se Puede Eliminar la Asiganación. Por favor, Intente Nuevamente"
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

        #endregion       
    }
}