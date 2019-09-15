using System.Web;
using System.Web.Optimization;

namespace TaskManager
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));


            bundles.Add(new ScriptBundle("~/bundles/Scripts").Include(
                      "~/Scripts/jquery-3.4.1.min.js",
                      "~/Scripts/alertify.min.js",
                      "~/Scripts/bootstrap.min.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.min.css",
                      "~/Content/mdb.min.css",
                      "~/Content/alertifyjs/themes/bootstrap.min.css",
                      "~/Content/alertifyjs/alertify.min.css",
                      "~/Content/site.css"));
        }
    }
}
