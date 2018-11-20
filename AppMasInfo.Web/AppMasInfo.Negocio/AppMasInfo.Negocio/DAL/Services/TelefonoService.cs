﻿using AppMasInfo.Negocio.DAL.Database;
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
    }
}
