using AppMasInfo.Negocio.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppMasInfo.Negocio.DAL.Services
{

    // Si una clase implementa una interfaz, entonces tiene que proporcionar implementación a todos sus métodos.
    public interface ITelefonoService
    {
        /// <summary>
        /// Metodo para insertar un nuevo telefono en DB
        /// </summary>
        /// <param name="p_Obj"></param>
        /// <returns></returns>
        BaseDto<bool> InsertarTelefono(TelefonoDto p_Obj);
    }
}
