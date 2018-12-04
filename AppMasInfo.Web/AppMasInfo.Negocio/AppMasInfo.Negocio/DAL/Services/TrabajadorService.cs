using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using AppMasInfo.Negocio.DAL.Database;
using AppMasInfo.Negocio.DAL.Entities;
using AppMasInfo.Negocio.Utils;

namespace AppMasInfo.Negocio.DAL.Services
{
    public class TrabajadorService : ITrabajadorService
    {
        #region propiedades privadas
        private static TrabajadorService Instance = null;
        private Database.MasInfoWebEntities_02 dbContext = null;
        #endregion

        //Permite el acceso global a dicha instancia a traves de este metodo.
        #region singleton
        private TrabajadorService() { }

        public static TrabajadorService GetInstance()
        {
            if (TrabajadorService.Instance == null)
                TrabajadorService.Instance = new TrabajadorService();

            return TrabajadorService.Instance;
        }
        #endregion

        #region metodos publicos

        #region InsertarTrabajador
        public BaseDto<bool> InsertarTrabajador(TrabajadorDto p_Obj)
        {
            BaseDto<bool> result = null;

            try
            {
                using (this.dbContext = new MasInfoWebEntities_02())
                {
                    var trabajadorDb = this.dbContext.Trabajador.Add(
                        new Database.Trabajador
                        {
                            IdUsuario = p_Obj.IdUsuario,
                            Rut = p_Obj.Rut,
                            Nombre = p_Obj.Nombre,
                            ApellidoPaterno = p_Obj.ApellidoPaterno,
                            ApellidoMaterno = p_Obj.ApellidoMaterno,
                            IdCargo = p_Obj.IdCargo,
                            FchCreate = p_Obj.FchCreate,
                            UsrCreate = p_Obj.UsrCreate,
                            Email = p_Obj.Email,
                            IdEstado = p_Obj.IdEstado,
                            IdCargoFuncion = p_Obj.IdCargoFuncion
                        });

                    // Guardamos los cambios en base de datos
                    this.dbContext.SaveChanges();

                    // Asignamos un valor a la variable result describiendo la ejecución correcta
                    result = new BaseDto<bool>(true);
                }
            }
            catch (SqlException sqlEx)
            {
                result = new BaseDto<bool>(true, new Exception("Error al ingresar nuevo usuario", sqlEx));
            }
            catch (Exception ex)
            {
                result = new BaseDto<bool>(true, new Exception("Error al ingresar nuevo usuario", ex));
            }

            return result;
        }
        #endregion

