﻿using AppMasInfo.Negocio.DAL.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppMasInfo.Negocio.DAL.Entities
{
    public class TelefonoDto
    {
        public int Id { get; set; }
        public string NumeroTelefono { get; set; }
        public int IdTipoTelefono { get; set; }
        public long IdUsuario { get; set; }

        public long? FiltroIdUsuario { get; set; }
    }
}
