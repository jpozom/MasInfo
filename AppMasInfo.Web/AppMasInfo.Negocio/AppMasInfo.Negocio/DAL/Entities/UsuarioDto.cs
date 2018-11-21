using AppMasInfo.Negocio.DAL.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppMasInfo.Negocio.DAL.Entities
{
    public class UsuarioDto : ObjectDto
    {
        #region contructores
        public UsuarioDto() : base() { }
        public UsuarioDto(int PaginaActual, int TamanoPagina) : base(PaginaActual, TamanoPagina) { }
        #endregion

        public long Id { get; set; }
        public string Username { get; set; }
        public string Pass { get; set; }
        public int IdRol { get; set; }
        public Nullable<bool> Habilitado { get; set; }

        public string FiltroUsername { get; set; }
        public int? FiltroIdRol { get; set; }

        public List<TrabajadorDto> ListaTrabajador { get; set; }

        public RolDto DetalleRol { get; set; }

    }
}
