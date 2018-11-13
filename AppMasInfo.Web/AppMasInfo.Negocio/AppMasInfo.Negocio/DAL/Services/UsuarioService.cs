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
    public class UsuarioService : IUsuarioService
    {
        #region propiedades privadas
        private static UsuarioService Instance = null;
        private Database.masInfoWebEntities dbContext = null;
        #endregion

        //Permite el acceso global a dicha instancia mediante un método de clase.
        #region singleton
        private UsuarioService() { }

        public static UsuarioService GetInstance()
        {
            if (UsuarioService.Instance == null)
                UsuarioService.Instance = new UsuarioService();

            return UsuarioService.Instance;
        }
        #endregion

        #region metodos publicos

        #region GetUsuarioByCredentials
        public UsuarioDto GetUsuarioByCredentials(UsuarioDto p_Filtro)
        {
            UsuarioDto objResult = null;

            try
            {
                using (this.dbContext = new Database.masInfoWebEntities())
                {
                    //El metodo .FirstOrDefault, retorna el primer objeto encontrado de acuerdo
                    //a un determinado filtro de búsqueda, y en caso contrario, retorna null
                    var usuarioDb = (from u in this.dbContext.Usuario.Include("Trabajador").Include("Rol")
                                     join r in this.dbContext.Rol on u.IdRol equals r.Id
                                     where u.Username == p_Filtro.Username &&
                                           u.Pass == p_Filtro.Pass
                                     select u).FirstOrDefault();

                    if (usuarioDb != null)
                    {
                        objResult = new UsuarioDto
                        {
                            // Asignar propiedades
                            Id = usuarioDb.Id,
                            Username = usuarioDb.Username,
                            IdRol = usuarioDb.IdRol,

                            DetalleRol = new RolDto
                            {
                                Id = usuarioDb.Rol.Id,
                                Descripcion = usuarioDb.Rol.Descripcion
                            },
                            //se retorna una lista completa de trabajdor con sus propiedades para ser utilizadas para la sesion del usuario actual
                            ListaTrabajador = (from ut in usuarioDb.Trabajador
                                               select new TrabajadorDto
                                               {
                                                   Nombre = ut.Nombre,
                                                   ApellidoPaterno = ut.ApellidoPaterno,
                                                   ApellidoMaterno = ut.ApellidoMaterno,
                                                   Email = ut.Email
                                               }).ToList()
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener el objeto en la busqueda ", ex);
            }

            return objResult;
        }
        #endregion

        #region GetUsuarioByUsername
        public BaseDto<UsuarioDto> GetUsuarioByUsername(UsuarioDto p_Filtro)
        {
            BaseDto<UsuarioDto> objResult = null;

            try
            {
                using (this.dbContext = new Database.masInfoWebEntities())
                {
                    //El metodo .FirstOrDefault, retorna el primer objeto encontrado de acuerdo
                    //a un determinado filtro de búsqueda, y en caso contrario, retorna null
                    var usuarioDb = (from u in this.dbContext.Usuario
                                     where u.Username == p_Filtro.FiltroUsername
                                     select new UsuarioDto
                                     {
                                         Id = u.Id,
                                         Username = u.Username,
                                         IdRol = u.IdRol,
                                         Pass = u.Pass
                                     }).FirstOrDefault();

                    objResult = new BaseDto<UsuarioDto>(usuarioDb);
                }
            }
            catch (SqlException sqlEx)
            {
                objResult = new BaseDto<UsuarioDto>(true, sqlEx);
            }
            catch (Exception ex)
            {
                objResult = new BaseDto<UsuarioDto>(true, ex);
            }

            return objResult;
        }
        #endregion

        #region InsertarUsuario
        public BaseDto<bool> InsertarUsuario(UsuarioDto p_Obj)
        {
            BaseDto<bool> result = null;

            try
            {
                using (this.dbContext = new masInfoWebEntities())
                {
                    var usuarioDb = this.dbContext.Usuario.Add(
                        new Database.Usuario
                        {
                            Username = p_Obj.Username,
                            Pass = p_Obj.Pass,
                            IdRol = p_Obj.IdRol
                        });

                    // Guardamos los cambios en base de datos
                    this.dbContext.SaveChanges();
                    //var idusr = (from u in this.dbContext.Usuario
                    //             where u.Username == p_Obj.Username
                    //             select u).SingleOrDefault();

                    //p_Obj.Id = idusr.Id;
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

        #endregion
    }
}
