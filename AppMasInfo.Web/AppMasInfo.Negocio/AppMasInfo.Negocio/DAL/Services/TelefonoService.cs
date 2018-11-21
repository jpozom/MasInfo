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
    public class TelefonoService : ITelefonoService
    {
        #region propiedades privadas
        private static TelefonoService Instance = null;
        private Database.MasInfoWebEntities_02 dbContext = null;
        #endregion

        #region singleton
        private TelefonoService() { }

        public static TelefonoService GetInstance()
        {
            if (TelefonoService.Instance == null)
                TelefonoService.Instance = new TelefonoService();

            return TelefonoService.Instance;
        }
        #endregion

        #region InsertarTelefono
        public BaseDto<bool> InsertarTelefono(TelefonoDto p_Obj)
        {
            BaseDto<bool> result = null;

            try
            {
                using (this.dbContext = new MasInfoWebEntities_02())
                {
                    var telefonoDb = this.dbContext.Telefono.Add(
                        new Database.Telefono
                        {
                            NumeroTelefono = p_Obj.NumeroTelefono,
                            IdTipoTelefono = p_Obj.IdTipoTelefono,                           
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

        #region GetTelefonoByIdUsuario
        public BaseDto<TelefonoDto> GetTelefonoByIdUsuario(TelefonoDto p_Filtro)
        {
            BaseDto<TelefonoDto> objResult = null;

            try
            {
                using (this.dbContext = new MasInfoWebEntities_02())
                {
                    var telefonoDb = (from tel in this.dbContext.Telefono
                                   join u in this.dbContext.Usuario on tel.IdUsuario equals u.Id
                                   where tel.IdUsuario == p_Filtro.FiltroIdUsuario
                                   select new TelefonoDto
                                   {
                                       Id = tel.Id,
                                       IdTipoTelefono = tel.IdTipoTelefono,
                                       NumeroTelefono = tel.NumeroTelefono,
                                       IdUsuario = tel.IdUsuario                                       
                                   }).FirstOrDefault();

                    objResult = new BaseDto<TelefonoDto>(telefonoDb);
                }
            }
            catch (SqlException sqlEx)
            {
                objResult = new BaseDto<TelefonoDto>(true, sqlEx);
            }
            catch (Exception ex)
            {
                objResult = new BaseDto<TelefonoDto>(true, ex);
            }

            return objResult;
        }
        #endregion

        #region UpdateTelefono
        public BaseDto<bool> UpdateTelefono(TelefonoDto p_Obj)
        {
            BaseDto<bool> resultObj = null;

            try
            {
                using (this.dbContext = new MasInfoWebEntities_02())
                {
                    var telefonoDb = this.dbContext.Telefono.FirstOrDefault(us => us.Id == p_Obj.Id);

                    if (telefonoDb != null)
                    {
                        telefonoDb.NumeroTelefono = p_Obj.NumeroTelefono;                       
                        telefonoDb.IdTipoTelefono = p_Obj.IdTipoTelefono;

                        this.dbContext.SaveChanges();
                        resultObj = new BaseDto<bool>(true);
                    }
                    else
                    {
                        throw new Exception("Datos de la tabla Telefono no encontrado en base de datos");
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
        public BaseDto<bool> Delete(TelefonoDto p_Obj)
        {
            BaseDto<bool> result = null;

            try
            {
                using (this.dbContext = new MasInfoWebEntities_02())
                {
                    //Obtener el objeto origen desde base de datos
                    //El metodo .FirstOrDefault, retorna el primer objeto encontrado de acuerdo
                    // a un determinado filtro de búsqueda, y en caso contrario, retorna null
                    var objOrigenDb = this.dbContext.Telefono.FirstOrDefault(c => c.Id == p_Obj.Id);

                    if (objOrigenDb != null)
                    {
                        objOrigenDb.NumeroTelefono = p_Obj.NumeroTelefono;
                        objOrigenDb.IdUsuario = p_Obj.IdUsuario;
                        objOrigenDb.IdTipoTelefono = p_Obj.IdTipoTelefono;
                        objOrigenDb.Id = p_Obj.Id;

                        this.dbContext.SaveChanges();
                        result = new BaseDto<bool>(true);
                    }
                    else
                    {
                        throw new Exception("Telefono no encontrado en base de datos");
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
