using AppMasInfo.Negocio.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppMasInfo.Negocio.DAL.Services
{
    public class UbicacionService : IUbicacionService
    {
        #region propiedades privadas
        private static UbicacionService Instance = null;
        private Database.MasInfoWebEntities_02 dbContext = null;
        #endregion

        #region singleton
        private UbicacionService() { }

        public static UbicacionService GetInstance()
        {
            if (UbicacionService.Instance == null)
                UbicacionService.Instance = new UbicacionService();

            return UbicacionService.Instance;
        }
        #endregion

        #region GetListaUbicacionAll
        public BaseDto<List<UbicacionDto>> GetListaUbicacionAll()
        {
            BaseDto<List<UbicacionDto>> lstResult = null;

            try
            {
                using (this.dbContext = new Database.MasInfoWebEntities_02())
                {
                    var rolResult = (from ub in this.dbContext.Ubicacion
                                     select new UbicacionDto
                                     {
                                         Id = ub.Id,
                                         Descripcion = ub.Descripcion
                                     }).ToList();
                    lstResult = new BaseDto<List<UbicacionDto>>(rolResult);
                }
            }
            catch (SqlException sqlEx)
            {
                lstResult = new BaseDto<List<UbicacionDto>>(true, sqlEx);
            }
            catch (Exception ex)
            {
                lstResult = new BaseDto<List<UbicacionDto>>(true, ex);
            }

            return lstResult;
        }
        #endregion
    }
}
