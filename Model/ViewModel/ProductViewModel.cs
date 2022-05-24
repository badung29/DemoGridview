using Model.EF;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel
{
    public class ProductViewModel
    {
        public long ID { get; set; }

        [Display(Name = "Category")]
        public long CategoryID { get; set; }

        [Required(ErrorMessage = "Name is requied")]
        [StringLength(250)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Code is requied")]
        [StringLength(10)]
        public string Code { get; set; }

        [Required(ErrorMessage = "MetaTitle is requied")]
        [Display(Name = "MetaTitle")]
        [StringLength(250)]
        public string MetaTitle { get; set; }

        [StringLength(500)]
        public string Description { get; set; }
        public string Image { get; set; }

        [Column(TypeName = "xml")]
        public string MoreImages { get; set; }

        [Required(ErrorMessage = "Price is requied")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:c}")]
        public decimal Price { get; set; }
        public decimal PromotionPrice { get; set; }
        public bool IncludedVAT { get; set; }

        [Required(ErrorMessage = "Quantity is requied")]
        public int Quantity { get; set; }
        public string Detail { get; set; }
        public int Warranty { get; set; }
        public int ViewCount { get; set; }
        public string CateName { get; set; }
        public string CateMetaTitle { get; set; }
        public string CateDescriptions { get; set; }
        public bool Status { get; set; }
        public List<ProductCategory> CateCollection { get; set; }
    }
}
