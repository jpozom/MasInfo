using AppMasInfo.Negocio.DAL.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppMasInfo.Negocio.DAL.Entities
{
    public class CargoDto
    {
        public int Id { get; set; }
        public string Descripcion { get; set; }
        public Nullable<int> IdCargoFuncion { get; set; }
    }
}
