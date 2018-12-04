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

        public string FechaHora
        {
            get
            {
                // se crea arreglo que se llena con las priopiedades que se asignaron en la obtencion de la lista completa de trabajador                 
                string fechaHora = string.Empty;
                fechaHora = String.Format("{0} / {1} / {2} - {3} : {4} hrs.", FchIngreso.Value.Day, FchIngreso.Value.Month, FchIngreso.Value.Year, FchIngreso.Value.Hour, FchIngreso.Value.Minute);

                return fechaHora;
            }
        }
    }
}
