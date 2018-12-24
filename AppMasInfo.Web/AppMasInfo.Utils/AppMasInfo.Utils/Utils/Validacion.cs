using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppMasInfo.Utils.Utils
{
    public class Validacion
    {
        public class Mensajes
        {
            public const string Required = "Este valor es obligatorio";
            public const string RegularExpression = "No cumple con el formato necesario";
            public const string DataTypeNumber = "Este valor debe ser númerico";
        }
        public class Patterns
        {
            public const string Rut = @"^[0-9]{7,8}-[0-9Kk]{1}";
            public const string Entero = @"^[0-9]{1,18}$";
            public const string Alfanumerico = @"^[A-Za-z0-9 ._áéíóúñÁÉÍÓÚÑ?¡!¿@]+${1,}";
            public const string Texto = @"^[A-Za-z ._áéíóúñÁÉÍÓÚÑ?¡!¿@]+${1,}";
            public const string FechaDDMMYYY = @"(?:(?:0[1-9]|1[0-2])[\/\\-. ]?(?:0[1-9]|[12][0-9])|(?:(?:0[13-9]|1[0-2])[\/\\-. ]?30)|(?:(?:0[13578]|1[02])[\/\\-. ]?31))[\/\\-. ]?(?:19|20)[0-9]{2}"; //dd/mm/yyyy                        
        }
    }
}
