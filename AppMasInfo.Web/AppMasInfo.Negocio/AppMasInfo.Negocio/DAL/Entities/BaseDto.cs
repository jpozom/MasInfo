using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppMasInfo.Negocio.DAL.Entities
{
    public class BaseDto<T>
    {
        #region constructores
        public BaseDto()
        {
            this.Error = null;
            this.DatosPaginado = null;
            this.Error = null;
        }

        public BaseDto(T Value)
        {
            this.HasError = false;
            this.Error = null;
            this.HasValue = (Value != null ? true : false);
            this.Value = Value;
            this.HasPaginado = false;
            this.DatosPaginado = null;
        }

        public BaseDto(bool HasError, Exception Error)
        {
            this.HasError = HasError;
            this.Error = Error;
            this.HasValue = false;
            this.Value = default(T);
            this.HasPaginado = false;
            this.DatosPaginado = null;
        }

        public BaseDto(bool HasPaginado, PaginadorDto DatosPaginado, T Value)
        {
            this.HasError = false;
            this.Error = null;
            this.HasValue = (Value != null ? true : false);
            this.Value = Value;
            this.HasPaginado = true;
            this.DatosPaginado = new PaginadorDto(DatosPaginado.PaginaActual,
                DatosPaginado.TamanoPagina,
                DatosPaginado.CantidadPaginas,
                DatosPaginado.CantidadRegistros);
        }
        #endregion

        #region propiedades publicas
        public bool HasError
        {
            get;
            set;
        }

        public Exception Error
        {
            get;
            set;
        }

        public bool HasValue
        {
            get;
            set;
        }

        public T Value
        {
            get;
            set;
        }

        public bool HasPaginado
        {
            get;
            set;
        }

        public PaginadorDto DatosPaginado
        {
            get;
            set;
        }
        #endregion
    }
}
