using AppMasInfo.Negocio.DAL.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppMasInfo.Negocio.DAL.Entities
{
    public class TelefonoDto
    {
        public int Id { get; set; }
        public string NumeroTelefono { get; set; }
        public string Tipo { get; set; }
        public long IdTutor { get; set; }
        public long IdPaciente { get; set; }
    }
}
