using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppMasInfo.Negocio.DAL.Entities
{
    public class TrabajadorDto
    {
        public long Id { get; set; }
        public string Nombre { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        public Nullable<int> IdCargo { get; set; }
        public Nullable<System.DateTime> FchCreate { get; set; }
        public string UsrCreate { get; set; }
        public Nullable<System.DateTime> FchUpdate { get; set; }
        public string UsrUpdate { get; set; }
        public string Email { get; set; }
        public int IdEstado { get; set; }
        public long? IdUsuario { get; set; }
        public Nullable<int> IdCargoFuncion { get; set; }

        public UsuarioDto DatosUsuario { get; set; }
        
        public int? FiltroIdEstado { get; set; }
        public long? FiltroIdUsuario { get; set; }
        public int? FiltroIdRol { get; set; }
        public long? FiltroId { get; set; }

        public EstadoDto DetalleEstado { get; set; }
        public UsuarioDto DetalleUsuario { get; set; }
        public RolDto DetalleRol { get; set; }
                 
        public List<UsuarioDto> ListaUsuarios { get; set; }
    }
}
