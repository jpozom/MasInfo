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
        private Database.MasInfoWebEntities_02 dbContext = null;
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
                using (this.dbContext = new Database.MasInfoWebEntities_02())
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
                using (this.dbContext = new MasInfoWebEntities_02())
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

        #region GetTutorByRut
        public BaseDto<TutorDto> GetTutorByRut(TutorDto p_Filtro)
        {
            BaseDto<TutorDto> objResult = null;

            try
            {
                using (this.dbContext = new MasInfoWebEntities_02())
                {
                    var tutorDb = (from p in this.dbContext.Tutor
                                   join es in this.dbContext.Estado on p.IdEstado equals es.Id
                                   where p.Rut == p_Filtro.FiltroRut
                                   select new TutorDto
                                   {
                                       Id = p.Id,
                                       Rut = p.Rut,
                                       Nombre = p.Nombre,
                                       ApellidoPaterno = p.ApellidoPaterno,
                                       ApellidoMaterno = p.ApellidoMaterno,
                                       Direccion = p.Direccion,
                                       FchCreate = p.FchCreate,
                                       UsrCreate = p.UsrCreate,
                                       FchUpdate = p.FchUpdate,
                                       UsrUpdate = p.UsrUpdate,
                                       IdEstado = p.IdEstado,
                                       DetalleEstado = new EstadoDto
                                       {
                                           Id = es.Id,
                                           Descripcion = es.Descripcion,
                                           Tabla = es.Tabla
                                       }
                                   }).FirstOrDefault();

                    objResult = new BaseDto<TutorDto>(tutorDb);
                }
            }
            catch (SqlException sqlEx)
            {
                objResult = new BaseDto<TutorDto>(true, sqlEx);
            }
            catch (Exception ex)
            {
                objResult = new BaseDto<TutorDto>(true, ex);
            }

            return objResult;
        }
        #endregion

        #region UpdateTutor
        public BaseDto<bool> UpdateTutor(TutorDto p_Obj)
        {
            BaseDto<bool> resultObj = null;

            try
            {
                using (this.dbContext = new MasInfoWebEntities_02())
                {
                    var tutorDb = this.dbContext.Tutor.FirstOrDefault(us => us.Id == p_Obj.Id);

                    if (tutorDb != null)
                    {
                        tutorDb.Nombre = p_Obj.Nombre;
                        tutorDb.ApellidoPaterno = p_Obj.ApellidoPaterno;
                        tutorDb.ApellidoMaterno = p_Obj.ApellidoMaterno;
                        tutorDb.Rut = p_Obj.Rut;
                        tutorDb.Direccion = p_Obj.Direccion;
                        tutorDb.FchUpdate = p_Obj.FchUpdate;
                        tutorDb.UsrUpdate = p_Obj.UsrUpdate;

                        this.dbContext.SaveChanges();
                        resultObj = new BaseDto<bool>(true);
                    }
                    else
                    {
                        throw new Exception("Tutor no encontrado en base de datos");
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

        #region GetTutorById
        public BaseDto<TutorDto> GetTutorById(TutorDto p_Filtro)
        {
            BaseDto<TutorDto> objResult = null;

            try
            {
                using (this.dbContext = new MasInfoWebEntities_02())
                {//left join al conjunto A sobre B se almacena en un campo temporal
                    var tutorDb = (from t in this.dbContext.Tutor
                                   join es in this.dbContext.Estado on t.IdEstado equals es.Id
                                   join u in this.dbContext.Usuario on t.IdUsuario equals u.Id
                                   join r in this.dbContext.Rol on u.IdRol equals r.Id                                   
                                   where t.Id == p_Filtro.FiltroId
                                   select new TutorDto
                                   {
                                       Id = t.Id,
                                       Nombre = t.Nombre,
                                       ApellidoPaterno = t.ApellidoPaterno,
                                       ApellidoMaterno = t.ApellidoMaterno,
                                       FchCreate = t.FchCreate,
                                       UsrCreate = t.UsrCreate,
                                       FchUpdate = t.FchUpdate,
                                       UsrUpdate = t.UsrUpdate,
                                       Email = t.Email,
                                       IdUsuario = t.IdUsuario,
                                       IdEstado = t.IdEstado,
                                       IdPaciente = t.IdPaciente,
                                       DetalleEstado = new EstadoDto
                                       {
                                           Id = es.Id,
                                           Descripcion = es.Descripcion,
                                           Tabla = es.Tabla
                                       },
                                       DatosUsuario = new UsuarioDto
                                       {
                                           Id = u.Id,
                                           Pass = u.Pass,
                                           Username = u.Username,
                                           IdRol = u.IdRol
                                       },
                                       DetalleRol = new RolDto
                                       {
                                           Id = r.Id,
                                           Descripcion = r.Descripcion
                                       }
                                   }).FirstOrDefault();

                    objResult = new BaseDto<TutorDto>(tutorDb);
                }
            }
            catch (SqlException sqlEx)
            {
                objResult = new BaseDto<TutorDto>(true, sqlEx);
            }
            catch (Exception ex)
            {
                objResult = new BaseDto<TutorDto>(true, ex);
            }

            return objResult;
        }
        #endregion

        #region GetTutorByPaciente
        public BaseDto<TutorDto> GetTutorByPaciente(TutorDto p_Filtro)
        {
            BaseDto<TutorDto> objResult = null;

            try
            {
                using (this.dbContext = new MasInfoWebEntities_02())
                {//left join al conjunto A sobre B se almacena en un campo temporal
                    var tutorDb = (from t in this.dbContext.Tutor
                                   join es in this.dbContext.Estado on t.IdEstado equals es.Id
                                   join u in this.dbContext.Usuario on t.IdUsuario equals u.Id
                                   join r in this.dbContext.Rol on u.IdRol equals r.Id                                      
                                   where t.IdPaciente == p_Filtro.FiltroIdPaciente
                                   select new TutorDto
                                   {
                                       Id = t.Id,
                                       Nombre = t.Nombre,
                                       Rut = t.Rut,
                                       ApellidoPaterno = t.ApellidoPaterno,
                                       ApellidoMaterno = t.ApellidoMaterno,
                                       Direccion = t.Direccion,
                                       FchCreate = t.FchCreate,
                                       UsrCreate = t.UsrCreate,
                                       FchUpdate = t.FchUpdate,
                                       UsrUpdate = t.UsrUpdate,
                                       Email = t.Email,
                                       IdUsuario = t.IdUsuario,
                                       IdEstado = t.IdEstado,
                                       IdPaciente = t.IdPaciente,
                                       DetalleEstado = new EstadoDto
                                       {
                                           Id = es.Id,
                                           Descripcion = es.Descripcion,
                                           Tabla = es.Tabla
                                       },
                                       DatosUsuario = new UsuarioDto
                                       {
                                           Id = u.Id,
                                           Pass = u.Pass,
                                           Username = u.Username,
                                           IdRol = u.IdRol
                                       },
                                       DetalleRol = new RolDto
                                       {
                                           Id = r.Id,
                                           Descripcion = r.Descripcion
                                       }                                      
                                   }).FirstOrDefault();

                    objResult = new BaseDto<TutorDto>(tutorDb);
                }
            }
            catch (SqlException sqlEx)
            {
                objResult = new BaseDto<TutorDto>(true, sqlEx);
            }
            catch (Exception ex)
            {
                objResult = new BaseDto<TutorDto>(true, ex);
            }

            return objResult;
        }
        #endregion
    }
}
