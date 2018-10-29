using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppMasInfo.Negocio.DAL.Entities
{
    [Serializable]
    public class PaginadorDto
    {
        #region constructores
        public PaginadorDto()
        {
        }

        public PaginadorDto(int PaginaActual, int TamanoPagina)
        {
            this.PaginaActual = PaginaActual;
            this.TamanoPagina = TamanoPagina;
        }

        public PaginadorDto(int PaginaActual, int TamanoPagina, int CantidadPaginas, int CantidadRegistros)
        {
            this.PaginaActual = PaginaActual;
            this.TamanoPagina = TamanoPagina;
            this.CantidadPaginas = CantidadPaginas;
            this.CantidadRegistros = CantidadRegistros;
        }
        #endregion

        #region propiedades publicas
        public int PaginaActual
        {
            get;
            set;
        }

        public int TamanoPagina
        {
            get;
            set;
        }

        public int CantidadPaginas
        {
            get;
            set;
        }

        public int CantidadRegistros
        {
            get;
            set;
        }
        #endregion
    }
}
