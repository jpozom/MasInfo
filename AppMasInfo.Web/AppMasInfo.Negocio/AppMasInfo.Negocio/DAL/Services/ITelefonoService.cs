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

        /// <summary>
        /// Metodo para obtener los datos de los telefonos asociados a ese Id por un determinado filtro de busqueda 
        /// </summary>
        /// <param name="p_Filtro">Filtro de datos</param>
        /// <returns>Usuario encontrado</returns>
        BaseDto<TelefonoDto> GetTelefonoByIdUsuario(TelefonoDto p_Filtro);

        /// <summary>
        /// Metodo para editar datos de la tabla Telefonos en DB
        /// </summary>
        /// <param name="p_Obj"></param>
        /// <returns></returns>
        BaseDto<bool> UpdateTelefono(TelefonoDto p_Obj);
    }
}
