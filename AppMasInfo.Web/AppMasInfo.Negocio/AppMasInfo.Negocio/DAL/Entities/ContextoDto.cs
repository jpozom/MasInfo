using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppMasInfo.Negocio.DAL.Entities
{
    [Serializable]
    public class ContextoDto
    {
        public string Username
        {
            get;
            set;
        }

        public string Pass
        {
            get;
            set;
        }

        public string Rol
        {
            get;
            set;
        }

        public string Nombre
        {
            get;
            set;
        }
       
        public string Email
        {
            get;
            set;
        }

        public string Cargo
        {
            get;
            set;
        }   
        

        public long IdUsuario
        {
            get;
            set;
        }

        public long IdRol
        {
            get;
            set;
        }
    }
}
