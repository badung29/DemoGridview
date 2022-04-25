using Model.EF;
using Model.ViewModel;
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

        public IEnumerable<PieChartCategory> ListCategory() {
            IQueryable<PieChartCategory> model = from category in db.ProductCategories
                                                 where category.ParentID == null
                                                 select new PieChartCategory()
                                                 {
                                                     Name = category.Name,
                                                     Value = (int)category.DisplayOrder
                                                 };
            return model.ToList();

        }
    }
}
