using AppMasInfo.Negocio.DAL.Entities;
using AppMasInfo.Negocio.DAL.Services;
using AppMasInfo.Utils.Utils;
using AppMasInfo.Web.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace AppMasInfo.Web.Controllers
{
    public class AccountController : Controller
    {
        #region Propiedades Privadas
        private IUsuarioService UsuarioServiceModel
        {
            get { return UsuarioService.GetInstance(); }
        }

        #endregion

        #region Metodos Publicos

        #region Index
        // GET: Account        
        public ActionResult Index()
        {
            return View();
        }
        #endregion

        #region Login
        [AllowAnonymous]
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public ActionResult Login(AccountViewModel p_ViewModel)
        {
            try
            {
                // Limpiamos los mensajes temporales
                TempData.Clear();

                if (ModelState.IsValid)
                {
                    //creamos un objeto usuario que nos devuelva todos los usuarios almacenados
                    UsuarioDto userInfo = new UsuarioDto
                    {
                        Username = p_ViewModel.Username,
                        Pass = p_ViewModel.Pass
                    };

                    //creamos una condicion que valide que los objetos no se devuelvan nulos
                    if (!string.IsNullOrEmpty(userInfo.Username) || !string.IsNullOrEmpty(userInfo.Pass))
                    {
                        var userFilterObj = new UsuarioDto();
                        userFilterObj.Username = p_ViewModel.Username;
                        userFilterObj.Pass = GlobalMethods.EncryptPass(p_ViewModel.Pass);

                        // Enviamos el objeto al servicio
                        var userDb = UsuarioServiceModel.GetUsuarioByCredentials(userFilterObj);

                        // Si el usuario es distinto de nulo, significa que esta autenticado en el sistema
                        if (userDb != null)
                        {
                            ////string nombreCompleto = string.Format("{0} {1} {2}",
                            //    userDb.ListaTrabajador.FirstOrDefault().Nombre,
                            //    userDb.ListaTrabajador.FirstOrDefault().ApellidoPaterno,
                            //    userDb.ListaTrabajador.FirstOrDefault().ApellidoMaterno,
                            //    userDb.ListaTrabajador.FirstOrDefault().Email);
                            
                            // Generamos la identidad con Owin
                            var identity = new ClaimsIdentity(DefaultAuthenticationTypes.ApplicationCookie);
                            identity.AddClaims(new List<Claim>
                            {
                                new Claim(ClaimTypes.NameIdentifier, userDb.Username),
                                //new Claim(ClaimTypes.Name, nombreCompleto),
                                new Claim(ClaimTypes.Role, userDb.DetalleRol.Descripcion),
                                //new Claim(ClaimTypes.Email, userInfo.),
                            });
                            HttpContext.GetOwinContext().Authentication.SignIn(identity);
                            //userInfo.AccesoPaginas = userDb.Value.DetalleRol.AccesoPaginasWeb;
                            //SessionStore.Store<ContextoDto>(SessionKeys.UsuarioSessionKey, userInfo);

                            return RedirectToAction("Index", "Home");
                        }
                        else
                        {
                            TempData["ErrorMessage"] = "Usuario y/o Password incorrectos";
                        }
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Usuario y/o Password incorrectos";
                    }
                }
                else
                {
                    TempData["ErrorMessage"] = "Existen campos sin ingresar";
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al ingresar", ex);
            }

            return View(p_ViewModel);
        }
        #endregion

        #region Logout
        [HttpGet]
        public ActionResult Logout()
        {
            Session.Abandon();
            HttpContext.GetOwinContext().Authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie);

            return RedirectToAction("Login");
        }
        #endregion
        #endregion
    }
}