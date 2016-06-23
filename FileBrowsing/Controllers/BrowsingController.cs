using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FileBrowsing.Controllers
{
    public class BrowsingController : Controller
    {
        // GET: Browsing
        public ActionResult Index()
        {
            return View();
        }
    }
}