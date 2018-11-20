using AppMasInfo.Negocio.DAL.Entities;
using AppMasInfo.Utils.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AppMasInfo.Web.Models
{
    public class PacienteViewModel
    {
        public PacienteViewModel()
        {
            this.LstPaciente = new BaseDto<List<PacienteDto>>(new List<PacienteDto>());
        }
        
        public BaseDto<List<PacienteDto>> LstPaciente
        {
            get;
            set;
        }

        public List<PacienteDto> lstPaciente
        {
            get;
            set;
        }

        public BaseDto<List<TutorDto>> LstTutor
        {
            get;
            set;
        }

        public List<TutorDto> lstTutor
        {
            get;
            set;
        }
    }

    public class PacienteCreateViewModel
    {
        #region Propiedades Paciente

        [Required(ErrorMessage = "Debe ingresar un Rut")]       
        [Display(Name = "Rut")]        
        public string Rut { get; set; }

        [Display(Name = "Nombre")]
        [Required(ErrorMessage = Validacion.Mensajes.Required)]
        [RegularExpression(Validacion.Patterns.Texto, ErrorMessage = Validacion.Mensajes.RegularExpression)]
        public string Nombre { get; set; }

        [Display(Name = "Apellido Paterno")]
        [Required(ErrorMessage = Validacion.Mensajes.Required)]
        [RegularExpression(Validacion.Patterns.Texto, ErrorMessage = Validacion.Mensajes.RegularExpression)]
        public string ApellidoPaterno { get; set; }

        [Display(Name = "Apellido Materno")]
        [Required(ErrorMessage = Validacion.Mensajes.Required)]
        [RegularExpression(Validacion.Patterns.Texto, ErrorMessage = Validacion.Mensajes.RegularExpression)]
        public string ApellidoMaterno { get; set; }

        [Display(Name = "Edad")]
        [Required(ErrorMessage = Validacion.Mensajes.Required)]
        [RegularExpression(Validacion.Patterns.Entero, ErrorMessage = Validacion.Mensajes.RegularExpression)]
        public int Edad { get; set; }

        [Display(Name = "Dirección")]
        [Required(ErrorMessage = Validacion.Mensajes.Required)]
        [RegularExpression(Validacion.Patterns.Alfanumerico, ErrorMessage = Validacion.Mensajes.RegularExpression)]
        public string Direccion { get; set; }

        [Display(Name = "Teléfono")]
        [Required(ErrorMessage = Validacion.Mensajes.Required)]       
        public string Telefono { get; set; }

        public string FiltroRut { get; set; }       
        #endregion

        #region Propiedades Tutor

        [Required(ErrorMessage = "Debe ingresar un Rut")]
        [Display(Name = "Rut")]
        public string RutTutor { get; set; }

        [Display(Name = "Nombre")]
        [Required(ErrorMessage = Validacion.Mensajes.Required)]
        [RegularExpression(Validacion.Patterns.Texto, ErrorMessage = Validacion.Mensajes.RegularExpression)]
        public string NombreTutor { get; set; }

        [Display(Name = "Apellido Paterno")]
        [Required(ErrorMessage = Validacion.Mensajes.Required)]
        [RegularExpression(Validacion.Patterns.Texto, ErrorMessage = Validacion.Mensajes.RegularExpression)]
        public string ApellidoPaternoTutor { get; set; }

        [Display(Name = "Apellido Materno")]
        [Required(ErrorMessage = Validacion.Mensajes.Required)]
        [RegularExpression(Validacion.Patterns.Texto, ErrorMessage = Validacion.Mensajes.RegularExpression)]
        public string ApellidoMaternoTutor { get; set; }

        [Display(Name = "Dirección")]
        [Required(ErrorMessage = Validacion.Mensajes.Required)]
        [RegularExpression(Validacion.Patterns.Alfanumerico, ErrorMessage = Validacion.Mensajes.RegularExpression)]
        public string DireccionTutor { get; set; }

        [Display(Name = "Telefono")]
        [Required(ErrorMessage = Validacion.Mensajes.Required)]
        [RegularExpression(Validacion.Patterns.Entero, ErrorMessage = Validacion.Mensajes.RegularExpression)]
        public string TelefonoTutor { get; set; }

        [Display(Name = "Email")]
        [Required(ErrorMessage = Validacion.Mensajes.Required)]
        [RegularExpression(Validacion.Patterns.Alfanumerico, ErrorMessage = Validacion.Mensajes.RegularExpression)]
        public string Email { get; set; }

        public long IdUsuario { get; set; }

        public long IdPaciente { get; set; }

        public long IdTutor { get; set; }

        [Display(Name = "Password")]
        [Required(ErrorMessage = "Debe Ingresar una Contraseña")]
        public string Pass { get; set; }

        [Display(Name = "Confirme Password")]
        public string ConfirmPass { get; set; }

        [Display(Name = "Usuario")]
        [Required(ErrorMessage = "Debe Ingresar un Nombre de Usuario")]
        public string Username { get; set; }
        
        [Required(ErrorMessage = "Debe Ingresar un Rol")]
        public int IdRol { get; set; }

        public List<RolDto> LstCRol { get; set; }

        public List<TipoTelefonoDto> LstTipoTelefono { get; set; }

        public int IdTipoTelefono { get; set; }

        #endregion

        public int IdEstado { get; set; }
           
        public int? FiltroIdEstado { get; set; }
    }

    public class PacienteEditViewModel
    {
        public long Id { get; set; }

        #region Propiedades Paciente
        
        [Display(Name = "Rut")]
        public string Rut { get; set; }

        [Display(Name = "Nombre")]
        public string Nombre { get; set; }

        [Display(Name = "Apellido Paterno")]
        public string ApellidoPaterno { get; set; }

        [Display(Name = "Apellido Materno")]        
        public string ApellidoMaterno { get; set; }

        [Display(Name = "Edad")]
        public int Edad { get; set; }

        [Display(Name = "Dirección")]  
        public string Direccion { get; set; }

        [Display(Name = "Teléfono")]       
        public string Telefono { get; set; }

        public string FiltroRut { get; set; }
        #endregion

        #region Propiedades Tutor

        public long IdTutor { get; set; }

        [Required(ErrorMessage = "Debe ingresar un Rut")]       
        public string RutTutor { get; set; }

        [Display(Name = "Nombre")]
        public string NombreTutor { get; set; }

        [Display(Name = "Apellido Paterno")]
        public string ApellidoPaternoTutor { get; set; }

        [Display(Name = "Apellido Materno")]  
        public string ApellidoMaternoTutor { get; set; }

        [Display(Name = "Dirección")]      
        public string DireccionTutor { get; set; }

        [Display(Name = "Telefono")]       
        public string TelefonoTutor { get; set; }

        [Display(Name = "Email")]        
        public string Email { get; set; }

        public long IdUsuario { get; set; }

        public long IdPaciente { get; set; }
       
        [Display(Name = "Password")]     
        public string Pass { get; set; }

        [Display(Name = "Confirme Password")]
        public string ConfirmPass { get; set; }

        [Display(Name = "Usuario")]       
        public string Username { get; set; }
      
        public int IdRol { get; set; }

        public List<RolDto> LstCRol { get; set; }

        public List<TipoTelefonoDto> LstTipoTelefono { get; set; }

        public long? FiltroIdUsuario { get; set; }

        public int IdTipoTelefono { get; set; }

        public int IdTelefono { get; set; }

        #endregion
    }

    public class PacienteDetailViewModel
    {
        public long IdTutor { get; set; }

        public int IdTelefono { get; set; }

        public int IdTipoTelefono { get; set; }

        public int IdRol { get; set; }

        public long IdUsuario { get; set; }

        public long IdPaciente { get; set; }

        public string NumeroTelefono { get; set; }

        public string DescricionTelefono { get; set; }

        [Display(Name = "Rut")]        
        public string RutPaciente { get; set; }

        [Display(Name = "Rut")]
        public string RutTutor { get; set; }

        [Display(Name = "Nombre")]        
        public string NombrePaciente { get; set; }

        [Display(Name = "Nombre")]
        public string NombreTutor { get; set; }

        [Display(Name = "Apellido Paterno")]        
        public string ApellidoPaternoPaciente { get; set; }

        [Display(Name = "Apellido Paterno")]
        public string ApellidoPaternoTutor { get; set; }

        [Display(Name = "Apellido Materno")]       
        public string ApellidoMaternoPaciente { get; set; }

        [Display(Name = "Apellido Materno")]
        public string ApellidoMaternoTutor { get; set; }

        [Display(Name = "Edad")]        
        public int Edad { get; set; }

        [Display(Name = "Dirección")]        
        public string DireccionPaciente { get; set; }

        [Display(Name = "Dirección")]
        public string DireccionTutor { get; set; }

        [Display(Name = "Username")]
        public string Username { get; set; }

        [Display(Name = "Telefono")]
        public string TelefonoTutor { get; set; }

        [Display(Name = "Fecha Inserción")]
        public Nullable<System.DateTime> FchCreatePaciente { get; set; }

        [Display(Name = "Fecha Inserción")]
        public Nullable<System.DateTime> FchCreateTutor { get; set; }

        [Display(Name = "Usuario Inserción")]
        public string UsrCreatePaciente { get; set; }

        [Display(Name = "Usuario Inserción")]
        public string UsrCreateTutor{ get; set; }

        [Display(Name = "Fecha Actualización")]
        public Nullable<System.DateTime> FchUpdatePaciente { get; set; }

        [Display(Name = "Fecha Actualización")]
        public Nullable<System.DateTime> FchUpdateTutor { get; set; }

        [Display(Name = "Usuario Actualización")]
        public string UsrUpdatePaciente{ get; set; }

        [Display(Name = "Usuario Actualización")]
        public string UsrUpdateTutor { get; set; }

        public int IdEstado { get; set; }

        public EstadoDto DetalleEstado { get; set; }
    }
}
