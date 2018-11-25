using AppMasInfo.Negocio.DAL.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AppMasInfo.Web.Models
{
    public class CambioPassViewModel
    {        
        public string Pass { get; set; }

        public string ConfirmPass { get; set; }
       
        public string Username { get; set; }

        public string NombreTrabajador { get; set; }

        public string ApellidoPaternoTrabajador { get; set; }

        public string ApellidoMaternoTrabajador { get; set; }

        public long IdUsuario { get; set; }

        public long IdTrabajador { get; set; }

        public int IdRol { get; set; }

        public RolDto DetalleRol { get; set; }

    }
}