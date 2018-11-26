using AppMasInfo.Negocio.DAL.Database;
using AppMasInfo.Negocio.DAL.Entities;
using AppMasInfo.Negocio.Utils;
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
        private Database.MasInfoWebEntities_02 dbContext = null;
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

        #region Metodos Publicos

        #region GetListaPacienteAll
        public BaseDto<List<PacienteDto>> GetListaPacienteAll(PacienteDto p_Filtro)
        {
            BaseDto<List<PacienteDto>> objResult = null;

            try
            {
                using (this.dbContext = new Database.MasInfoWebEntities_02())
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
                                          IdEstado = p.IdEstado,
                                          NumeroTelefono = p.NumeroTelefono,
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
            BaseDto<bool> result = null;

            try
            {
                using (this.dbContext = new MasInfoWebEntities_02())
                {
                    var pacienteDb = this.dbContext.Paciente.Add(
                        new Database.Paciente
                        {
                            Rut = p_Obj.Rut,
                            Nombre = p_Obj.Nombre,
                            ApellidoPaterno = p_Obj.ApellidoPaterno,
                            ApellidoMaterno = p_Obj.ApellidoMaterno,
                            Edad = p_Obj.Edad,
                            Direccion = p_Obj.Direccion,
                            FchCreate = p_Obj.FchCreate,
                            UsrCreate = p_Obj.UsrCreate,
                            NumeroTelefono = p_Obj.NumeroTelefono,
                            IdEstado = p_Obj.IdEstado
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

        #region UpdatePaciente
        public BaseDto<bool> UpdatePaciente(PacienteDto p_Obj)
        {
            BaseDto<bool> resultObj = null;

            try
            {
                using (this.dbContext = new MasInfoWebEntities_02())
                {
                    var pacienteDb = this.dbContext.Paciente.FirstOrDefault(us => us.Id == p_Obj.Id);

                    if (pacienteDb != null)
                    {
                        pacienteDb.Nombre = p_Obj.Nombre;
                        pacienteDb.ApellidoPaterno = p_Obj.ApellidoPaterno;
                        pacienteDb.ApellidoMaterno = p_Obj.ApellidoMaterno;
                        pacienteDb.Edad = p_Obj.Edad;
                        pacienteDb.Direccion = p_Obj.Direccion;
                        pacienteDb.NumeroTelefono = p_Obj.NumeroTelefono;
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
                using (this.dbContext = new MasInfoWebEntities_02())
                {
                    var pacienteDb = (from p in this.dbContext.Paciente
                                      join es in this.dbContext.Estado on p.IdEstado equals es.Id
                                      join t in this.dbContext.Tutor on p.Id equals t.IdPaciente                                                                                                                
                                      join ep in this.dbContext.EquipoPaciente on p.Id equals ep.IdPaciente into tmpCf
                                      join pu in this.dbContext.PacienteUbicacion on p.Id equals pu.IdPaciente into tmpC                                      
                                      from ep in tmpCf.DefaultIfEmpty()
                                      from pu in tmpC.DefaultIfEmpty()
                                      where p.Id == p_Filtro.FiltroId &&
                                      pu.Habilitado == true
                                      orderby pu.FchIngreso descending
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
                                          NumeroTelefono = p.NumeroTelefono,
                                          IdEstado = p.IdEstado,
                                          DetalleEstado = new EstadoDto
                                          {
                                              Id = es.Id,
                                              Descripcion = es.Descripcion,
                                              Tabla = es.Tabla
                                          },
                                          DetalleTutor = new TutorDto
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
                                              IdPaciente = t.IdPaciente
                                          },                                                                                    
                                          DetalleEquipoPaciente = new EquipoPacienteDto
                                          {
                                              Id = ep == null ? 0 : ep.Id,
                                              IdPaciente = ep == null ? 0 : ep.IdPaciente,
                                              Idtrabajador = ep == null ? 0 : ep.IdPaciente,
                                          },
                                          DetallePacienteUbicacion = new PacienteUbicacionDto
                                          {
                                              Id = pu == null ? 0 : pu.Id,
                                              IdPaciente = pu == null ? 0 : pu.IdPaciente,
                                              FchIngreso = pu.FchIngreso,
                                              Observacion = pu.Observacion,
                                          },
                                          DetalleUbicacion = (from ub in this.dbContext.Ubicacion
                                                        join pu in this.dbContext.PacienteUbicacion on ub.Id equals pu.IdUbicacion
                                                        where ub.Id == pu.IdUbicacion
                                                        select new UbicacionDto
                                                        {
                                                            Id = ub.Id,
                                                            Descripcion = ub.Descripcion

                                                        }).FirstOrDefault()
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

        #region GetPacienteByRut
        public BaseDto<PacienteDto> GetPacienteByRut(PacienteDto p_Filtro)
        {
            BaseDto<PacienteDto> objResult = null;

            try
            {
                using (this.dbContext = new MasInfoWebEntities_02())
                {
                    var pacienteDb = (from p in this.dbContext.Paciente
                                      join es in this.dbContext.Estado on p.IdEstado equals es.Id
                                      where p.Rut == p_Filtro.FiltroRut
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
                                          NumeroTelefono = p.NumeroTelefono,
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
                using (this.dbContext = new MasInfoWebEntities_02())
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

        #region GetListaPacienteByFitro
        public BaseDto<List<PacienteDto>> GetListaPacienteByFitro(PacienteDto p_Filtro)
        {
            BaseDto<List<PacienteDto>> result = null;
            var lstResultado = new List<PacienteDto>();

            try
            {
                using (this.dbContext = new MasInfoWebEntities_02())
                {
                    if (p_Filtro.FiltroNombre != null || p_Filtro.FiltroId != null)
                    {
                        lstResultado = (from p in this.dbContext.Paciente
                                        where (p.IdEstado == p_Filtro.FiltroIdEstado || p_Filtro.FiltroIdEstado == null) &&
                                              (p.Nombre.Contains(p_Filtro.FiltroNombre) || string.IsNullOrEmpty(p_Filtro.FiltroNombre) &&
                                              (p.Id == p_Filtro.FiltroId))
                                        select new PacienteDto
                                        {
                                            Id = p.Id,
                                            Nombre = p.Nombre,
                                            ApellidoPaterno = p.ApellidoPaterno,
                                            ApellidoMaterno = p.ApellidoMaterno,
                                            Direccion = p.Direccion,
                                            Edad = p.Edad,
                                            Rut = p.Rut,
                                            FchCreate = p.FchCreate,
                                            FchUpdate = p.FchUpdate,
                                            IdEstado = p.IdEstado
                                        }).ToList();
                    }
                    else
                    {
                        lstResultado = (from p in this.dbContext.Paciente
                                        where (p.IdEstado == p_Filtro.FiltroIdEstado || p_Filtro.FiltroIdEstado == null)
                                        select new PacienteDto
                                        {
                                            Id = p.Id,
                                            Nombre = p.Nombre,
                                            ApellidoPaterno = p.ApellidoPaterno,
                                            ApellidoMaterno = p.ApellidoMaterno,
                                            Direccion = p.Direccion,
                                            Edad = p.Edad,
                                            Rut = p.Rut,
                                            FchCreate = p.FchCreate,
                                            FchUpdate = p.FchUpdate,
                                            IdEstado = p.IdEstado                                            
                                        }).ToList();
                    }


                    PaginadorDto datosPaginado = p_Filtro.DatosPaginado;
                    var lstPaginada = DataPaginationUtils.GetPagedList<PacienteDto>(ref datosPaginado, lstResultado);
                    result = new BaseDto<List<PacienteDto>>(true, datosPaginado, lstPaginada);
                }
            }
            catch (SqlException sqlEx)
            {
                result = new BaseDto<List<PacienteDto>>(true, sqlEx);
            }
            catch (Exception ex)
            {
                result = new BaseDto<List<PacienteDto>>(true, new Exception("Error al intentar obtener los pacientes", ex));
            }

            return result;
        }
        #endregion

        #endregion
    }
}
