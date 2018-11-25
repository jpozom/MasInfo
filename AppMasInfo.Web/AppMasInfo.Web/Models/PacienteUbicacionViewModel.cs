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
        public long IdPaciente { get; set; }

        public int Id { get; set; }

        [Required(ErrorMessage = "Debe Ingresar una Observación del Estado Actual del Paciente")]
        public string Observacion { get; set; }

        [Required(ErrorMessage = "Debe Seleccionar una Ubicación")]
        public int IdUbicacion { get; set; }

        public List<PacienteUbicacionDto> LstPacienteUbicacion { get; set; }

        public List<UbicacionDto> LstUbicaciones { get; set; }

        public PacienteDto DetallePaciente { get; set; }

        public UbicacionDto DetalleUbicacion { get; set; }
    }
}