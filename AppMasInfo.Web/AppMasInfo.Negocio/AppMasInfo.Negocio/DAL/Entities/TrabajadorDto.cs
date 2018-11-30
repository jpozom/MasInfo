using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppMasInfo.Negocio.DAL.Entities
{
    public class TrabajadorDto : ObjectDto
    {
        #region contructores
        public TrabajadorDto() : base() { }
        public TrabajadorDto(int PaginaActual, int TamanoPagina) : base(PaginaActual, TamanoPagina) { }
        #endregion

        public long Id { get; set; }
        public string Rut { get; set; }
        public string Nombre { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }       
        public string Email { get; set; }
        public Nullable<System.DateTime> FchCreate { get; set; }
        public string UsrCreate { get; set; }
        public Nullable<System.DateTime> FchUpdate { get; set; }
        public string UsrUpdate { get; set; }      
        public int? IdCargoFuncion { get; set; }
        public int? IdCargo { get; set; }
        public int IdEstado { get; set; }
        public long IdUsuario { get; set; }
                      
        public int? FiltroIdEstado { get; set; }
        public long? FiltroIdUsuario { get; set; }
        public int? FiltroIdRol { get; set; }
        public long? FiltroId { get; set; }
        public string FiltroNombre { get; set; }
        public string FiltroUsername{ get; set; }
        public string FiltroRut { get; set; }

        public UsuarioDto DatosUsuario { get; set; }
        public EstadoDto DetalleEstado { get; set; }
        public UsuarioDto DetalleUsuario { get; set; }
        public RolDto DetalleRol { get; set; }
        public CargoDto DetalleCargo { get; set; }
        public CargoFuncionDto DetalleFuncion { get; set; }
        public TelefonoDto DetalleTelefono { get; set; }

        public List<UsuarioDto> ListaUsuarios { get; set; }

        public string NombreCompleto {
            get { return Nombre + " " + ApellidoPaterno + " " + ApellidoMaterno; }
        }

        public string NombreCompletoCargo
        {
            get
            {
                // se crea arreglo que se llena con las priopiedades que se asignaron en la obtencion de la lista completa de trabajador                 
                string nombreCompletoCargo = string.Empty;
                nombreCompletoCargo = String.Format("{0} {1} {2} - {3}", Nombre, ApellidoPaterno, ApellidoMaterno,
                    (DetalleCargo != null ? DetalleCargo.Descripcion : ""));

                return nombreCompletoCargo;
            }
        }
    }
}
