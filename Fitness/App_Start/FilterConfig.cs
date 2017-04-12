using System.Web;
using System.Web.Mvc;

namespace Fitness
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            //get rid of this to get custom default error page instead of shared/error
            filters.Add(new HandleErrorAttribute());
        }
    }
}
