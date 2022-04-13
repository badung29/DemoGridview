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
    public class ProductController : Controller
    {
        // GET: Product
        public ActionResult Index( int page = 1, int pageSize = 5)
        {
            var dao = new ProductDao();
            var model = dao.ListAllPaging();
            return View(model);
        }
        public ActionResult GetListData()
        {
            var dao = new ProductDao();
            var data = dao.ListAllPaging();
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult DetailProduct(int id)
        {
            var product = new ProductDao().ViewDetail(id);
            ProductViewModel cate = new ProductViewModel();
            cate.ID = product.ID;
            cate.CategoryID = product.CategoryID;
            cate.Name = product.Name;
            cate.Code = product.Code;
            cate.Description = product.Description;
            cate.MetaTitle = product.MetaTitle;
            cate.Price = (decimal)product.Price;
            cate.Quantity = product.Quantity;
            var dao = new CategoryDao();
            cate.CateCollection = dao.ListAll();
            return PartialView("DetailProduct", cate);
        }

        [HttpGet]
        public ActionResult EditProduct(int id)
        {
            var product = new ProductDao().ViewDetail(id);
            ProductViewModel cate = new ProductViewModel();
            cate.ID = product.ID;
            cate.CategoryID = product.CategoryID;
            cate.Name = product.Name;
            cate.Code = product.Code;
            cate.Description = product.Description;
            cate.MetaTitle = product.MetaTitle;
            cate.Price = (decimal)product.Price;
            cate.Quantity = product.Quantity;
            var dao = new CategoryDao();
            cate.CateCollection = dao.ListAll();
            return PartialView("EditProduct", cate);
        }

        [HttpPost]
        public ActionResult UpdateProduct(ProductViewModel data)
        {
            var Catedao = new CategoryDao();
            data.CateCollection = Catedao.ListAll();
            if (ModelState.IsValid)
            {
                if (data.Price == 0)
                {
                    ModelState.AddModelError("", "Please enter the price!");
                }
                else if (data.Quantity == 0)
                {
                    ModelState.AddModelError("", "Please enter the quantity!");
                }
                else
                {
                    var product = new Product();
                    product.ID = data.ID;
                    product.Name = data.Name;
                    product.Code = data.Code;
                    product.Description = data.Description;
                    product.MetaTitle = data.MetaTitle;
                    product.Price = data.Price;
                    product.Quantity = data.Quantity;
                    product.CategoryID = data.CategoryID;
                    var dao = new ProductDao();
                    var result = dao.Update(product);
                    if (result)
                    {
                        TempData["SuccessMessage"] = "Product " + product.Name + " Saved Successfully";
                        return Json(true, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        ModelState.AddModelError("", "Update product fail");
                    }
                }
            }
            return PartialView("EditProduct", data);
        }

        [HttpPost]
        public JsonResult DeleteProduct(int id)
        {
            if (id != 0)
            {
                new ProductDao().Delete(id);
            }
            else { return Json(new { status = false }); }

            return Json(new { status = true });
        }
    }
}