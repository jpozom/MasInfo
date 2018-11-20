using AppMasInfo.Negocio.DAL.Database;
using AppMasInfo.Negocio.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppMasInfo.Negocio.DAL.Services
{
    public class EquipoPacienteService : IEquipoPacienteService
    {
        #region propiedades privadas
        private static EquipoPacienteService Instance = null;
        private Database.MasInfoWebEntities_02 dbContext = null;
        #endregion

        #region singleton
        private EquipoPacienteService() { }

        public static EquipoPacienteService GetInstance()
        {
            if (EquipoPacienteService.Instance == null)
                EquipoPacienteService.Instance = new EquipoPacienteService();

            return EquipoPacienteService.Instance;
        }
        #endregion

        #region GetEquipoPacienteByIdPaciente
        public BaseDto<List<EquipoPacienteDto>> GetEquipoPacienteByIdPaciente(PacienteDto p_Filtro)
        {
            BaseDto<List<EquipoPacienteDto>> objResult = null;

            try
            {
                using (this.dbContext = new Database.MasInfoWebEntities_02())
                {
                    var epDb = (from ep in this.dbContext.EquipoPaciente
                                join t in this.dbContext.Trabajador on ep.IdTrabajador equals t.Id
                                join p in this.dbContext.Paciente on ep.IdPaciente equals p.Id
                                where ep.IdPaciente == p_Filtro.FiltroId
                                select new EquipoPacienteDto
                                {
                                    IdPaciente = ep.IdPaciente,
                                    Idtrabajador  = ep.IdTrabajador,
                                    Trabajador = new TrabajadorDto { Id =  ep.IdTrabajador,
                                                                    Nombre = t.Nombre,
                                                                    ApellidoPaterno = t.ApellidoPaterno,
                                                                    ApellidoMaterno = t.ApellidoMaterno,
                                                                    IdUsuario = t.IdUsuario 
                                                                  },
                                    Paciente = new PacienteDto { Id =p.Id,
                                                                     Nombre = p.Nombre,
                                                                     ApellidoPaterno = p.ApellidoPaterno,
                                                                     ApellidoMaterno = p.ApellidoMaterno,
                                                                     Rut = p.Rut
                                                                }

                                }).ToList();

                    objResult = new BaseDto<List<EquipoPacienteDto>>(epDb);
                }
            }
            catch (SqlException sqlEx)
            {
                objResult = new BaseDto<List<EquipoPacienteDto>>(true, sqlEx);
            }
            catch (Exception ex)
            {
                objResult = new BaseDto<List<EquipoPacienteDto>>(true, ex);
            }

            return objResult;
        }
        #endregion

    }
}
