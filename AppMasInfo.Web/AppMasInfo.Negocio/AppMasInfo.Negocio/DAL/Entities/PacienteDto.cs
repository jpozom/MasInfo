using AppMasInfo.Negocio.DAL.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppMasInfo.Negocio.DAL.Entities
{
    public class PacienteDto
    {
        public long Id { get; set; }
        public string Rut { get; set; }
        public string Nombre { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        public int Edad { get; set; }
        public string Direccion { get; set; }        
        public Nullable<System.DateTime> FchCreate { get; set; }
        public string UsrCreate { get; set; }
        public Nullable<System.DateTime> FchUpdate { get; set; }
        public string UsrUpdate { get; set; }
        public int IdEstado { get; set; }
        public string NumeroTelefono { get; set; }

        public int? FiltroIdEstado { get; set; }
        public long? FiltroId { get; set; }
        public string FiltroRut { get; set; }
        public EstadoDto DetalleEstado { get; set; }

    }
}
