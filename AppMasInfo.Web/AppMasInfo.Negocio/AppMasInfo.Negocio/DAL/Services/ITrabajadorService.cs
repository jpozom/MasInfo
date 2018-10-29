using AppMasInfo.Negocio.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppMasInfo.Negocio.DAL.Services
{
    public interface ITrabajadorService
    {
        /// <summary>
        /// Metodo para insertar un nuevo trabajador en DB
        /// </summary>
        /// <param name="p_Obj"></param>
        /// <returns></returns>
        BaseDto<bool> InsertarTrabajador(TrabajadorDto p_Obj);

        /// <summary>
        /// Metodo que obtiene una lista con todos los trabajadores disponibles en DB
        /// </summary>
        /// <returns></returns>
        BaseDto<List<TrabajadorDto>> GetListaTrabajadorAll();

        /// <summary>
        /// Metodo para eliminar un trabajador solo de forma logica del sistema y almacenando el usuario que lo realizo
        /// </summary>
        /// <param name="p_Obj"></param>
        /// <returns></returns>
        BaseDto<bool> Delete(TrabajadorDto p_Obj);

        /// <summary>
        /// Metodo para obtener una lista de trabajadoresS mediante un determinado filtro de busqueda en la DB
        /// </summary>
        /// <param name="p_Filtro"></param>
        /// <returns></returns>
       BaseDto<List<TrabajadorDto>> GetListaTrabajadorbyFiltro(TrabajadorDto p_Filtro);

        /// <summary>
        /// Metodo para editar un nuevo trabajador en DB
        /// </summary>
        /// <param name="p_Obj"></param>
        /// <returns></returns>
        BaseDto<bool> UpdateTrabajador(TrabajadorDto p_Obj);

        /// <summary>
        /// Metodo para obtener un trabajador por un determinado Id
        /// </summary>
        /// <param name="p_Filtro">Filtro de datos</param>
        /// <returns>Usuario encontrado</returns>
        BaseDto<TrabajadorDto> GetTrabajadorById(TrabajadorDto p_Filtro);
    }
}
