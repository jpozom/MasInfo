using AppMasInfo.Negocio.DAL.Entities;
using AppMasInfo.Utils.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AppMasInfo.Web.Models
{
    public class TutorViewModel
    {
        public TutorViewModel()
        {
            this.LstTutor = new BaseDto<List<TutorDto>>(new List<TutorDto>());
        }

        public BaseDto<List<TutorDto>> LstTutor
        {
            get;
            set;
        }        
    }

    public class TutorCreateViewModel
    {
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

        [Display(Name = "Dirección")]
        [Required(ErrorMessage = Validacion.Mensajes.Required)]
        [RegularExpression(Validacion.Patterns.Alfanumerico, ErrorMessage = Validacion.Mensajes.RegularExpression)]
        public string Direccion { get; set; }

        [Display(Name = "Telefono")]
        [Required(ErrorMessage = Validacion.Mensajes.Required)]
        [RegularExpression(Validacion.Patterns.Entero, ErrorMessage = Validacion.Mensajes.RegularExpression)]
        public int Telefono { get; set; }

        [Display(Name = "Edad")]
        [Required(ErrorMessage = Validacion.Mensajes.Required)]
        [RegularExpression(Validacion.Patterns.Entero, ErrorMessage = Validacion.Mensajes.RegularExpression)]
        public int Email { get; set; }
                
        public int IdEstado { get; set; }

        public long IdUsuario { get; set; }

        public long IdPaciente { get; set; }
    }

    public class TutorEditViewModel
    {
        public long Id { get; set; }

        [Display(Name = "Rut")]
        [Required(ErrorMessage = Validacion.Mensajes.Required)]
        [RegularExpression(Validacion.Patterns.Alfanumerico, ErrorMessage = Validacion.Mensajes.RegularExpression)]
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

        [Display(Name = "Telefono")]
        [Required(ErrorMessage = Validacion.Mensajes.Required)]
        [RegularExpression(Validacion.Patterns.Alfanumerico, ErrorMessage = Validacion.Mensajes.RegularExpression)]
        public int Telefono { get; set; }

        public long? IdTutor { get; set; }

        public int IdEstado { get; set; }
    }

    public class TutorDetailViewModel
    {
        public long IdTutor { get; set; }

        public int IdTelefono { get; set; }

        public int IdTipoTelefono { get; set; }

        public int IdRol { get; set; }

        public long IdUsuario { get; set; }

        public long IdPaciente { get; set; }

        public string NumeroTelefono { get; set; }

        public string DescripcionTelefono { get; set; }       

        [Display(Name = "Rut")]
        public string RutTutor { get; set; }       

        [Display(Name = "Nombre")]
        public string NombreTutor { get; set; }        

        [Display(Name = "Apellido Paterno")]
        public string ApellidoPaternoTutor { get; set; }

        [Display(Name = "Apellido Materno")]
        public string ApellidoMaternoTutor { get; set; }

        [Display(Name = "Edad")]
        public int Edad { get; set; }

        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Dirección")]
        public string DireccionTutor { get; set; }

        [Display(Name = "Username")]
        public string Username { get; set; }

        [Display(Name = "Telefono")]
        public string TelefonoTutor { get; set; }

        [Display(Name = "Fecha Inserción")]
        public Nullable<System.DateTime> FchCreateTutor { get; set; }      

        [Display(Name = "Usuario Inserción")]
        public string UsrCreateTutor { get; set; }

        [Display(Name = "Fecha Actualización")]
        public Nullable<System.DateTime> FchUpdateTutor { get; set; }      

        [Display(Name = "Usuario Actualización")]
        public string UsrUpdateTutor { get; set; }

        [Display(Name = "Estado")]
        public int IdEstado { get; set; }       

        public EstadoDto DetalleEstado { get; set; }
        public UsuarioDto DatosUsuario { get; set; }
        public RolDto DetalleRol { get; set; }
        public TelefonoDto DetalleTelefono { get; set; }
        public PacienteDto DetallePaciente { get; set; }
        public TipoTelefonoDto DetalleTipoTelefono { get; set; }
    }
}