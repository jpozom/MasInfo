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
                            Observacion = ubicacion.Observacion,
                            Habilitado = true
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
                                             where pu.IdPaciente == p_Filtro.FiltroId &&
                                             pu.Habilitado == true
                                             select new PacienteUbicacionDto
                                             {
                                                 Id = pu.Id,
                                                 IdPaciente = pu.IdPaciente,
                                                 IdUbicacion = pu.IdUbicacion,
                                                 Observacion = pu.Observacion,
                                                 FchIngreso = pu.FchIngreso,
                                                 DetallePaciente = new PacienteDto
                                                 {
                                                     Id = p.Id,
                                                     Nombre = p.Nombre,
                                                     ApellidoPaterno = p.ApellidoPaterno,
                                                     ApellidoMaterno = p.ApellidoMaterno,
                                                     Rut = p.Rut
                                                 },
                                                 DetalleUbicacion = new UbicacionDto
                                                 {
                                                     Id = ub.Id,
                                                     Descripcion = ub.Descripcion,                                                    
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

        #region UpdatePacienteUbicacion
        public BaseDto<bool> UpdatePacienteUbicacion(PacienteUbicacionDto p_Obj)
        {
            BaseDto<bool> resultObj = null;

            try
            {
                using (this.dbContext = new MasInfoWebEntities_02())
                {
                    var pacienteUbiDb = this.dbContext.PacienteUbicacion.FirstOrDefault(p => p.Id == p_Obj.Id);

                    if (pacienteUbiDb != null)
                    {
                        pacienteUbiDb.Observacion = p_Obj.Observacion;

                        this.dbContext.SaveChanges();
                        resultObj = new BaseDto<bool>(true);
                    }
                    else
                    {
                        throw new Exception("Datosn no encontrados en base de datos");
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                resultObj = new BaseDto<bool>(true, sqlEx);
            }
            catch (Exception ex)
            {
                resultObj = new BaseDto<bool>(true, ex);
            }

            return resultObj;
        }
        #endregion

        #region Delete
        public BaseDto<bool> Delete(PacienteUbicacionDto p_Obj)
        {
            BaseDto<bool> result = null;

            try
            {
                using (this.dbContext = new MasInfoWebEntities_02())
                {
                    //Obtener el objeto origen desde base de datos
                    //El metodo .FirstOrDefault, retorna el primer objeto encontrado de acuerdo
                    // a un determinado filtro de búsqueda, y en caso contrario, retorna null
                    var objOrigenDb = this.dbContext.PacienteUbicacion.FirstOrDefault(u => u.Id == p_Obj.Id);

                    if (objOrigenDb != null)
                    {
                        objOrigenDb.Habilitado = false;

                        this.dbContext.SaveChanges();
                        result = new BaseDto<bool>(true);
                    }
                    else
                    {
                        throw new Exception("Id PacienteUbicacion no encontrado en base de datos");
                    }
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
    }
}
