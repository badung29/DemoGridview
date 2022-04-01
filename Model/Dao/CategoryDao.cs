using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Dao
{
    public class CategoryDao
    {
        OnlineShopDbContext db = null;
        public CategoryDao()
        {
            db = new OnlineShopDbContext();
        }


        public List<ProductCategory> ListAll()
        {
            return db.ProductCategories.Where(x => x.Status == true).ToList();
        }

        public long Insert(ProductCategory entity)
        {
            db.ProductCategories.Add(entity);
            db.SaveChanges();
            return entity.ID;
        }

        public ProductCategory ViewDetail(long id)
        {
            return db.ProductCategories.Find(id);
        }

        public bool CheckProductCategoryName(string Name)
        {
            return db.ProductCategories.Count(x => x.Name == Name) > 0;
        }
    }
}
