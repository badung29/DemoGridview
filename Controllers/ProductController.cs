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
        public ActionResult Index(string searchString, int page = 1, int pageSize = 5)
        {
            var dao = new ProductDao();
            //var model = dao.ListAllPaging();
            int totalProduct = dao.CountProduct(searchString);
            float numberPage = (float)totalProduct / pageSize;
            var model = dao.GetAllProducts(searchString, page, pageSize);
            ViewBag.pageCurrent = page;
            ViewBag.numberPage = (int)Math.Ceiling(numberPage);
            ViewBag.SearchString = searchString;
            return View(model);
        }

        //public ActionResult GetDataProduct(int page, int pageSize)
        //{
        //    var dao = new ProductDao();
        //    int totalProduct = dao.CountProduct();
        //    var dataProduct = dao.ListAllPaging(page, pageSize);
        //    return Json(new { total = totalProduct, data = dataProduct }, JsonRequestBehavior.AllowGet);
        //}

        [HttpGet]
        public ActionResult CreateNewProduct()
        {
            ProductViewModel cate = new ProductViewModel();
            var dao = new CategoryDao();
            cate.CateCollection = dao.ListAll();
            return PartialView("CreateProduct", cate);
        }

        [HttpPost]
        public ActionResult CreateProduct(ProductViewModel data)
        {
            var Catedao = new CategoryDao();
            data.CateCollection = Catedao.ListAll();
            if (ModelState.IsValid)
            {
                var dao = new ProductDao();
                if (dao.CheckProductName(data.Name))
                {
                    ModelState.AddModelError("", "Duplicate product name!");
                }
                else if (dao.CheckProductCode(data.Code))
                {
                    ModelState.AddModelError("", "Duplicate product code!");
                }
                else if (data.Price == 0)
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
                    product.Name = data.Name;
                    product.Code = data.Code;
                    product.MetaTitle = data.MetaTitle;
                    product.Price = data.Price;
                    product.Quantity = data.Quantity;
                    product.CategoryID = data.CategoryID;
                    long id = dao.Insert(product);
                    if (id > 0)
                    {
                        TempData["SuccessMessage"] = "Product " + product.Name + " Created Successfully!";
                        return Json(true, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        TempData["SuccessMessage"] = "Product " + product.Name + " Created Failed!";
                        return Json(true, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            return PartialView("CreateProduct", data);
        }

        [HttpGet]
        public ActionResult DetailProduct(int id)
        {
            //var product = new ProductDao().ViewDetail(id);
            var product = new ProductDao().GetProductByID(id).FirstOrDefault();
            ProductViewModel cate = new ProductViewModel();
            cate.ID = product.ID;
            cate.CategoryID = product.CategoryID;
            cate.Name = product.Name;
            cate.Code = product.Code;
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
            //var product = new ProductDao().ViewDetail(id);
            var product = new ProductDao().GetProductByID(id).FirstOrDefault();
            ProductViewModel cate = new ProductViewModel();
            cate.ID = product.ID;
            cate.CategoryID = product.CategoryID;
            cate.Name = product.Name;
            cate.Code = product.Code;
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
                var dao = new ProductDao();
                if (dao.CheckProductName(data.Name))
                {
                    ModelState.AddModelError("", "Duplicate product name!");
                }
                else if (dao.CheckProductCode(data.Code))
                {
                    ModelState.AddModelError("", "Duplicate product code!");
                }
                else if (data.Price == 0)
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
                    product.MetaTitle = data.MetaTitle;
                    product.Price = data.Price;
                    product.Quantity = data.Quantity;
                    product.CategoryID = data.CategoryID;
                    var daoProduct = new ProductDao();
                    var result = daoProduct.Update(product);
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