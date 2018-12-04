using AppMasInfo.Negocio.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace AppMasInfo.Negocio.DAL.Services
{
    public interface IEquipoPacienteService
    {
        BaseDto<List<EquipoPacienteDto>> GetEquipoPacienteByIdPaciente(PacienteDto p_Filtro);
        BaseDto<bool> AddEquipoPaciente(EquipoPacienteDto equipo);
        BaseDto<bool> Delete(EquipoPacienteDto p_Obj);
    }
}
