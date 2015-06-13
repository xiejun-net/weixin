using System.Web;
using System.Web.Mvc;

namespace sohovan.com.wxweb
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}