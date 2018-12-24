﻿using AppMasInfo.Negocio.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppMasInfo.Negocio.DAL.Services
{
    public interface ITipoTelefonoService
    {
        /// <summary>
        /// Metodo para obtener todos los tipo telefonos existentes en DB
        /// </summary>
        /// <returns></returns>
        BaseDto<List<TipoTelefonoDto>> GetListaTipoTelefonoAll();
    }
}
