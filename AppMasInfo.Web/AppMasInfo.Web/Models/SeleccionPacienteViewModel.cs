﻿using AppMasInfo.Negocio.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AppMasInfo.Web.Models
{
    public class SeleccionPacienteViewModel
    {
        public SeleccionPacienteViewModel()
        {
            this.LstPaciente = new BaseDto<List<PacienteDto>>(new List<PacienteDto>());
        }
        public BaseDto<List<PacienteDto>> LstPaciente { get; set; }

        public List<PacienteDto> lstPaciente { get; set; } //Este no deberia llenar

        public BaseDto<List<TutorDto>> LstTutor { get; set; }

        public List<TutorDto> lstTutor { get; set; }

        public String FiltroRut { get; set; }

        public string FiltroNombre { get; set; }

        public int? FiltroIdEstado { get; set; }

        public PaginadorDto FiltroPaginado { get; set; }

    }


}