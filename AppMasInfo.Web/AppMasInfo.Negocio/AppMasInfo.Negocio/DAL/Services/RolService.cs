using AppMasInfo.Negocio.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppMasInfo.Negocio.DAL.Services
{
    public class RolService : IRolService
    {
        #region propiedades privadas
        private static RolService Instance = null;
        private Database.masInfoWebEntities dbContext = null;
        #endregion

        #region singleton
        private RolService() { }

        public static RolService GetInstance()
        {
            if (RolService.Instance == null)
                RolService.Instance = new RolService();

            return RolService.Instance;
        }
        #endregion

        #region GetListaRolAll
        public BaseDto<List<RolDto>> GetListaRolAll()
        {
            BaseDto<List<RolDto>> lstResult = null;

            try
            {
                using (this.dbContext = new Database.masInfoWebEntities())
                {                    
                     var rolResult = (from r in this.dbContext.Rol
                                   select new RolDto
                                   {
                                       Id = r.Id,
                                       Descripcion = r.Descripcion
                                   }).ToList();
                    lstResult = new BaseDto<List<RolDto>>(rolResult);
                }
            }
            catch (SqlException sqlEx)
            {
                lstResult = new BaseDto<List<RolDto>>(true, sqlEx);
            }
            catch (Exception ex)
            {
                lstResult = new BaseDto<List<RolDto>>(true, ex);
            }

            return lstResult;
        }
        #endregion
    }
}
