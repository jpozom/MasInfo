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
    public class TipoTelefonoService : ITipoTelefonoService
    {
        #region propiedades privadas
        private static TipoTelefonoService Instance = null;
        private Database.MasInfoWebEntities_02 dbContext = null;
        #endregion

        #region singleton
        private TipoTelefonoService() { }

        public static TipoTelefonoService GetInstance()
        {
            if (TipoTelefonoService.Instance == null)
                TipoTelefonoService.Instance = new TipoTelefonoService();

            return TipoTelefonoService.Instance;
        }
        #endregion

        #region GetListaTipoTelefonoAll
        public BaseDto<List<TipoTelefonoDto>> GetListaTipoTelefonoAll()
        {
            BaseDto<List<TipoTelefonoDto>> lstResult = null;

            try
            {
                using (this.dbContext = new Database.MasInfoWebEntities_02())
                {
                    var tipoTelefonoResult = (from t in this.dbContext.TipoTelefono
                                     select new TipoTelefonoDto
                                     {
                                         IdTipoTelefono = t.IdTipoTelefono,
                                         Descripcion = t.Descripcion
                                     }).ToList();
                    lstResult = new BaseDto<List<TipoTelefonoDto>>(tipoTelefonoResult);
                }
            }
            catch (SqlException sqlEx)
            {
                lstResult = new BaseDto<List<TipoTelefonoDto>>(true, sqlEx);
            }
            catch (Exception ex)
            {
                lstResult = new BaseDto<List<TipoTelefonoDto>>(true, ex);
            }

            return lstResult;
        }
        #endregion
    }
}
