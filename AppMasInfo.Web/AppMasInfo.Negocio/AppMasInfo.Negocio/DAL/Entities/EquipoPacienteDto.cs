using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppMasInfo.Negocio.DAL.Entities
{
    public class EquipoPacienteDto
    {
        public long Id { get; set; }
        public long IdPaciente { get; set; }
        public long Idtrabajador { get; set; }
        public PacienteDto Paciente { get; set; }
        public TrabajadorDto Trabajador { get; set;  }
    }
}
