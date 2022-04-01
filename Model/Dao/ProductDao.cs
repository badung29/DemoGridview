using Model.EF;
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

        public long Insert(Product entity)
        {
            db.Products.Add(entity);
            db.SaveChanges();
            return entity.ID;
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
                db.Products.Remove(product);
                db.SaveChanges();
                return true;
            }
            catch (Exception) { return false; }
        }

        public IEnumerable<ProductViewModel> ListAllPaging(string searchString, int page, int pageSize)
        {
            IQueryable<ProductViewModel> model = from product in db.Products
                                                 join productCategory in db.ProductCategories
                                                 on product.CategoryID equals productCategory.ID
                                                 select new ProductViewModel()
                                                 {
                                                     CateMetaTitle = productCategory.MetaTitle,
                                                     CateName = productCategory.Name,
                                                     Status = product.Status,
                                                     ID = product.ID,
                                                     Name = product.Name,
                                                     Code = product.Code,
                                                     MetaTitle = product.MetaTitle,
                                                     Price = (decimal)product.Price,
                                                     Quantity = product.Quantity
                                                 };
            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.Name.Contains(searchString) || x.CateName.Contains(searchString));
            }
            int start = (page - 1) * pageSize;
            var dataProduct = model.OrderByDescending(x => x.ID).Skip(start).Take(pageSize);
            return dataProduct.ToList();
        }

        public int CountProduct(string searchString)
        {
            IQueryable<ProductViewModel> model = from product in db.Products
                                                 join productCategory in db.ProductCategories
                                                 on product.CategoryID equals productCategory.ID
                                                 select new ProductViewModel()
                                                 {
                                                     CateMetaTitle = productCategory.MetaTitle,
                                                     CateName = productCategory.Name,
                                                     Status = product.Status,
                                                     ID = product.ID,
                                                     Name = product.Name,
                                                     Code = product.Code,
                                                     Price = (decimal)product.Price,
                                                     Quantity = product.Quantity
                                                 };
            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.Name.Contains(searchString) || x.CateName.Contains(searchString));
            }
            return model.Count();
        }

        public List<string> ListName(string keyword)
        {
            return db.Products.Where(x => x.Name.Contains(keyword)).Select(x => x.Name).ToList();
        }

        public List<Product> ListNewProduct(int top)
        {
            return db.Products.OrderByDescending(x => x.CreatedDate).Take(top).ToList();
        }

        public List<Product> ListByCategoryId(long categoryID, ref int totalRecord, int page = 1, int pageSize = 2)
        {
            totalRecord = db.Products.Where(x => x.CategoryID == categoryID).Count();
            var model = db.Products.Where(x => x.CategoryID == categoryID);
            //var model = from a in db.Products
            //            join b in db.ProductCategories
            //            on a.CategoryID equals b.ID
            //            where a.CategoryID == categoryID
            //            select new ProductViewModel()
            //            {
            //                CateMetaTitle = b.MetaTitle,
            //                CateName = b.Name,
            //                CreatedDate = a.CreatedDate,
            //                ID = a.ID,
            //                Images = a.Image,
            //                Name = a.Name,
            //                MetaTitle = a.MetaTitle,
            //                Price = a.Price
            //            };
            model.OrderByDescending(x => x.CreatedDate).Skip((page - 1) * pageSize).Take(pageSize);
            return model.ToList();
        }

        public List<Product> Search(string keyword, ref int totalRecord, int page = 1, int pageSize = 2)
        {
            totalRecord = db.Products.Where(x => x.Name.Contains(keyword)).Count();
            var model = db.Products.Where(x => x.Name.Contains(keyword));

            model.OrderByDescending(x => x.CreatedDate).Skip((page - 1) * pageSize).Take(pageSize);
            return model.ToList();
        }

        public List<Product> ListFeatureProduct(int top)
        {
            return db.Products.Where(x => x.TopHot != null && x.TopHot > DateTime.Now).OrderByDescending(x => x.CreatedDate).Take(top).ToList();
        }

        public List<Product> ListRelatedProducts(long productId)
        {
            var product = db.Products.Find(productId);
            return db.Products.Where(x => x.ID != productId && x.CategoryID == product.CategoryID).ToList();
        }

        public Product ViewDetail(long id)
        {
            return db.Products.Find(id);
        }

        public bool ChangeStatus(long id)
        {
            var product = db.Products.Find(id);
            product.Status = !product.Status;
            db.SaveChanges();
            return product.Status;
        }

        public bool CheckProductName(string Name)
        {
            return db.Products.Count(x => x.Name == Name) > 0;
        }

        public bool CheckProductCode(string Code)
        {
            return db.Products.Count(x => x.Code == Code) > 0;
        }
    }
}
