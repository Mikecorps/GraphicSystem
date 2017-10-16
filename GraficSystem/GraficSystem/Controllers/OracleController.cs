using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Oracle.ManagedDataAccess.Client;

namespace GraficSystem.Controllers
{
    public class OracleController : Controller
    {
        // GET: Oracle
        public ActionResult OracleConections()
        {
            return View();
        }
        [HttpGet]
        public JsonResult conect(string host, string port, string ssid, string user, string pass)
        {
            OracleHelper helper = new OracleHelper(host, port, ssid, user, pass);
            this.Session["helper"] = helper;
            return Json(true, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult getTables()
        {
            OracleHelper temp = this.Session["helper"] as OracleHelper;
            return Json(temp.json, JsonRequestBehavior.AllowGet);
        }
        public JsonResult getData(string table)
        {
            OracleHelper temp = this.Session["helper"] as OracleHelper;
            return Json(temp.getData(table), JsonRequestBehavior.AllowGet);
        }
        public ActionResult OracleDataView()
        {
            return View();
        }
    }
}