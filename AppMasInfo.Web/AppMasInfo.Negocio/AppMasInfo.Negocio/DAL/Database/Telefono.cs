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
    
    public partial class Telefono
    {
        public int Id { get; set; }
        public string NumeroTelefono { get; set; }
        public string Tipo { get; set; }
        public long IdTutor { get; set; }
        public long IdPaciente { get; set; }
    
        public virtual Paciente Paciente { get; set; }
        public virtual Tutor Tutor { get; set; }
    }
}