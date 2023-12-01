using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyClass.Model
{
    public class ProductInfo
    {
        public int Id { get; set; }

        [Display(Name = "Mã loại sản phẩm")]
        public int CatID { get; set; }

        [Display(Name = "Tên sản phẩm")]
        public string Name { get; set; }

        [Display(Name = "Tên loại sản phẩm")]
        public string CatName { get; set; }

        //Bo sung them truong Slug cua Categories: detail product
        public string CategorySlug { get; set; }

        [Display(Name = "Mã nhà cung cấp")]
        public int SupplierId { get; set; }

        [Display(Name = "Tên nhà cung cấp")]
        public string SupplierName { get; set; }

        [Display(Name = "Liên kết")]
        public string Slug { get; set; }

        [Display(Name = "Chi tiết")]
        public string Detail { get; set; }

        [Display(Name = "Hình ảnh")]
        public string Image { get; set; }

        [Display(Name = "Giá bán")]
        [DisplayFormat(DataFormatString = "{0:C0}")]
        public decimal Price { get; set; }

        [Display(Name = "Giá giảm")]
        [DisplayFormat(DataFormatString = "{0:C0}")]
        public decimal SalePrice { get; set; }

        [Display(Name = "Sô lượng")]
        public decimal Amount { get; set; }

        [Display(Name = "Mô tả")]
        public string MetaDesc { get; set; }

        [Display(Name = "Từ khóa")]
        public string MetaKey { get; set; }

        [Display(Name = "Người tạo")]
        public int CreateBy { get; set; }

        [Display(Name = "Ngày tạo")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime CreateAt { get; set; }

        [Display(Name = "Người cập nhật")]
        public int? UpdateBy { get; set; }

        [Display(Name = "Ngày cập nhật")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? UpdateAt { get; set; }

        [Display(Name = "Trạng thái")]
        public int Status { get; set; }
    }
}
