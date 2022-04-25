using Model.Dao;
using Model.EF;
using Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DemoGridview.Controllers
{
    public class ChartController : Controller
    {
        public ActionResult Index()
        {
            var dao = new CategoryDao();
            var model = dao.ListCategory();
            return View(model);
        }        
    }
}
