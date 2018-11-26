using AppMasInfo.Negocio.DAL.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AppMasInfo.Web.Models
{
    public class SeleccionPacienteViewModel
    {
        public SeleccionPacienteViewModel()
        {
            this.ListaPacientes = new BaseDto<List<PacienteDto>>(new List<PacienteDto>());
        }

        public BaseDto<List<PacienteDto>> ListaPacientes { get; set; }

        public List<PacienteDto> lstPaciente { get; set; } 
        
        public List<TutorDto> LstTutor { get; set; }

        public long FiltroIdRut { get; set; }

        public string FiltroNombre { get; set; }

        public int? FiltroIdEstado { get; set; }

        public PaginadorDto FiltroPaginado { get; set; }

        public long IdPaciente { get; set; }
    }

    public class SeleccionPacienteDetailViewModel
    {
        public long Id { get; set; }       

        public int IdPacienteUbicacion { get; set; }

        public long IdUsuario { get; set; }

        public long IdPaciente { get; set; }

        public bool Disabled { get; set; }

        [Display(Name = "Número teléfono")]
        public string NumeroTelefono { get; set; }

        [Display(Name = "Rut")]
        public string RutPaciente { get; set; }

        [Display(Name = "Nombre")]
        public string NombrePaciente { get; set; }

        [Display(Name = "Apellido Paterno")]
        public string ApellidoPaternoPaciente { get; set; }

        [Display(Name = "Apellido Materno")]
        public string ApellidoMaternoPaciente { get; set; }

        [Display(Name = "Edad")]
        public int Edad { get; set; }

        [Display(Name = "Dirección")]
        public string DireccionPaciente { get; set; }      
      
        public string Observacion { get; set; }

        [Display(Name = "Fecha Ingreso Paciente a la Ubicación")]
        public Nullable<System.DateTime> FchIngreso { get; set; }

        public PacienteUbicacionDto DetallePacienteUbicacion { get; set; }
        public UbicacionDto DetalleUbicacion { get; set; }
        public TutorDto DetalleTutor { get; set; }
        public EquipoPacienteDto DetalleEquipoPaciente { get; set; }        
        public PacienteDto Paciente { get; set; }
        public PacienteDto DetallePaciente { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un trabajador")]
        public long Idtrabajador { get; set; }

        [Required(ErrorMessage = "Debe Seleccionar una Ubicación")]
        public int IdUbicacion { get; set; }

        public List<EquipoPacienteDto> Equipo { get; set; }

        public List<TrabajadorDto> LstTrabajadores { get; set; }

        public List<UbicacionDto> LstUbicaciones { get; set; }

        public List<PacienteUbicacionDto> LstPacienteUbicacion { get; set; }       
    }

}