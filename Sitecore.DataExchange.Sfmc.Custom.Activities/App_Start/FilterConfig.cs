using System.Web;
using System.Web.Mvc;

namespace Sitecore.DataExchange.Sfmc.Custom.Activities
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
