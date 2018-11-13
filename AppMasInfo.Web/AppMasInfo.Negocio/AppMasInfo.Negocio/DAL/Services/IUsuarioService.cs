using AppMasInfo.Negocio.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppMasInfo.Negocio.DAL.Services
{
    public interface IUsuarioService
    {
        /// <summary>
        /// Metodo para validar el usuario con las credenciales
        /// </summary>
        /// <param name="p_Filtro">Objeto con las credenciales del usuario</param>
        /// <returns>Usuario del sistema</returns>
        UsuarioDto GetUsuarioByCredentials(UsuarioDto p_Filtro);

        /// <summary>
        /// Metodo para obtener un usuario creado por un determinado filtro de busqueda
        /// </summary>
        /// <param name="p_Filtro">Objeto con las credenciales del usuario</param>
        /// <returns>Usuario del sistema</returns>
        BaseDto<UsuarioDto> GetUsuarioByUsername(UsuarioDto p_Filtro);

        /// <summary>
        /// Metodo para insertar un nuevo Usuario en DB
        /// </summary>
        /// <param name="p_Obj"></param>
        /// <returns></returns>
        BaseDto<bool> InsertarUsuario(UsuarioDto p_Obj);
    }
}
