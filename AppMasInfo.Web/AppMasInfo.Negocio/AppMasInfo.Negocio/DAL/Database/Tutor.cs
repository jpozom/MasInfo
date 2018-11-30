//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AppMasInfo.Negocio.DAL.Database
{
    using System;
    using System.Collections.Generic;
    
    public partial class Tutor
    {
        public long Id { get; set; }
        public string Rut { get; set; }
        public string Nombre { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        public string Direccion { get; set; }
        public Nullable<System.DateTime> FchCreate { get; set; }
        public string UsrCreate { get; set; }
        public Nullable<System.DateTime> FchUpdate { get; set; }
        public string UsrUpdate { get; set; }
        public long IdUsuario { get; set; }
        public int IdEstado { get; set; }
        public string Email { get; set; }
        public long IdPaciente { get; set; }
        public int Edad { get; set; }
    
        public virtual Estado Estado { get; set; }
        public virtual Paciente Paciente { get; set; }
        public virtual Usuario Usuario { get; set; }
    }
}
