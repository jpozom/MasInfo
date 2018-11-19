﻿using AppMasInfo.Negocio.DAL.Entities;
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
    }
}
