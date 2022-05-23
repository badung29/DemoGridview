using Model.EF;
using Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace Model.Dao
{
    public class ProductDao
    {
        OnlineShopDbContext db = null;
        string conString = ConfigurationManager.ConnectionStrings["OnlineShopDbContext"].ToString();
        public ProductDao()
        {
            db = new OnlineShopDbContext();
        }

        public bool Update(Product entity)
        {
            //try
            //{
            //    var product = db.Products.Find(entity.ID);
            //    product.Name = entity.Name;
            //    product.Code = entity.Code;
            //    product.MetaTitle = entity.MetaTitle;
            //    product.Description = entity.Description;
            //    product.Image = entity.Image;
            //    product.MoreImages = entity.MoreImages;
            //    product.Price = entity.Price;
            //    product.PromotionPrice = entity.PromotionPrice;
            //    product.IncludedVAT = entity.IncludedVAT;
            //    product.Quantity = entity.Quantity;
            //    product.CategoryID = entity.CategoryID;
            //    product.Detail = entity.Detail;
            //    product.Status = entity.Status;
            //    product.TopHot = entity.TopHot;
            //    product.ViewCount = entity.ViewCount;
            //    product.ModifiedDate = DateTime.Now;
            //    db.SaveChanges();
            //    return true;
            //}
            //catch (Exception)
            //{
            //    return false;
            //}

            int i = 0;
            using (SqlConnection connection = new SqlConnection(conString))
            {
                SqlCommand command = new SqlCommand("Sp_UpdateProducts", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ID", entity.ID);
                command.Parameters.AddWithValue("@Name", entity.Name);
                command.Parameters.AddWithValue("@Code", entity.Code);
                command.Parameters.AddWithValue("@MetaTitle", entity.MetaTitle);
                command.Parameters.AddWithValue("@CategoryID", entity.CategoryID);
                command.Parameters.AddWithValue("@Price", entity.Price);
                command.Parameters.AddWithValue("@Quantity", entity.Quantity);

                connection.Open();
                i = command.ExecuteNonQuery();
                connection.Close();
            }
            if (i > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool Delete(int id)
        {
            try
            {
                //var product = db.Products.Find(id);
                ////db.Products.Remove(product);
                //db.SaveChanges();
                string result = "";
                using (SqlConnection connection = new SqlConnection(conString)) {
                    SqlCommand command = new SqlCommand("Sp_DeleteProduct", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ID",id);
                    command.Parameters.Add("@OUTPUTMESSAGE", SqlDbType.VarChar,50).Direction = ParameterDirection.Output;

                    //connection.Open();
                    command.ExecuteNonQuery();
                    result = command.Parameters["@OUTPUTMESSAGE"].Value.ToString();
                    connection.Close();
                }
                    return true;
            }
            catch (Exception) { return false; }
        }

        public IEnumerable<ProductViewModel> GetAllProducts(string searchString, int page, int pageSize)
        {
            if (searchString == null) { searchString = ""; }
            List<ProductViewModel> model = new List<ProductViewModel>();
            using (SqlConnection connection = new SqlConnection(conString)) {
                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "Sp_GetAllProducts";
                command.Parameters.AddWithValue("@Search", searchString);
                SqlDataAdapter sqlDA = new SqlDataAdapter(command);
                DataTable dtProducts = new DataTable();

                connection.Open();
                sqlDA.Fill(dtProducts);
                connection.Close();

                foreach (DataRow dr in dtProducts.Rows) {
                    model.Add(new ProductViewModel
                    {   
                        ID = Convert.ToInt32(dr["ID"]),
                        Code = dr["Code"].ToString(),
                        Name = dr["Name"].ToString(),
                        CateName = dr["CateName"].ToString(),
                        MetaTitle = dr["MetaTitle"].ToString(),
                        Price = Convert.ToDecimal(dr["Price"]),
                        Quantity = Convert.ToInt32(dr["Quantity"])
                    });
                }
            }

            int start = (page - 1) * pageSize;
            var dataProduct = model.OrderByDescending(x => x.ID).Skip(start).Take(pageSize);
            return dataProduct.ToList();
        }
        public int CountProduct(string searchString)
        {
            if (searchString == null) { searchString = ""; }
            List<ProductViewModel> model = new List<ProductViewModel>();
            using (SqlConnection connection = new SqlConnection(conString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "Sp_GetAllProducts";
                command.Parameters.AddWithValue("@Search", searchString);
                SqlDataAdapter sqlDA = new SqlDataAdapter(command);
                DataTable dtProducts = new DataTable();

                connection.Open();
                sqlDA.Fill(dtProducts);
                connection.Close();

                foreach (DataRow dr in dtProducts.Rows)
                {
                    model.Add(new ProductViewModel
                    {
                        ID = Convert.ToInt32(dr["ID"]),
                        Code = dr["Code"].ToString(),
                        Name = dr["Name"].ToString(),
                        CateName = dr["CateName"].ToString(),
                        MetaTitle = dr["MetaTitle"].ToString(),
                        Price = Convert.ToDecimal(dr["Price"]),
                        Quantity = Convert.ToInt32(dr["Quantity"])
                    });
                }
            }
            return model.Count();
        }

        public IEnumerable<ProductViewModel> GetProductByID(int id)
        {
            List<ProductViewModel> model = new List<ProductViewModel>();
            using (SqlConnection connection = new SqlConnection(conString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "Sp_GetProductByID";
                command.Parameters.AddWithValue("@ID",id);
                SqlDataAdapter sqlDA = new SqlDataAdapter(command);
                DataTable dtProducts = new DataTable();

                connection.Open();
                sqlDA.Fill(dtProducts);
                connection.Close();

                foreach (DataRow dr in dtProducts.Rows)
                {
                    model.Add(new ProductViewModel
                    {
                        ID = Convert.ToInt32(dr["ID"]),
                        Code = dr["Code"].ToString(),
                        Name = dr["Name"].ToString(),                   
                        MetaTitle = dr["MetaTitle"].ToString(),
                        CategoryID = Convert.ToInt64(dr["CategoryID"]),
                        Price = Convert.ToDecimal(dr["Price"]),
                        Quantity = Convert.ToInt32(dr["Quantity"])
                    });
                }
            }
            return model;
        }

        //public IEnumerable<ProductViewModel> ListAllPaging()
        //{
        //    IQueryable<ProductViewModel> model = from product in db.Products
        //                                         join productCategory in db.ProductCategories
        //                                         on product.CategoryID equals productCategory.ID into temp
        //                                         from tem in temp.DefaultIfEmpty()
        //                                         select new ProductViewModel()
        //                                         {
        //                                             CateName = tem.Name != null ? tem.Name : "No Category",
        //                                             ID = product.ID,
        //                                             Name = product.Name,
        //                                             Code = product.Code,
        //                                             MetaTitle = product.MetaTitle,
        //                                             Price = (decimal)product.Price,
        //                                             Quantity = product.Quantity
        //                                         };
        //    return model.ToList();
        //}

        //public int CountProduct()
        //{
        //    IQueryable<ProductViewModel> model = from product in db.Products
        //                                         join productCategory in db.ProductCategories
        //                                         on product.CategoryID equals productCategory.ID into temp
        //                                         from tem in temp.DefaultIfEmpty()
        //                                         select new ProductViewModel()
        //                                         {
        //                                             CateName = tem.Name != null ? tem.Name : "No Category",
        //                                             ID = product.ID,
        //                                             Name = product.Name,
        //                                             Code = product.Code,
        //                                             MetaTitle = product.MetaTitle,
        //                                             Price = (decimal)product.Price,
        //                                             Quantity = product.Quantity
        //                                         };

        //    return model.Count();
        //}
        //public Product ViewDetail(long id)
        //{
        //    return db.Products.Find(id);
        //}

    }
}
