using AppMasInfo.Negocio.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppMasInfo.Negocio.DAL.Services
{
    public interface IPacienteService
    {
        
        BaseDto<List<PacienteDto>> GetListaPacienteAll();

        /// <summary>
        /// Metodo para insertar un nuevo paciente en DB
        /// </summary>
        /// <param name="p_Obj"></param>
        /// <returns></returns>
        BaseDto<bool> CreatePaciente(PacienteDto p_Obj);

        /// <summary>
        /// Metodo para editar un nuevo paciente en DB
        /// </summary>
        /// <param name="p_Obj"></param>
        /// <returns></returns>
        BaseDto<bool> UpdatePaciente(PacienteDto p_Obj);

        /// <summary>
        /// Metodo para obtener un paciente por un determinado Id
        /// </summary>
        /// <param name="p_Filtro">Filtro de datos</param>
        /// <returns>Usuario encontrado</returns>
        BaseDto<PacienteDto> GetPacienteById(PacienteDto p_Filtro);

        /// <summary>
        /// Metodo para obtener un paciente por un determinado Rut
        /// </summary>
        /// <param name="p_Filtro">Filtro de datos</param>
        /// <returns>Usuario encontrado</returns>
        BaseDto<PacienteDto> GetPacienteByRut(PacienteDto p_Filtro);

        /// <summary>
        /// Metodo para eliminar un paciente solo de forma logica del sistema y almacenando el usuario que lo realizo
        /// </summary>
        /// <param name="p_Obj"></param>
        /// <returns></returns>
        BaseDto<bool> Delete(PacienteDto p_Obj);

        /// <summary>
        /// Metodo que filtra la busqueda de los pacientes
        /// </summary>
        /// <param name="p_Filtro"></param>
        /// <returns></returns>
        BaseDto<List<PacienteDto>> GetListaPacienteByFitro(PacienteDto p_Filtro);

        BaseDto<List<PacienteDto>> GetListaPacienteByIdTutor(PacienteDto p_Filtro);

        BaseDto<List<PacienteDto>> GetListaPacienteByUbicacion(PacienteDto p_Filtro);

        BaseDto<List<PacienteDto>> GetListaPacienteByEstado(PacienteDto p_Filtro);
    }
}
