using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HProjectStaff.Controllers
{
    public class ManagementController : Controller
    {
        // GET: Management
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Store()
        {
            return View();
        }
        public ActionResult SalePerson()
        {
            return View();
        }
    }
}