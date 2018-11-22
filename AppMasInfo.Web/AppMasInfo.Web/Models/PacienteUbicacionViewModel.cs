using AppMasInfo.Negocio.DAL.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AppMasInfo.Web.Models
{
    public class PacienteUbicacionViewModel
    {
        public PacienteDto DetallePaciente { get; set; }

        public long IdPaciente { get; set; }

        public int Id { get; set; }

        public string Observacion { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un trabajador")]
        public int IdUbicacion { get; set; }

        public List<PacienteUbicacionDto> LstPacienteUbicacion { get; set; }

        public List<UbicacionDto> LstUbicaciones { get; set; }
    }
}