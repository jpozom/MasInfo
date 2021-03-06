﻿using AppMasInfo.Negocio.DAL.Entities;
using AppMasInfo.Utils.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AppMasInfo.Web.Models
{
    public class EquipoPacienteViewModel
    { 
        public PacienteDto Paciente { get; set; }

        public long IdPaciente { get; set; }

        public long Id { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un trabajador")]
        public long Idtrabajador { get; set; }

        public List<EquipoPacienteDto> Equipo { get; set; }

        public List<TrabajadorDto> LstTrabajadores { get; set; }


    }


}
