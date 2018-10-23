using System.Web;
using System.Web.Optimization;

namespace AppMasInfo.Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            //bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
            //            "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery.min.js",
                        "~/Scripts/bootstrap.min.js",
                        "~/Scripts/bootstrap-dialog.js",
                        "~/Scripts/modernizr.min.js",
                        "~/Scripts/detect.js",
                        "~/Scripts/fastclick.js",
                        "~/Scripts/jquery.slimscroll.js",
                        "~/Scripts/jquery.blockUI.js",
                        "~/Scripts/waves.js",
                        "~/Scripts/wow.min.js",
                        "~/Scripts/jquery.nicescroll.js",
                        "~/Scripts/jquery.scrollTo.min.js",
                        "~/Scripts/spin.min.js",
                        "~/Scripts/app.js",
                        "~/Scripts/deloitte-core.js"));            
        }
    }
}
