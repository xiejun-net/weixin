using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace sohovan.com.wxdemo.Controllers
{
    public class SharedController : Controller
    {
        //
        // GET: /Shared/

        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /Shared/

        public ActionResult Error()
        {
            return View();
        }

    }
}