        #region UpdateTrabajador
        public BaseDto<bool> UpdateTrabajador(TrabajadorDto p_Obj)
        {
            BaseDto<bool> resultObj = null;

            try
            {
                using (this.dbContext = new MasInfoWebEntities_02())
                {
                    var trabajadorDb = this.dbContext.Trabajador.FirstOrDefault(p => p.Id == p_Obj.Id);

                    if (trabajadorDb != null)
                    {
                        trabajadorDb.Nombre = p_Obj.Nombre;
                        trabajadorDb.ApellidoPaterno = p_Obj.ApellidoPaterno;
                        trabajadorDb.ApellidoMaterno = p_Obj.ApellidoMaterno;
                        trabajadorDb.IdCargo = p_Obj.IdCargo;
                        trabajadorDb.IdCargoFuncion = p_Obj.IdCargoFuncion;
                        trabajadorDb.Email = p_Obj.Email;
                        trabajadorDb.FchUpdate = p_Obj.FchUpdate;
                        trabajadorDb.UsrUpdate = p_Obj.UsrUpdate;

                        this.dbContext.SaveChanges();
                        resultObj = new BaseDto<bool>(true);
                    }
                    else
                    {
                        throw new Exception("Trabajador no encontrado en base de datos");
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

        #region GetListaTrabajadorAll
        public BaseDto<List<TrabajadorDto>> GetListaTrabajadorAll()
        {
            BaseDto<List<TrabajadorDto>> objResult = null;

            try
            {
                using (this.dbContext = new Database.MasInfoWebEntities_02())
                {
                    var lstResult = (from usr in this.dbContext.Trabajador
                                     join c in this.dbContext.Cargo on usr.IdCargo equals c.Id
                                     select new TrabajadorDto
                                     {
                                         Id = usr.Id,
                                         Rut = usr.Rut,
                                         Nombre = usr.Nombre,
                                         ApellidoPaterno = usr.ApellidoPaterno,
                                         ApellidoMaterno = usr.ApellidoMaterno,
                                         IdCargo = usr.IdCargo,
                                         UsrCreate = usr.UsrCreate,
                                         Email = usr.Email,
                                         IdUsuario = usr.IdUsuario,
                                         IdEstado = usr.IdEstado,
                                         DetalleCargo = new CargoDto
                                         {
                                             Id = c == null ? 0 : c.Id,
                                             Descripcion = c.Descripcion,                                             
                                         },
                                     }).ToList();
                    objResult = new BaseDto<List<TrabajadorDto>>(lstResult);
                }
            }
            catch (SqlException sqlEx)
            {
                objResult = new BaseDto<List<TrabajadorDto>>(true, sqlEx);
            }
            catch (Exception ex)
            {
                objResult = new BaseDto<List<TrabajadorDto>>(true, ex);
            }

            return objResult;
        }
        #endregion

        #region GetListaTrabajadorbyFiltro
        public BaseDto<List<TrabajadorDto>> GetListaTrabajadorbyFiltro(TrabajadorDto p_Filtro)
        {
            BaseDto<List<TrabajadorDto>> objResult = null;

            try
            {
                using (this.dbContext = new Database.MasInfoWebEntities_02())
                {
                    var lstResult = (from t in this.dbContext.Trabajador
                                     join es in this.dbContext.Estado on t.IdEstado equals es.Id
                                     join u in this.dbContext.Usuario on t.IdUsuario equals u.Id
                                     join r in this.dbContext.Rol on u.IdRol equals r.Id
                                     where (t.IdEstado == p_Filtro.FiltroIdEstado || p_Filtro.FiltroIdEstado == null) &&
                                           (u.IdRol == p_Filtro.FiltroIdRol || p_Filtro.FiltroIdRol == null) &&
                                           (u.Username.Contains(p_Filtro.FiltroUsername)|| string.IsNullOrEmpty(p_Filtro.FiltroUsername)) &&
                                           u.Habilitado == true
                                     select new TrabajadorDto
                                     {
                                         Id = t.Id,
                                         Rut = t.Rut,
                                         Nombre = t.Nombre,
                                         ApellidoPaterno = t.ApellidoPaterno,
                                         Email = t.Email,
                                         IdEstado = t.IdEstado,
                                         UsrCreate = t.UsrCreate,
                                         FchCreate = t.FchCreate,
                                         FchUpdate = t.FchUpdate,
                                         UsrUpdate = t.UsrUpdate,
                                         IdUsuario = t.IdUsuario,
                                         IdCargo = t.IdCargo,
                                         DetalleEstado = new EstadoDto
                                         {
                                             Id = es.Id,
                                             Descripcion = es.Descripcion,
                                             Tabla = es.Tabla
                                         },
                                         DatosUsuario = new UsuarioDto
                                         {
                                             Id = u.Id,
                                             Username = u.Username,                                           
                                             IdRol = u.IdRol,
                                         },
                                         DetalleRol = new RolDto
                                         {
                                             Id = r.Id,
                                             Descripcion = r.Descripcion
                                         }
                                     }).ToList();
                    PaginadorDto datosPaginado = p_Filtro.DatosPaginado;
                    var lstPaginada = DataPaginationUtils.GetPagedList<TrabajadorDto>(ref datosPaginado, lstResult);
                    objResult = new BaseDto<List<TrabajadorDto>>(true, datosPaginado, lstPaginada);
                }
            }
            catch (SqlException sqlEx)
            {
                objResult = new BaseDto<List<TrabajadorDto>>(true, sqlEx);
            }
            catch (Exception ex)
            {
                objResult = new BaseDto<List<TrabajadorDto>>(true, ex);
            }

            return objResult;
        }
        #endregion

        #region GetTrabajadorById
        public BaseDto<TrabajadorDto> GetTrabajadorById(TrabajadorDto p_Filtro)
        {
            BaseDto<TrabajadorDto> objResult = null;

            try
            {
                using (this.dbContext = new MasInfoWebEntities_02())
                {//left join al conjunto A sobre B se almacena en un campo temporal
                    var trabajadorDb = (from t in this.dbContext.Trabajador
                                        join es in this.dbContext.Estado on t.IdEstado equals es.Id
                                        join u in this.dbContext.Usuario on t.IdUsuario equals u.Id
                                        join r in this.dbContext.Rol on u.IdRol equals r.Id
                                        join tel in this.dbContext.Telefono on u.Id equals tel.IdUsuario
                                        join cf in this.dbContext.CargoFuncion on t.IdCargoFuncion equals cf.Id into tmpCf
                                        join c in this.dbContext.Cargo on t.IdCargo equals c.Id into tmpC
                                        from cf in tmpCf.DefaultIfEmpty()
                                        from c in tmpC.DefaultIfEmpty()
                                        where t.Id == p_Filtro.FiltroId
                                        select new TrabajadorDto
                                        {
                                            Id = t.Id,
                                            Rut = t.Rut,
                                            Nombre = t.Nombre,
                                            ApellidoPaterno = t.ApellidoPaterno,
                                            ApellidoMaterno = t.ApellidoMaterno,
                                            FchCreate = t.FchCreate,
                                            UsrCreate = t.UsrCreate,
                                            FchUpdate = t.FchUpdate,
                                            UsrUpdate = t.UsrUpdate,
                                            IdCargo = c.Id,
                                            Email = t.Email,
                                            IdUsuario = t.IdUsuario,
                                            IdEstado = t.IdEstado,
                                            IdCargoFuncion = cf.Id,
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
                                            },
                                            DetalleCargo = new CargoDto
                                            {
                                                Id = c == null ? 0 : c.Id,
                                                Descripcion = c.Descripcion,
                                                IdCargoFuncion = c == null ? 0 : c.IdCargoFuncion
                                            },
                                            DetalleFuncion = new CargoFuncionDto
                                            {
                                                Id = cf == null ? 0 : cf.Id,
                                                Descripcion = cf.Descripcion
                                            },
                                            DetalleTelefono = new TelefonoDto
                                            {
                                                Id = tel.Id,
                                                NumeroTelefono = tel.NumeroTelefono
                                            },
                                        }).FirstOrDefault();

                    objResult = new BaseDto<TrabajadorDto>(trabajadorDb);
                }
            }
            catch (SqlException sqlEx)
            {
                objResult = new BaseDto<TrabajadorDto>(true, sqlEx);
            }
            catch (Exception ex)
            {
                objResult = new BaseDto<TrabajadorDto>(true, ex);
            }

            return objResult;
        }
        #endregion

        #region GetTrabajadorByRut
        public BaseDto<TrabajadorDto> GetTrabajadorByRut(TrabajadorDto p_Filtro)
        {
            BaseDto<TrabajadorDto> objResult = null;

            try
            {
                using (this.dbContext = new MasInfoWebEntities_02())
                {
                    var trabajadorDb = (from p in this.dbContext.Trabajador
                                        join es in this.dbContext.Estado on p.IdEstado equals es.Id
                                        where p.Rut == p_Filtro.FiltroRut &&
                                        p.IdEstado == p_Filtro.FiltroIdEstado
                                        select new TrabajadorDto
                                        {
                                            Id = p.Id,
                                            Rut = p.Rut,
                                            Nombre = p.Nombre,
                                            ApellidoPaterno = p.ApellidoPaterno,
                                            ApellidoMaterno = p.ApellidoMaterno,
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

                    objResult = new BaseDto<TrabajadorDto>(trabajadorDb);
                }
            }
            catch (SqlException sqlEx)
            {
                objResult = new BaseDto<TrabajadorDto>(true, sqlEx);
            }
            catch (Exception ex)
            {
                objResult = new BaseDto<TrabajadorDto>(true, ex);
            }

            return objResult;
        }
        #endregion

        #region Delete
        public BaseDto<bool> Delete(TrabajadorDto p_Obj)
        {
            BaseDto<bool> result = null;

            try
            {
                using (this.dbContext = new MasInfoWebEntities_02())
                {
                    //Obtener el objeto origen desde base de datos
                    //El metodo .FirstOrDefault, retorna el primer objeto encontrado de acuerdo
                    //a un determinado filtro de búsqueda, y en caso contrario, retorna null
                    var objOrigenDb = this.dbContext.Trabajador.FirstOrDefault(c => c.Id == p_Obj.Id);

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
                        throw new Exception("Trabajador no encontrado en base de datos");
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

        #region GetTrabajadorByUsuarioId
        public BaseDto<TrabajadorDto> GetTrabajadorByUsuarioId(TrabajadorDto p_Filtro)
        {
            BaseDto<TrabajadorDto> objResult = null;

            try
            {
                using (this.dbContext = new MasInfoWebEntities_02())
                {
                    var trabajadorDb = (from t in this.dbContext.Trabajador
                                        join es in this.dbContext.Estado on t.IdEstado equals es.Id
                                        join u in this.dbContext.Usuario on t.IdUsuario equals u.Id
                                        join r in this.dbContext.Rol on u.IdRol equals r.Id
                                        where t.IdUsuario == p_Filtro.FiltroIdUsuario &&
                                        (t.IdEstado == p_Filtro.FiltroIdEstado || p_Filtro.FiltroIdEstado == null)
                                        select new TrabajadorDto
                                        {
                                            Id = t.Id,
                                            Rut = t.Rut,
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
                                            },
                                        }).FirstOrDefault();

                    objResult = new BaseDto<TrabajadorDto>(trabajadorDb);
                }
            }
            catch (SqlException sqlEx)
            {
                objResult = new BaseDto<TrabajadorDto>(true, sqlEx);
            }
            catch (Exception ex)
            {
                objResult = new BaseDto<TrabajadorDto>(true, ex);
            }

            return objResult;
        }
        #endregion

        #endregion
    }
}
