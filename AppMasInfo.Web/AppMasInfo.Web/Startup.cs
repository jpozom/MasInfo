using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(AppMasInfo.Web.Startup))]

namespace AppMasInfo.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=316888
            ConfigureAuth(app);

            //Configuracioon de mensajes de validacion
            ClientDataTypeModelValidatorProvider.ResourceClassKey = "Mensajes";
            DefaultModelBinder.ResourceClassKey = "Mensajes";
        }
    }
}
