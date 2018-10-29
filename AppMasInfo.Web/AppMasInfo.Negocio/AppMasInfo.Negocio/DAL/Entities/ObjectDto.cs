using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppMasInfo.Negocio.DAL.Entities
{
    [Serializable]
    public abstract class ObjectDto
    {
        #region constructores
        public ObjectDto()
        {
            this.DatosPaginado = null;
        }

        public ObjectDto(int PaginaActual, int TamanoPagina)
        {
            this.DatosPaginado = new PaginadorDto(PaginaActual, TamanoPagina);
        }
        #endregion

        #region propiedades publicas
        public PaginadorDto DatosPaginado
        {
            get;
            set;
        }
        #endregion  
    }
}
