using AppMasInfo.Negocio.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppMasInfo.Negocio.DAL.Services
{
    public interface ICargoService
    {
        /// <summary>
        /// Metodo para obtener todos los cargos existentes en DB
        /// </summary>
        /// <returns></returns>
        BaseDto<List<CargoDto>> GetListaCargoAll();
    }
}
