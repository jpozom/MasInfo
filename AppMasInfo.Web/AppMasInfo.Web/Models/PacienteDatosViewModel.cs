using AppMasInfo.Negocio.DAL.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AppMasInfo.Web.Models
{
    public class PacienteDatosViewModel
    {
        #region Propiedades Usuario
        public string Username { get; set; }

        #endregion

        #region Propiedades Paciente

        public string RutPaciente { get; set; }

        public long IdPaciente { get; set; }

        public string NombrePaciente { get; set; }

        public string ApellidoPaternoPaciente { get; set; }

        public string ApellidoMaternoPaciente { get; set; }

        public int Edad { get; set; }

        public string DireccionPaciente { get; set; }

        public string TelefonoPaciente { get; set; }

        public string Observacion { get; set; }

        [Display(Name = "Número teléfono")]
        public string NumeroTelefono { get; set; }

        public Nullable<System.DateTime> FchIngreso { get; set; }

        public int IdPacienteUbicacion { get; set; }

        public int IdUbicacion { get; set; }

        #endregion

        #region Propiedades Tutor

        public string RutTutor { get; set; }

        public string NombreTutor { get; set; }
       
        public string ApellidoPaternoTutor { get; set; }
        
        public string ApellidoMaternoTutor { get; set; }
      
        public string DireccionTutor { get; set; }
       
        public string TelefonoTutor { get; set; }
        
        public string EmailTutor { get; set; }
        #endregion

        #region Objetos

        public EstadoDto DetalleEstado { get; set; }
        public UsuarioDto DatosUsuario { get; set; }
        public RolDto DetalleRol { get; set; }
        public TelefonoDto DetalleTelefono { get; set; }
        public PacienteDto DetallePaciente { get; set; }
        public TipoTelefonoDto DetalleTipoTelefono { get; set; }
        public PacienteUbicacionDto DetallePacienteUbicacion { get; set; }
        public UbicacionDto DetalleUbicacion { get; set; }
        public TutorDto DetalleTutor { get; set; }
        public EquipoPacienteDto DetalleEquipoPaciente { get; set; }

        #endregion

        #region Listas
        public List<PacienteUbicacionDto> LstPacienteUbicacion { get; set; }
        public List<UbicacionDto> LstUbicaciones { get; set; }
        #endregion
    }
}