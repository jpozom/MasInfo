﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppMasInfo.Utils.Utils
{
    public class EnumUtils
    {
        /// <summary>
        /// Enum con todos los roles existentes en DB
        /// </summary>
        public enum RolEnum
        {
            Medico = 1,
            Enfermera = 2,
            Tecnico = 3,
            Auxiliar = 4,
            Administrador = 5
        }

        /// <summary>
        /// Enum con todos los estados de la tabla Estados existentes en DB
        /// </summary>
        public enum EstadoEnum
        {
            Trabajador_Habilitado = 1,
            Trabajador_Deshabilitado = 2,
            Tutor_Habilitado = 3,
            Tutor_Deshabilitado = 4,
            Paciente_Habilitado = 5,
            Paciente_Deshabilitado = 6           
        }


    }
}
