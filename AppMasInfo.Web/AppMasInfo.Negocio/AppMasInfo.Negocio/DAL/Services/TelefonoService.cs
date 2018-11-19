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
        private Database.MasInfoWebEntities dbContext = null;
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
                using (this.dbContext = new MasInfoWebEntities())
                {
                    var telefonoDb = this.dbContext.Telefono.Add(
                        new Database.Telefono
                        {
                            NumeroTelefono = p_Obj.NumeroTelefono,
                            Tipo = p_Obj.Tipo,
                            IdPaciente = p_Obj.IdPaciente,
                            IdTutor = p_Obj.IdTutor                            
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
    }
}
