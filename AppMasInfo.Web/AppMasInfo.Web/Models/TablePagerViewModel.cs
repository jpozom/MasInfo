using AppMasInfo.Negocio.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppMasInfo.Web.Models
{
    public class TablePagerViewModel<T>
    {
        public BaseDto<T> ListaDatosPaginados
        {
            get;
            set;
        }
    }
}