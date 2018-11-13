using AppMasInfo.Negocio.DAL.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppMasInfo.Negocio.DAL.Entities
{
    public class UsuarioDto
    {
        public long Id { get; set; }
        public string Username { get; set; }
        public string Pass { get; set; }
        public int IdRol { get; set; }

        public string FiltroUsername { get; set; }

        public List<TrabajadorDto> ListaTrabajador { get; set; }

        public RolDto DetalleRol { get; set; }

    }
}
