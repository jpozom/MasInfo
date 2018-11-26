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

        #region Index

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
                        viewModel.IdPaciente = tutorDB.Value.DetallePaciente.Id;
                        viewModel.DetalleUbicacion = tutorDB.Value.DetalleUbicacion;
                        if (tutorDB.Value.DetallePacienteUbicacion.Habilitado == true)
                        {
                            viewModel.Observacion = tutorDB.Value.DetallePacienteUbicacion.Observacion;
                            viewModel.FchIngreso = tutorDB.Value.DetallePacienteUbicacion.FchIngreso;
                        }

                        var filtroP = new PacienteDto();
                        filtroP.FiltroId = tutorDB.Value.DetallePaciente.Id;

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
                        viewModel.IdPaciente = tutorDB.Value.DetallePaciente.Id;
                        if (poDb.HasError)
                            throw poDb.Error;
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

        #endregion

        #region GetEquipoTrabajo
        [HttpGet]
        public ActionResult EquipoTrabajo(long p_Id)
        {
            SeleccionPacienteDetailViewModel viewModel = new SeleccionPacienteDetailViewModel();

            try
            {
                PacienteDto filtroPaciente = new PacienteDto();
                filtroPaciente.FiltroId = p_Id;

                var objRespuestaDb = PacienteServiceModel.GetPacienteById(filtroPaciente);

                if (objRespuestaDb.HasValue)
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
                        IdPacienteUbicacion = objRespuestaDb.Value.DetallePacienteUbicacion.Id,
                        Observacion = objRespuestaDb.Value.DetallePacienteUbicacion.Observacion,
                        FchIngreso = objRespuestaDb.Value.DetallePacienteUbicacion.FchIngreso,
                        DetalleUbicacion = objRespuestaDb.Value.DetalleUbicacion,
                        DetalleEquipoPaciente = objRespuestaDb.Value.DetalleEquipoPaciente,

                    };
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
            }

            catch (Exception ex)
            {
                throw ex;
            }

            return View(viewModel);
        }
        #endregion
    }
}