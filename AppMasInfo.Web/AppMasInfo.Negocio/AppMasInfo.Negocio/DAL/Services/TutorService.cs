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
    public class TutorService : ITutorService
    {
        #region propiedades privadas
        private static TutorService Instance = null;
        private Database.MasInfoWebEntities dbContext = null;
        #endregion

        #region singleton
        private TutorService() { }

        public static TutorService GetInstance()
        {
            if (TutorService.Instance == null)
                TutorService.Instance = new TutorService();

            return TutorService.Instance;
        }
        #endregion

        #region GetTutorAll
        public BaseDto<List<TutorDto>> GetTutorAll(TutorDto p_Filtro)
        {
            BaseDto<List<TutorDto>> objResult = null;

            try
            {
                using (this.dbContext = new Database.MasInfoWebEntities())
                {
                    var tutorDb = (from t in this.dbContext.Tutor
                                      where t.IdEstado == p_Filtro.FiltroIdEstado
                                      select new TutorDto
                                      {
                                          Id = t.Id,
                                          Rut = t.Rut,
                                          Nombre = t.Nombre,
                                          ApellidoPaterno = t.ApellidoPaterno,
                                          ApellidoMaterno = t.ApellidoMaterno,                             
                                          Direccion = t.Direccion,                                   
                                          Telefono = t.Telefono,
                                          IdUsuario = t.IdUsuario,
                                          IdEstado = t.IdEstado,
                                          Email = t.Email,
                                          IdPaciente = t.IdPaciente
                                      }).ToList();

                    objResult = new BaseDto<List<TutorDto>>(tutorDb);
                }
            }
            catch (SqlException sqlEx)
            {
                objResult = new BaseDto<List<TutorDto>>(true, sqlEx);
            }
            catch (Exception ex)
            {
                objResult = new BaseDto<List<TutorDto>>(true, ex);
            }

            return objResult;
        }
        #endregion

        #region InsertarTutor
        public BaseDto<bool> InsertarTutor(TutorDto p_Obj)
        {
            BaseDto<bool> result = null;

            try
            {
                using (this.dbContext = new MasInfoWebEntities())
                {
                    var tutorDb = this.dbContext.Tutor.Add(
                        new Database.Tutor
                        {
                            Rut = p_Obj.Rut,
                            Nombre = p_Obj.Nombre,
                            ApellidoPaterno = p_Obj.ApellidoPaterno,
                            ApellidoMaterno = p_Obj.ApellidoMaterno,                            
                            Direccion = p_Obj.Direccion,
                            FchCreate = p_Obj.FchCreate,
                            UsrCreate = p_Obj.UsrCreate,
                            Telefono = p_Obj.Telefono,
                            Email = p_Obj.Email,
                            IdEstado = p_Obj.IdEstado,
                            IdPaciente = p_Obj.IdPaciente,
                            IdUsuario = p_Obj.IdUsuario
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
    }
}
