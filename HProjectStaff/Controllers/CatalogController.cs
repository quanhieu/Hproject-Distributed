﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HProjectStaff.Controllers
{
    public class CatalogController : Controller
    {
        // GET: Catalog
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Category()
        {
            return View();
        }

        public ActionResult Product()
        {
            return View();
        }
    }
}