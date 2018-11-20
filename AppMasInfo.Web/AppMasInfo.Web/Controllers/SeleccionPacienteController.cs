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
    [Authorize]
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
        #endregion

        #region Metodos Publicos
        // GET: Paciente
        [HttpGet]
        public ActionResult Index()
        {
            //PacienteViewModel viewModel = new PacienteViewModel();
            SeleccionPacienteViewModel viewModel = new SeleccionPacienteViewModel();

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
                var pFiltroObj = new PacienteDto(p_ViewModel.FiltroPaginado.PaginaActual, p_ViewModel.FiltroPaginado.TamanoPagina);
                if (p_ViewModel.FiltroIdEstado != null) { pFiltroObj.FiltroIdEstado = p_ViewModel.FiltroIdEstado; } else { pFiltroObj.FiltroIdEstado = (int)EnumUtils.EstadoEnum.Paciente_Habilitado; }
                pFiltroObj.FiltroRut = p_ViewModel.FiltroRut;
                pFiltroObj.FiltroNombre = p_ViewModel.FiltroNombre;

                var pListDbResponse = this.PacienteServiceModel.GetListaPacienteByFitro(pFiltroObj);

                if (pListDbResponse.HasValue)
                    jsonResult = JsonConvert.SerializeObject(pListDbResponse);

                if (pListDbResponse.HasError)
                    throw pListDbResponse.Error;
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Ha ocurrido un error al obtener la lista de Grupos Contribuyentes. Por favor, inténtelo nuevamente";
            }

            return jsonResult;
        }



        #endregion
    }
}