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
        public IEnumerable<ProductViewModel> ListProduct()
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

        public IEnumerable<PieChartCategory> ListCategory()
        {
            var dataProduct = ListProduct();
            var model = new List<PieChartCategory>();
            Dictionary<string, int> checkDup = dataProduct.GroupBy(x => x.CateName)
                                        .Where(g => g.Count() > 1)
                                        .ToDictionary(x => x.Key, x => x.Count());

            foreach (var datas in checkDup)
            {
                model.Add(new PieChartCategory() { Name = datas.Key, Value = datas.Value });
            }
            return model.ToList();
        }
    }
}
