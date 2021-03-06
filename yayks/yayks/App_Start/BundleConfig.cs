﻿using System.Web;
using System.Web.Optimization;

namespace yayks
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                "~/Scripts/startbootstrap-sb-admin-2-gh-pages/vendor/metisMenu/metisMenu.min.js",
                "~/Scripts/startbootstrap-sb-admin-2-gh-pages/dist/js/sb-admin-2.min.js",
                "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                     "~/Content/bootstrap.min.css",
                        "~/Content/font-awesome.min.css",
                        "~/Scripts/startbootstrap-sb-admin-2-gh-pages/dist/css/sb-admin-2.min.css",
                        "~/Scripts/startbootstrap-sb-admin-2-gh-pages/vendor/metisMenu/metisMenu.min.css",
                      "~/Content/site.css"));
        }
    }
}
