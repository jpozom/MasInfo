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
    public class PacienteService : IPacienteService
    {
        #region propiedades privadas
        private static PacienteService Instance = null;
        private Database.masInfoWebEntities dbContext = null;
        #endregion

        #region singleton
        private PacienteService() { }

        public static PacienteService GetInstance()
        {
            if (PacienteService.Instance == null)
                PacienteService.Instance = new PacienteService();

            return PacienteService.Instance;
        }
        #endregion

        #region GetPacienteAll
        public BaseDto<List<PacienteDto>> GetPacienteAll(PacienteDto p_Filtro)
        {
            BaseDto<List<PacienteDto>> objResult = null;

            try
            {
                using (this.dbContext = new Database.masInfoWebEntities())
                {
                    var pacienteDb = (from p in this.dbContext.Paciente
                                     where p.IdEstado == p_Filtro.FiltroIdEstado
                                     select new PacienteDto
                                     {
                                         Id = p.Id,
                                         Rut = p.Rut,
                                         Nombre = p.Nombre,
                                         ApellidoPaterno = p.ApellidoPaterno,
                                         ApellidoMaterno = p.ApellidoMaterno,
                                         Edad = p.Edad,
                                         Direccion = p.Direccion,
                                         IdTutor = p.IdTutor,
                                         IdEstado = p.IdEstado,
                                         Telefono = p.Telefono,                                                                                 
                                     }).ToList();

                    objResult = new BaseDto<List<PacienteDto>>(pacienteDb);
                }
            }
            catch (SqlException sqlEx)
            {
                objResult = new BaseDto<List<PacienteDto>>(true, sqlEx);
            }
            catch (Exception ex)
            {
                objResult = new BaseDto<List<PacienteDto>>(true, ex);
            }

            return objResult;
        }
        #endregion

        #region CreatePaciente
        public BaseDto<bool> CreatePaciente(PacienteDto p_Obj)
        {
            BaseDto<bool> resultObj = null;

            try
            {
                using (this.dbContext = new Database.masInfoWebEntities())
                {
                    var pacienteDb = new Paciente
                    {
                        Id = p_Obj.Id,
                        Rut = p_Obj.Rut,
                        Nombre = p_Obj.Nombre,
                        ApellidoPaterno = p_Obj.ApellidoPaterno,
                        ApellidoMaterno = p_Obj.ApellidoMaterno,
                        Edad = p_Obj.Edad,
                        Direccion = p_Obj.Direccion,
                        FchCreate = p_Obj.FchCreate,
                        UsrCreate = p_Obj.UsrCreate,
                        IdTutor = p_Obj.IdTutor,                        
                        Telefono = p_Obj.Telefono,
                        IdEstado = p_Obj.IdEstado
                    };

                    this.dbContext.Paciente.Add(pacienteDb);
                    this.dbContext.SaveChanges();

                    resultObj = new BaseDto<bool>(true);
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

        #region UpdatePaciente
        public BaseDto<bool> UpdatePaciente(PacienteDto p_Obj)
        {
            BaseDto<bool> resultObj = null;

            try
            {
                using (this.dbContext = new masInfoWebEntities())
                {
                    var pacienteDb = this.dbContext.Paciente.FirstOrDefault(us => us.Id == p_Obj.Id);

                    if (pacienteDb != null)
                    {
                        pacienteDb.Nombre = p_Obj.Nombre;
                        pacienteDb.ApellidoPaterno = p_Obj.ApellidoPaterno;
                        pacienteDb.ApellidoMaterno = p_Obj.ApellidoMaterno;
                        pacienteDb.Edad = p_Obj.Edad;
                        pacienteDb.Direccion = p_Obj.Direccion;
                        pacienteDb.Telefono = p_Obj.Telefono;
                        pacienteDb.FchUpdate = p_Obj.FchUpdate;
                        pacienteDb.UsrUpdate = p_Obj.UsrUpdate;                        

                        this.dbContext.SaveChanges();
                        resultObj = new BaseDto<bool>(true);
                    }
                    else
                    {
                        throw new Exception("Usuario no encontrado en base de datos");
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

        #region GetPacienteById
        public BaseDto<PacienteDto> GetPacienteById(PacienteDto p_Filtro)
        {
            BaseDto<PacienteDto> objResult = null;

            try
            {
                using (this.dbContext = new masInfoWebEntities())
                {
                    var pacienteDb = (from p in this.dbContext.Paciente
                                     join es in this.dbContext.Estado on p.IdEstado equals es.Id                                     
                                     where p.Id == p_Filtro.FiltroId
                                     select new PacienteDto
                                     {
                                         Id = p.Id,
                                         Rut = p.Rut,
                                         Nombre = p.Nombre,
                                         ApellidoPaterno = p.ApellidoPaterno,
                                         ApellidoMaterno = p.ApellidoMaterno,
                                         Edad = p.Edad,
                                         Direccion = p.Direccion,
                                         FchCreate = p.FchCreate,
                                         UsrCreate = p.UsrCreate,
                                         FchUpdate = p.FchUpdate,
                                         UsrUpdate = p.UsrUpdate,
                                         IdTutor = p.IdTutor,
                                         Telefono = p.Telefono,
                                         IdEstado = p.IdEstado,
                                         DetalleEstado = new EstadoDto
                                         {
                                             Id = es.Id,
                                             Descripcion = es.Descripcion,
                                             Tabla = es.Tabla
                                         }                                         
                                     }).FirstOrDefault();

                    objResult = new BaseDto<PacienteDto>(pacienteDb);
                }
            }
            catch (SqlException sqlEx)
            {
                objResult = new BaseDto<PacienteDto>(true, sqlEx);
            }
            catch (Exception ex)
            {
                objResult = new BaseDto<PacienteDto>(true, ex);
            }

            return objResult;
        }
        #endregion

        #region Delete
        public BaseDto<bool> Delete(PacienteDto p_Obj)
        {
            BaseDto<bool> result = null;

            try
            {
                using (this.dbContext = new masInfoWebEntities())
                {
                    //Obtener el objeto origen desde base de datos
                   //El metodo .FirstOrDefault, retorna el primer objeto encontrado de acuerdo
                  // a un determinado filtro de búsqueda, y en caso contrario, retorna null
                    var objOrigenDb = this.dbContext.Paciente.FirstOrDefault(c => c.Id == p_Obj.Id);

                    if (objOrigenDb != null)
                    {
                        objOrigenDb.FchUpdate = p_Obj.FchUpdate;
                        objOrigenDb.UsrUpdate = p_Obj.UsrUpdate;
                        objOrigenDb.IdEstado = p_Obj.IdEstado;

                        this.dbContext.SaveChanges();
                        result = new BaseDto<bool>(true);
                    }
                    else
                    {
                        throw new Exception("Paciente no encontrado en base de datos");
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
