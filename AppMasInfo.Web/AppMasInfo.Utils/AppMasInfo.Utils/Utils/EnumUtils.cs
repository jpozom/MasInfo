using System;
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
            Administrador = 5,
            Tutor = 6
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

        /// <summary>
        /// Enum con todos los tipos de telefonos de la tabla TipoTelefono existentes en DB
        /// </summary>
        public enum TipoTelefonoEnum
        {
            Celular_Personal = 1,
            Celular_Trabajo = 2,
            Fijo_Casa = 3,
            Fijo_Trabajo = 4         
        }
    }
}
