using AppMasInfo.Negocio.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AppMasInfo.Negocio.Utils
{
    public class DataPaginationUtils
    {
        public static List<T> GetPagedList<T>(ref PaginadorDto p_PaginadorObj, List<T> p_objList)
        {
            List<T> lstResult = new List<T>();

            try
            {
                if (p_PaginadorObj != null && p_objList != null)
                {
                    int cantidadRegs = 0;
                    int cantidadPags = 0;

                    int regsInit = (p_PaginadorObj.PaginaActual == 1 ? 0 :
                        (int)((p_PaginadorObj.PaginaActual * p_PaginadorObj.TamanoPagina) - p_PaginadorObj.TamanoPagina));

                    cantidadRegs = p_objList.Count();
                    cantidadPags = (int)Math.Truncate((decimal)p_objList.Count() / (decimal)p_PaginadorObj.TamanoPagina) +
                        ((p_objList.Count() % p_PaginadorObj.TamanoPagina) >= 1 ? 1 : 0);

                    lstResult = p_objList.Skip(regsInit).Take(p_PaginadorObj.TamanoPagina).ToList();
                    p_PaginadorObj.CantidadPaginas = cantidadPags;
                    p_PaginadorObj.CantidadRegistros = cantidadRegs;
                }
                else
                {
                    p_PaginadorObj = new PaginadorDto();
                    p_PaginadorObj.CantidadPaginas = 1;
                    p_PaginadorObj.CantidadRegistros = 0;
                    lstResult = p_objList;
                }
            }
            catch (Exception ex)
            {
                p_PaginadorObj.CantidadPaginas = 1;
                p_PaginadorObj.CantidadRegistros = 0;

                throw ex;
            }

            return lstResult;
        }
    }
}
