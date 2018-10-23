using AppMasInfo.Negocio.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppMasInfo.Negocio.DAL.Services
{
    public interface ICargoFuncionService
    {
        /// <summary>
        /// Metodo para obtener todos las funciones profesionales existentes en DB
        /// </summary>
        /// <returns></returns>
        BaseDto<List<CargoFuncionDto>> GetListaCargoFuncionAll();
    }
}
