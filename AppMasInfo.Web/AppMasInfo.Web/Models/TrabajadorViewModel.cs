using AppMasInfo.Negocio.DAL.Entities;
using AppMasInfo.Utils.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AppMasInfo.Web.Models
{
    public class TrabajadorViewModel
    {
        public TrabajadorViewModel()
        {
            this.LstTrabajador = new BaseDto<List<TrabajadorDto>>(new List<TrabajadorDto>());
        }

        public BaseDto<List<TrabajadorDto>> LstTrabajador { get; set; }

        public List<TrabajadorDto> LstTrabajadores { get; set; }

        public List<UsuarioDto> LstUsuario { get; set; }
       
        public UsuarioDto DatosUsuario { get; set; }
      
        public List<RolDto> LstRoles { get; set; }
       
        public RolDto DetalleRol { get; set; }

        public PaginadorDto FiltroPaginado { get; set; }

        public long FiltroIdUsuario { get; set; }
       
        public int? FiltroIdEstado { get; set; }
        
        public int? FiltroIdRol { get; set; }

        public string FiltroNombre { get; set; }
    }

    public class TrabajadorCreateViewModel
    {
        #region propiedades

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

        //[Required(ErrorMessage = Validacion.Mensajes.Required)]
        //[Display(Name = "Ingrese Username")]
        //public string Username { get; set; }

        //[Required(ErrorMessage = Validacion.Mensajes.Required)]
        //public string Pass { get; set; }
        
        public string ConfirmPass { get; set; }
       
        [Display(Name = "Ingrese Email")]
        [Required(ErrorMessage = Validacion.Mensajes.Required)]
        [RegularExpression(Validacion.Patterns.Alfanumerico, ErrorMessage = Validacion.Mensajes.RegularExpression)]
        public string Email { get; set; }
       
        public int? IdRol { get; set; }
        
        public int? IdCargoFuncion { get; set; }

        public int? IdCargo { get; set; }

        public long? IdUsuario { get; set; }        

        public int IdEstado { get; set; }

        public UsuarioDto DatosUsuario { get; set; }

        public List<RolDto> LstCRol { get; set; }

        public List<CargoDto> LstCargo { get; set; }

        public List<CargoFuncionDto> LstCargoFuncion { get; set; }

        #endregion              
    }

    public class TrabajadorEditViewModel
    {
        public long Id { get; set; }

        [Display(Name = "Nombre")]        
        public string Nombre { get; set; }

        [Display(Name = "Apellido Paterno")]        
        public string ApellidoPaterno { get; set; }

        [Display(Name = "Apellido Materno")]        
        public string ApellidoMaterno { get; set; }

        [Display(Name = "Ingrese Username")]        
        public string Username { get; set; }

        [Display(Name = "Ingrese Password")]       
        public string Pass { get; set; }

        [Display(Name = "Ingrese Email")]        
        public string Email { get; set; }

        [Display(Name = "Ingrese Cargo")]       
        public int? IdCargo { get; set; }
        
        public int? IdCargoFuncion { get; set; }

        public long? IdUsuario { get; set; }

        public int IdEstado { get; set; }

        public int? IdRol { get; set; }

        public List<RolDto> LstRol { get; set; }

        public List<CargoDto> LstCargo { get; set; }

        public List<CargoFuncionDto> LstCargoFuncion { get; set; }
       
        public string ConfirmPass { get; set; }
        
        public UsuarioDto DatosUsuario { get; set; }
    }

    public class TrabajadorDetailViewModel
    {
        public long Id { get; set; }

        [Display(Name = "Nombre Usuario")]
        public string Nombre { get; set; }

        [Display(Name = "Apellido Paterno")]
        public string ApellidoPaterno { get; set; }

        [Display(Name = "Apellido Materno")]
        public string ApellidoMaterno { get; set; }

        [Display(Name = "Username")]
        public string Username { get; set; }
        
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Fecha Inserción")]
        public Nullable<System.DateTime> FchCreate { get; set; }

        [Display(Name = "Usuario Inserción")]
        public string UsrCreate { get; set; }

        [Display(Name = "Fecha Actualización")]
        public Nullable<System.DateTime> FchUpdate { get; set; }

        [Display(Name = "Usuario Actualización")]
        public string UsrUpdate { get; set; }

        [Display(Name = "Estado")]
        public int IdEstado { get; set; }

        [Display(Name = "Rol")]
        public int IdRol { get; set; }

        [Display(Name = "Cargo")]
        public int Cargo { get; set; }

        [Display(Name = "Funcion")]
        public int Funcion { get; set; }

        public UsuarioDto DatosUsuario { get; set; }

        public EstadoDto DetalleEstado { get; set; }

        public CargoDto DetalleCargo { get; set; }

        public CargoFuncionDto DetalleFuncion { get; set; }

        public RolDto DetalleRol { get; set; }

    }
}