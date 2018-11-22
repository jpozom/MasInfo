using AppMasInfo.Negocio.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppMasInfo.Negocio.DAL.Services
{
    public interface IUbicacionService
    {
        BaseDto<List<UbicacionDto>> GetListaUbicacionAll();
    }
}
