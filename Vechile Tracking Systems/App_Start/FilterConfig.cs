using System.Web;
using System.Web.Mvc;

namespace Vechile_Tracking_Systems
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
