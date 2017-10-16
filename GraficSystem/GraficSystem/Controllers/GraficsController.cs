using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using System.Net;

namespace GraficSystem.Controllers
{
    public class GraficsController : Controller
    {
        // GET: Grafics
        public ActionResult Index()
        {
            return View();
        }
        
    }
}