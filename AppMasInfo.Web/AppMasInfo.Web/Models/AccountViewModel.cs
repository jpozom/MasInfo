using AppMasInfo.Negocio.DAL.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AppMasInfo.Web.Models
{
    public class AccountViewModel
    {
        [Required(ErrorMessage = "Debe ingresar el nombre de usuario")]
        [Display(Name = "Usuario Otorgado")]
        public string Username
        {
            get;
            set;
        }

        [Required(ErrorMessage = "Debe ingresar la contraseña")]
        [Display(Name = "Contraseña")]
        public string Pass
        {
            get;
            set;
        }

        public RolDto DetalleRol { get; set; }
    }
}