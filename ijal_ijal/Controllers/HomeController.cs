using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Linq.Dynamic;
using System.Web.Mvc;
using ijal_ijal.Models;
namespace ijal_ijal.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult GetProduct()
        {
            var draw = Request.Form.GetValues("draw").FirstOrDefault();
            var start = Request.Form.GetValues("start").FirstOrDefault();
            var length = Request.Form.GetValues("length").FirstOrDefault();

            var sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
            var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();

            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            int totalRecords = 0;

            using (IjalEntities dc = new IjalEntities())
            {
                var product = (from products in dc.ijal_product select products);

                if(!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                {
                    product = product.OrderBy(sortColumn + " " + sortColumnDir);
                }
                totalRecords = product.Count();
                var data_product = product.Skip(skip).Take(pageSize).ToList();
                return Json(new { draw = draw, recordsFiltered = totalRecords, totalRecords = totalRecords, data = data_product }, JsonRequestBehavior.AllowGet);
                
                //   var product = dc.ijal_product.OrderBy(a => a.store_name).ToList();
                //   return Json(new { data = product }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}