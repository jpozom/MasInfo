using AppMasInfo.Negocio.DAL.Entities;
using AppMasInfo.Negocio.DAL.Services;
using AppMasInfo.Utils.Utils;
using AppMasInfo.Web.Models;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace AppMasInfo.Web.Controllers
{
    //Describe cómo usar el atributo Authorize para controlar el acceso a las paginas y a sus metodos.
    [Authorize(Roles = "Administrador, Médico, Enfermero/a, Técnico, Auxiliar")]
    public class SeleccionPacienteController : Controller
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

        private ITrabajadorService TrabajadorServiceModel
        {
            get
            {
                return TrabajadorService.GetInstance();
            }
        }

        private IEquipoPacienteService EquipoPacienteServiceModel
        {
            get
            {
                return EquipoPacienteService.GetInstance();
            }
        }

        private IPacienteUbicacionService PacienteUbicacionServiceModel
        {
            get
            {
                return PacienteUbicacionService.GetInstance();
            }
        }

        private IUbicacionService UbicacionServiceModel
        {
            get
            {
                return UbicacionService.GetInstance();
            }
        }
        #endregion

        #region Metodos Publicos

        #region Index
        // GET: Paciente
        [HttpGet]
        public ActionResult Index()
        {
            SeleccionPacienteViewModel viewModel = new SeleccionPacienteViewModel();

            try
            {
                var lstPaciente = this.PacienteServiceModel.GetListaPacienteAll();
                viewModel.LstPaciente.Value = lstPaciente.HasValue ? lstPaciente.Value : new List<PacienteDto>();

                //se crea un objeto
                var pacienteFiltroObj = new PacienteDto(1, 10);
                pacienteFiltroObj.FiltroIdEstado = (int)EnumUtils.EstadoEnum.Paciente_Habilitado;
                //se envia el objeto al servicio
                var pacienteListDbResponse = this.PacienteServiceModel.GetListaPacienteByFitro(pacienteFiltroObj);

                if (!pacienteListDbResponse.HasError)
                {
                    viewModel.LstPaciente = pacienteListDbResponse;
                }

                if (pacienteListDbResponse.HasError)
                {
                    TempData["ErrorMessage"] = "Ha ocurrido un error al obtener la lista de Grupos de Trabajo. Por favor, inténtelo nuevamente";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Ha ocurrido un error al obtener la lista de pacientes. Por favor, inténtelo nuevamente " + ex;
            }

            return View(viewModel);
        }

        [HttpPost]
        public string Index(SeleccionPacienteViewModel p_ViewModel)
        {
            string jsonResult = string.Empty;

            try
            {
                var pacienteFiltroObj = new PacienteDto(p_ViewModel.FiltroPaginado.PaginaActual, p_ViewModel.FiltroPaginado.TamanoPagina);
                pacienteFiltroObj.FiltroIdEstado = (int)EnumUtils.EstadoEnum.Paciente_Habilitado;
                pacienteFiltroObj.FiltroId = p_ViewModel.FiltroRut;
                pacienteFiltroObj.FiltroNombre = p_ViewModel.FiltroNombre;

                var pListDbResponse = this.PacienteServiceModel.GetListaPacienteByFitro(pacienteFiltroObj);

                if (pListDbResponse.HasValue)
                    jsonResult = JsonConvert.SerializeObject(pListDbResponse);

                if (pListDbResponse.HasError)
                    throw pListDbResponse.Error;
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Ha ocurrido un error al obtener la lista de Pacientes. Por favor, inténtelo nuevamente " + ex;
            }

            return jsonResult;
        }
        #endregion

        #region Detail
        [HttpGet]
        public ActionResult Detail(long p_Id)
        {
            SeleccionPacienteDetailViewModel viewModel = new SeleccionPacienteDetailViewModel();

            try
            {
                PacienteDto filtroPaciente = new PacienteDto();
                filtroPaciente.FiltroId = p_Id;

                var objRespuestaDb = PacienteServiceModel.GetPacienteById(filtroPaciente);

                if (objRespuestaDb.HasValue)
                {
                    TutorDto filtroTutor = new TutorDto();
                    filtroTutor.FiltroId = objRespuestaDb.Value.DetalleTutor.Id;

                    var objRespDb = TutorServiceModel.GetTutorById(filtroTutor);

                    if (objRespDb.HasValue)
                    {
                        viewModel = new SeleccionPacienteDetailViewModel
                        {
                            RutPaciente = objRespuestaDb.Value.Rut,
                            IdPaciente = objRespuestaDb.Value.Id,
                            NombrePaciente = objRespuestaDb.Value.Nombre,
                            ApellidoPaternoPaciente = objRespuestaDb.Value.ApellidoPaterno,
                            ApellidoMaternoPaciente = objRespuestaDb.Value.ApellidoMaterno,
                            Edad = objRespuestaDb.Value.Edad,
                            NumeroTelefono = objRespuestaDb.Value.NumeroTelefono,
                            DireccionPaciente = objRespuestaDb.Value.Direccion,
                            DetalleTutor = objRespuestaDb.Value.DetalleTutor,
                            NumeroTelefonoTutor = objRespDb.Value.DetalleTelefono.NumeroTelefono,
                            IdPacienteUbicacion = objRespuestaDb.Value.DetallePacienteUbicacion.Id,
                            Observacion = objRespuestaDb.Value.DetallePacienteUbicacion.Observacion,
                            FchIngreso = objRespuestaDb.Value.DetallePacienteUbicacion.FchIngreso,
                            DetalleUbicacion = objRespuestaDb.Value.DetalleUbicacion,
                            DetalleEquipoPaciente = objRespuestaDb.Value.DetalleEquipoPaciente,
                        };
                        //Deshabilitar la opcion de modificar la observacion ingresada al paciente, esta opcion
                        //queda solo habilitada para roles asignados
                        var RolActual = (((System.Security.Claims.ClaimsIdentity)User.Identity).Claims.Where(c => c.Type == System.Security.Claims.ClaimTypes.Role).Select(c => c.Value).FirstOrDefault());

                        if (RolActual == "Auxiliar")
                        {
                            viewModel.Disabled = true;
                        }
                        else
                        {
                            viewModel.Disabled = false;
                        }
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Ha ocurrido un error al obtener los datos. Por favor, inténtelo nuevamente";
                    }
                }
                else
                {
                    TempData["ErrorMessage"] = "Ha ocurrido un error al obtener los datos. Por favor, inténtelo nuevamente";
                }

                var filtroPac = new PacienteDto();
                filtroPac.FiltroId = p_Id;

                var oDb = this.EquipoPacienteServiceModel.GetEquipoPacienteByIdPaciente(filtroPac);
                var ids = new HashSet<long>();
                if (oDb.HasValue)
                {
                    viewModel.Paciente = new PacienteDto();
                    BaseDto<PacienteDto> paciente = this.PacienteServiceModel.GetPacienteById(filtroPac);
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
                viewModel.IdPaciente = p_Id;
                if (oDb.HasError)
                    throw oDb.Error;

                var filtroP = new PacienteDto();
                filtroP.FiltroId = p_Id;

                var poDb = this.PacienteUbicacionServiceModel.GetUbicacionPacienteByIdPaciente(filtroP);
                var pids = new HashSet<int>();
                if (poDb.HasValue)
                {
                    viewModel.DetallePaciente = new PacienteDto();
                    BaseDto<PacienteDto> paciente = this.PacienteServiceModel.GetPacienteById(filtroP);
                    if (paciente.HasValue)
                    {
                        viewModel.DetallePaciente = paciente.Value;
                    }
                    viewModel.LstPacienteUbicacion = poDb.Value;
                    pids = new HashSet<int>(poDb.Value.Select(x => x.IdUbicacion));
                }
                var ptDb = this.UbicacionServiceModel.GetListaUbicacionAll().Value;
                viewModel.LstUbicaciones = new List<UbicacionDto>();
                viewModel.LstUbicaciones = ptDb.Where(t => !pids.Contains(t.Id)).ToList();
                viewModel.IdUbicacion = 0;
                viewModel.IdPaciente = p_Id;
                if (poDb.HasError)
                    throw oDb.Error;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Detail(SeleccionPacienteDetailViewModel p_ViewModel)
        {
            try
            {
                // Creamos un objeto para almacenar los datos de la tabla PacienteUbicacion modificados por el usuario
                PacienteUbicacionDto objActualizarDB = new PacienteUbicacionDto();
                objActualizarDB.Id = p_ViewModel.IdPacienteUbicacion;
                objActualizarDB.Observacion = p_ViewModel.Observacion;

                // Actualizamos los datos en base de datos
                var actualizaResultObj = PacienteUbicacionServiceModel.UpdatePacienteUbicacion(objActualizarDB);

                if (actualizaResultObj.HasValue)
                {
                    TempData["SaveOkMessage"] = "Observación Actualizada Correctamente";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["ErrorMessage"] = "Ha ocurrido un Error al Obtener Datos. Por favor, Inténtelo Nuevamente";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Ha ocurrido un Error al Obtener Datos. Por favor, Inténtelo Nuevamente" + ex;
            }

            return View(p_ViewModel);
        }
        #endregion

        #endregion
    }
}