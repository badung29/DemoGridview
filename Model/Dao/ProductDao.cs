﻿using Model.EF;
using Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Dao
{
    public class ProductDao
    {
        OnlineShopDbContext db = null;

        public ProductDao()
        {
            db = new OnlineShopDbContext();
        }

        public bool Update(Product entity)
        {
            try
            {
                var product = db.Products.Find(entity.ID);
                product.Name = entity.Name;
                product.Code = entity.Code;
                product.MetaTitle = entity.MetaTitle;
                product.Description = entity.Description;
                product.Image = entity.Image;
                product.MoreImages = entity.MoreImages;
                product.Price = entity.Price;
                product.PromotionPrice = entity.PromotionPrice;
                product.IncludedVAT = entity.IncludedVAT;
                product.Quantity = entity.Quantity;
                product.CategoryID = entity.CategoryID;
                product.Detail = entity.Detail;
                product.Status = entity.Status;
                product.TopHot = entity.TopHot;
                product.ViewCount = entity.ViewCount;
                product.ModifiedDate = DateTime.Now;
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool Delete(int id)
        {
            try
            {
                var product = db.Products.Find(id);
                //db.Products.Remove(product);
                db.SaveChanges();
                return true;
            }
            catch (Exception) { return false; }
        }

        public IEnumerable<ProductViewModel> ListAllPaging()
        {
            IQueryable<ProductViewModel> model = from product in db.Products
                                                 join productCategory in db.ProductCategories
                                                 on product.CategoryID equals productCategory.ID into temp
                                                 from tem in temp.DefaultIfEmpty()
                                                 select new ProductViewModel()
                                                 {
                                                     CateName = tem.Name != null ? tem.Name : "No Category",
                                                     ID = product.ID,
                                                     Name = product.Name,
                                                     Code = product.Code,
                                                     MetaTitle = product.MetaTitle,
                                                     Price = (decimal)product.Price,
                                                     Quantity = product.Quantity
                                                 };                       
            return model.ToList();
        }

        public Product ViewDetail(long id)
        {
            return db.Products.Find(id);
        }

    }
}
