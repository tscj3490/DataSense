using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace DataSense_UI.App_Start
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/scripts").Include(
              "~/Scripts/jquery.min.js",
              "~/Scripts/popper.min.js",
              "~/Scripts/bootstrap.min.js",
              "~/Scripts/custom.js",
              "~/Scripts/kendo.all.min.js",
              "~/Scripts/kendo.aspnetmvc.min.js"
              ));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.min.css",
                      "~/Content/responsive.css",
                      "~/Content/style.css",
                       "~/Content/kendo.common.min.css",
                      "~/Content/kendo.rtl.min.css",
                      "~/Content/kendo.blueopal.min.css",
                      "~/Content/kendo.dataviz.min.css",
                      "~/Content/kendo.dataviz.default.min.css"
                      ));
        }
    }
}