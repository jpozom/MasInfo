using AppMasInfo.Negocio.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppMasInfo.Negocio.DAL.Services
{
    public class CargoFuncionService : ICargoFuncionService
    {
        #region propiedades privadas
        private static CargoFuncionService Instance = null;
        private Database.MasInfoWebEntities_02 dbContext = null;
        #endregion

        #region singleton
        private CargoFuncionService() { }

        public static CargoFuncionService GetInstance()
        {
            if (CargoFuncionService.Instance == null)
                CargoFuncionService.Instance = new CargoFuncionService();

            return CargoFuncionService.Instance;
        }
        #endregion

        #region GetListaCargoFuncionAll
        public BaseDto<List<CargoFuncionDto>> GetListaCargoFuncionAll()
        {
            BaseDto<List<CargoFuncionDto>> lstResult = null;

            try
            {
                using (this.dbContext = new Database.MasInfoWebEntities_02())
                {
                    var objResult = (from cf in this.dbContext.CargoFuncion
                                     select new CargoFuncionDto
                                     {
                                         Id = cf.Id,
                                         Descripcion = cf.Descripcion,                                         
                                     }).ToList();

                    lstResult = new BaseDto<List<CargoFuncionDto>>(objResult);
                }
            }
            catch (SqlException sqlEx)
            {
                lstResult = new BaseDto<List<CargoFuncionDto>>(true, sqlEx);
            }
            catch (Exception ex)
            {
                lstResult = new BaseDto<List<CargoFuncionDto>>(true, ex);
            }

            return lstResult;
        }
        #endregion
    }
}
