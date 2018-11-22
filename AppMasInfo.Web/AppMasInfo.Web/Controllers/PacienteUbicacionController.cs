﻿using AppMasInfo.Negocio.DAL.Entities;
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
    public class PacienteUbicacionController : Controller
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
            PacienteUbicacionViewModel viewModel = new PacienteUbicacionViewModel();

            try
            {
                TempData.Clear();

                var filtroPaciente = new PacienteDto();
                filtroPaciente.FiltroId = p_Id;

                var oDb = this.PacienteUbicacionServiceModel.GetUbicacionPacienteByIdPaciente(filtroPaciente);
                var ids = new HashSet<int>();
                if (oDb.HasValue)
                {
                    viewModel.DetallePaciente = new PacienteDto();
                    BaseDto<PacienteDto> paciente = this.PacienteServiceModel.GetPacienteById(filtroPaciente);
                    if (paciente.HasValue)
                    {
                        viewModel.DetallePaciente = paciente.Value;
                    }
                    viewModel.LstPacienteUbicacion = oDb.Value;
                    ids = new HashSet<int>(oDb.Value.Select(x => x.IdUbicacion));
                }
                var tDb = this.UbicacionServiceModel.GetListaUbicacionAll().Value;
                viewModel.LstUbicaciones = new List<UbicacionDto>();
                viewModel.LstUbicaciones = tDb.Where(t => !ids.Contains(t.Id)).ToList();
                viewModel.IdUbicacion = 0;
                viewModel.IdPaciente = p_Id;
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

        [HttpPost]
        public ActionResult Edit(PacienteUbicacionViewModel viewModel)
        {
            try
            {
                TempData.Clear();

                //guardar asignacion
                PacienteUbicacionDto ubicacion = new PacienteUbicacionDto();
                ubicacion.IdPaciente = viewModel.IdPaciente;
                ubicacion.IdUbicacion = viewModel.IdUbicacion;
                ubicacion.FchIngreso = DateTime.Now;
                ubicacion.UsrIngreso = User.Identity.GetUserId();
                ubicacion.Observacion = viewModel.Observacion;

                BaseDto<bool> res = this.PacienteUbicacionServiceModel.AddUbicacionPaciente(ubicacion);

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

                var oDb = this.PacienteUbicacionServiceModel.GetUbicacionPacienteByIdPaciente(filtroPaciente);
                var ids = new HashSet<int>();
                if (oDb.HasValue)
                {
                    viewModel.DetallePaciente = new PacienteDto();
                    BaseDto<PacienteDto> paciente = this.PacienteServiceModel.GetPacienteById(filtroPaciente);
                    if (paciente.HasValue)
                    {
                        viewModel.DetallePaciente = paciente.Value;
                    }
                    viewModel.LstPacienteUbicacion = oDb.Value;
                    ids = new HashSet<int>(oDb.Value.Select(x => x.IdUbicacion));
                }
                var tDb = this.UbicacionServiceModel.GetListaUbicacionAll().Value;
                viewModel.LstUbicaciones = new List<UbicacionDto>();
                viewModel.LstUbicaciones = tDb.Where(t => !ids.Contains(t.Id)).ToList();
                viewModel.IdUbicacion = 0;
                viewModel.IdPaciente = viewModel.IdPaciente;
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
        #endregion

        #endregion
    }
}