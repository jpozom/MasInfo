using AppMasInfo.Negocio.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppMasInfo.Negocio.DAL.Services
{
    public class CargoService : ICargoService
    {
        #region propiedades privadas
        private static CargoService Instance = null;
        private Database.MasInfoWebEntities dbContext = null;
        #endregion

        #region singleton
        private CargoService() { }

        public static CargoService GetInstance()
        {
            if (CargoService.Instance == null)
                CargoService.Instance = new CargoService();

            return CargoService.Instance;
        }
        #endregion

        #region GetListaCargoAll
        public BaseDto<List<CargoDto>> GetListaCargoAll()
        {
            BaseDto<List<CargoDto>> lstResult = null;

            try
            {
                using (this.dbContext = new Database.MasInfoWebEntities())
                {
                    var objResult = (from c in this.dbContext.Cargo
                                     select new CargoDto
                                     {
                                         Id = c.Id,                                         
                                         Descripcion = c.Descripcion,  
                                         IdCargoFuncion = c.IdCargoFuncion
                                     }).ToList();

                    lstResult = new BaseDto<List<CargoDto>>(objResult);
                }
            }
            catch (SqlException sqlEx)
            {
                lstResult = new BaseDto<List<CargoDto>>(true, sqlEx);
            }
            catch (Exception ex)
            {
                lstResult = new BaseDto<List<CargoDto>>(true, ex);
            }

            return lstResult;
        }
        #endregion
    }
}
