using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace eMalicaMVC
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

        }

        public void Application_BeginRequest(object sender, EventArgs e)
        {
            SetCulture();
        }

        private void SetCulture()
        {
            CultureInfo threadCultureInfo = (CultureInfo)CultureInfo.CurrentCulture.Clone();
            CultureInfo uiCultureInfo = (CultureInfo)CultureInfo.CurrentUICulture.Clone();
            threadCultureInfo.DateTimeFormat.ShortDatePattern =
                  ReformatDateTimeFormatString(threadCultureInfo.DateTimeFormat.ShortDatePattern);
            uiCultureInfo.DateTimeFormat.ShortDatePattern =
                  ReformatDateTimeFormatString(uiCultureInfo.DateTimeFormat.ShortDatePattern);
            Thread.CurrentThread.CurrentCulture = threadCultureInfo;
            Thread.CurrentThread.CurrentUICulture = uiCultureInfo;
        }

        private static string ReformatDateTimeFormatString(string format)
        {
            string refromatString = format;

            if (format.Where(x => x == 'd').Count() == 1)
            {
                refromatString = format.Replace("d", "dd");
            }

            if (format.Where(x => x == 'M').Count() == 1)
            {
                refromatString = format.Replace("M", "MM");

            }
            return refromatString;
        }
    }
}
