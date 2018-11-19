﻿using AppMasInfo.Negocio.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppMasInfo.Negocio.DAL.Services
{
    public interface IRolService
    {
        /// <summary>
        /// Metodo para obtener todos los roles existentes en DB
        /// </summary>
        /// <returns></returns>
        BaseDto<List<RolDto>> GetListaRolAll();

        /// <summary>
        /// Metodo que devuelve solo una lista de roles Usuario/Trabajador
        /// </summary>
        /// <returns></returns>
        BaseDto<List<RolDto>> GetListaRolTrabajador();

        /// <summary>
        /// Metodo que devuelve solo una lista de roles Usuario/Tutor
        /// </summary>
        /// <returns></returns>
        BaseDto<List<RolDto>> GetListaRolTutor();
    }
}
