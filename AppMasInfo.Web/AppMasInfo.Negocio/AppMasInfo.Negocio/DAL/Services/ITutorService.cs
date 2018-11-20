using AppMasInfo.Negocio.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppMasInfo.Negocio.DAL.Services
{
    // Si una clase implementa una interfaz, entonces tiene que proporcionar implementación a todos sus métodos.
    public interface ITutorService
    {
        /// <summary>
        /// Metodo para insertar un nuevo tutor en DB
        /// </summary>
        /// <param name="p_Obj"></param>
        /// <returns></returns>
        BaseDto<bool> InsertarTutor(TutorDto p_Obj);

        /// <summary>
        /// Metodo para obtener un tutor por un determinado Rut
        /// </summary>
        /// <param name="p_Filtro">Filtro de datos</param>
        /// <returns>Usuario encontrado</returns>
        BaseDto<TutorDto> GetTutorByRut(TutorDto p_Filtro);

        /// <summary>
        /// Metodo para actualizar registros de tutores almacenados
        /// </summary>
        /// <param name="p_Obj"></param>
        /// <returns></returns>
        BaseDto<bool> UpdateTutor(TutorDto p_Obj);

        /// <summary>
        /// Metodo para obtener un tutor por un determinado Id
        /// </summary>
        /// <param name="p_Filtro">Filtro de datos</param>
        /// <returns>Usuario encontrado</returns>
        BaseDto<TutorDto> GetTutorById(TutorDto p_Filtro);

        /// <summary>
        /// Metodo para obtener un paciente por Id por un determinado filtro de busqueda 
        /// </summary>
        /// <param name="p_Filtro">Filtro de datos</param>
        /// <returns>Usuario encontrado</returns>
        BaseDto<TutorDto> GetTutorByPaciente(TutorDto p_Filtro);
    }
}
