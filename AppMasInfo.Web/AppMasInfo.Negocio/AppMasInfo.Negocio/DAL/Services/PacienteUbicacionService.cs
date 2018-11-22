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
    public class PacienteUbicacionService : IPacienteUbicacionService
    {
        #region propiedades privadas
        private static PacienteUbicacionService Instance = null;
        private Database.MasInfoWebEntities_02 dbContext = null;
        #endregion

        #region singleton
        private PacienteUbicacionService() { }

        public static PacienteUbicacionService GetInstance()
        {
            if (PacienteUbicacionService.Instance == null)
                PacienteUbicacionService.Instance = new PacienteUbicacionService();

            return PacienteUbicacionService.Instance;
        }
        #endregion

        #region AddUbicacionPaciente
        public BaseDto<bool> AddUbicacionPaciente(PacienteUbicacionDto ubicacion)
        {
            BaseDto<bool> result = null;

            try
            {
                using (this.dbContext = new MasInfoWebEntities_02())
                {
                    var equipoPaciente = this.dbContext.PacienteUbicacion.Add(
                        new Database.PacienteUbicacion
                        {
                            IdPaciente = ubicacion.IdPaciente,
                            IdUbicacion = ubicacion.IdUbicacion,
                            FchIngreso = ubicacion.FchIngreso,
                            UsrIngreso = ubicacion.UsrIngreso,
                            Observacion = ubicacion.Observacion
                        });

                    // Guardamos los cambios en base de datos
                    this.dbContext.SaveChanges();

                    result = new BaseDto<bool>(true);
                }
            }
            catch (SqlException sqlEx)
            {
                result = new BaseDto<bool>(true, sqlEx);
            }
            catch (Exception ex)
            {
                result = new BaseDto<bool>(true, ex);
            }
            return result;
        }
        #endregion

        #region GetUbicacionPacienteByIdPaciente
        public BaseDto<List<PacienteUbicacionDto>> GetUbicacionPacienteByIdPaciente(PacienteDto p_Filtro)
        {
            BaseDto<List<PacienteUbicacionDto>> objResult = null;

            try
            {
                using (this.dbContext = new Database.MasInfoWebEntities_02())
                {
                    var pacienteUbicacion = (from pu in this.dbContext.PacienteUbicacion
                                             join ub in this.dbContext.Ubicacion on pu.IdUbicacion equals ub.Id
                                             join p in this.dbContext.Paciente on pu.IdPaciente equals p.Id                                            
                                             where pu.IdPaciente == p_Filtro.FiltroId
                                             select new PacienteUbicacionDto
                                             {
                                                 IdPaciente = pu.IdPaciente,
                                                 IdUbicacion = pu.IdUbicacion,                                                 
                                                 DetallePaciente = new PacienteDto
                                                 {
                                                     Id = p.Id,
                                                     Nombre = p.Nombre,
                                                     ApellidoPaterno = p.ApellidoPaterno,
                                                     ApellidoMaterno = p.ApellidoMaterno,
                                                     Rut = p.Rut
                                                 }

                                             }).ToList();

                    objResult = new BaseDto<List<PacienteUbicacionDto>>(pacienteUbicacion);
                }
            }
            catch (SqlException sqlEx)
            {
                objResult = new BaseDto<List<PacienteUbicacionDto>>(true, sqlEx);
            }
            catch (Exception ex)
            {
                objResult = new BaseDto<List<PacienteUbicacionDto>>(true, ex);
            }

            return objResult;
        }
        #endregion
    }
}
