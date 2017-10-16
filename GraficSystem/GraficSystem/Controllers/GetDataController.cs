using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace GraficSystem.Controllers
{
    public class GetDataController : Controller
    {

        
        // GET: GetData
       
        public ActionResult ConectionDB()
        {
            
            return View();
        }
        [HttpGet]
        public JsonResult conection(string server, string user, string pass)
        {
            SqlHelper helper = new SqlHelper(server, user, pass);
            this.Session["helper"] = helper;
            return Json(true, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult getTables(string table, string catalog)
        {
            SqlHelper temp = this.Session["helper"] as SqlHelper;
            var rows = temp.getDataFromTables(table, catalog).ToList();
            return Json (rows,JsonRequestBehavior.AllowGet);
            

        }

        public ActionResult TablesView()
        {
            return View();
        }
        [HttpGet]
        public JsonResult getdata()
        {
            SqlHelper temp = this.Session["helper"] as SqlHelper;
            return Json(temp.datos.ToList(), JsonRequestBehavior.AllowGet);
        }

        


    }
}