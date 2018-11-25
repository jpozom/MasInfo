using AppMasInfo.Negocio.DAL.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppMasInfo.Negocio.DAL.Entities
{
    public class TutorDto
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

        public int? FiltroIdEstado { get; set; }
        public string FiltroRut { get; set; }
        public long? FiltroIdPaciente { get; set; }
        public long? FiltroId { get; set; }
        public long? FiltroIdUsuario { get; set; }

        public EstadoDto DetalleEstado { get; set; }
        public UsuarioDto DatosUsuario { get; set; }
        public RolDto DetalleRol { get; set; }
        public TelefonoDto DetalleTelefono { get; set; }
        public PacienteDto DetallePaciente { get; set; }
        public TipoTelefonoDto DetalleTipoTelefono { get; set; }
        public PacienteUbicacionDto DetallePacienteUbicacion { get; set; }
        public UbicacionDto DetalleUbicacion { get; set; }
    }
}
