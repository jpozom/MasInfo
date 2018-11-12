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
        private Database.masInfoWebEntities dbContext = null;
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
                using (this.dbContext = new masInfoWebEntities())
                {
                    using (var trx = new TransactionScope())
                    {
                        var usuarioDb = this.dbContext.Usuario.Add(
                            new Database.Usuario
                            {
                                Username = p_Obj.DatosUsuario.Username,
                                Pass = p_Obj.DatosUsuario.Pass,
                                IdRol = p_Obj.DatosUsuario.IdRol
                            });

                        this.dbContext.SaveChanges();
                        // Si el IdUsuario es mayor a cero, entonces se ingresó correctamente
                        // el registro en base de datos.
                        if (usuarioDb.Id > 0)
                        {
                            this.dbContext.Trabajador.Add(new Trabajador
                            {
                                IdUsuario = usuarioDb.Id,
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
                        }

                        // Guardamos los cambios en base de datos
                        this.dbContext.SaveChanges();

                        // Completamos la transacción
                        trx.Complete();

                        // Asignamos un valor a la variable result describiendo la ejecución correcta
                        result = new BaseDto<bool>(true);
                    }
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
                using (this.dbContext = new masInfoWebEntities())
                {
                    var trabajadorDb = this.dbContext.Trabajador.FirstOrDefault(p => p.Id == p_Obj.Id);
                    var usuarioDb = this.dbContext.Usuario.FirstOrDefault(us => us.Id == p_Obj.DatosUsuario.Id);

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

                        if (usuarioDb != null)
                        {
                            //si el pass que viene del formulario edit viene vacio mantiene la contraseña encriptada actual
                            // si no cambia la contraseña actual por la ingresada en el formulario edit
                            if (p_Obj.DatosUsuario.Pass != "")
                            {
                                usuarioDb.Pass = p_Obj.DatosUsuario.Pass;                                
                            }

                            usuarioDb.IdRol = p_Obj.DatosUsuario.IdRol;
                            usuarioDb.Username = p_Obj.DatosUsuario.Username;
                        }
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
                using (this.dbContext = new Database.masInfoWebEntities())
                {
                    var lstResult = (from usr in this.dbContext.Trabajador
                                     select new TrabajadorDto
                                     {
                                         Id = usr.Id,
                                         Nombre = usr.Nombre,
                                         ApellidoPaterno = usr.ApellidoPaterno,
                                         ApellidoMaterno = usr.ApellidoMaterno,
                                         IdCargo = usr.IdCargo,
                                         UsrCreate = usr.UsrCreate,
                                         Email = usr.Email,
                                         IdUsuario = usr.IdUsuario,
                                         IdEstado = usr.IdEstado
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
                using (this.dbContext = new Database.masInfoWebEntities())
                {
                    var lstResult = (from t in this.dbContext.Trabajador
                                     join es in this.dbContext.Estado on t.IdEstado equals es.Id
                                     join u in this.dbContext.Usuario on t.IdUsuario equals u.Id
                                     join r in this.dbContext.Rol on u.IdRol equals r.Id
                                     where (t.IdUsuario == p_Filtro.FiltroIdUsuario || p_Filtro.FiltroIdUsuario == null) &&
                                           (t.Nombre.Contains(p_Filtro.FiltroNombre) || string.IsNullOrEmpty(p_Filtro.FiltroNombre)) &&
                                           (t.IdEstado == p_Filtro.FiltroIdEstado || p_Filtro.FiltroIdEstado == null) &&
                                           (u.IdRol == p_Filtro.FiltroIdRol || p_Filtro.FiltroIdRol == null)
                                     select new TrabajadorDto
                                     {
                                         Id = t.Id,
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
                                             Pass = u.Pass,
                                             IdRol = u.IdRol
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
                using (this.dbContext = new masInfoWebEntities())
                {
                    var trabajadorDb = (from t in this.dbContext.Trabajador
                                        join es in this.dbContext.Estado on t.IdEstado equals es.Id
                                        join u in this.dbContext.Usuario on t.IdUsuario equals u.Id
                                        join r in this.dbContext.Rol on u.IdRol equals r.Id
                                        join cf in this.dbContext.CargoFuncion on t.IdCargoFuncion equals cf.Id
                                        join c in this.dbContext.Cargo on t.IdCargo equals c.Id
                                        where t.Id == p_Filtro.FiltroId

                                        select new TrabajadorDto
                                        {
                                            Id = t.Id,
                                            Nombre = t.Nombre,
                                            ApellidoPaterno = t.ApellidoPaterno,
                                            ApellidoMaterno = t.ApellidoMaterno,
                                            FchCreate = t.FchCreate,
                                            UsrCreate = t.UsrCreate,
                                            FchUpdate = t.FchUpdate,
                                            UsrUpdate = t.UsrUpdate,
                                            IdCargo = t.IdCargo,
                                            Email = t.Email,
                                            IdUsuario = t.IdUsuario,
                                            IdEstado = t.IdEstado,
                                            IdCargoFuncion = t.IdCargoFuncion,
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
                                                Id = c.Id,
                                                Descripcion = c.Descripcion,
                                                IdCargoFuncion = c.IdCargoFuncion
                                            },
                                            DetalleFuncion = new CargoFuncionDto
                                            {
                                                Id = cf.Id,
                                                Descripcion = cf.Descripcion
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
                using (this.dbContext = new masInfoWebEntities())
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

        #endregion
    }
}
