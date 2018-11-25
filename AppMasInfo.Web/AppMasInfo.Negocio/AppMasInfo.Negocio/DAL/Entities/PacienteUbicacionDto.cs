using AppMasInfo.Negocio.DAL.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppMasInfo.Negocio.DAL.Entities
{
    public class PacienteUbicacionDto
    {
        public int Id { get; set; }
        public long IdPaciente { get; set; }
        public int IdUbicacion { get; set; }
        public Nullable<System.DateTime> FchIngreso { get; set; }
        public string UsrIngreso { get; set; }        
        public string Observacion { get; set; }
        public Nullable<bool> Habilitado { get; set; }

        public PacienteDto DetallePaciente { get; set; }
        public UbicacionDto DetalleUbicacion { get; set; }
    }
}
