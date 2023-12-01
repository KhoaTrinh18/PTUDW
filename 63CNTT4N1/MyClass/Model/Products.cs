using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyClass.Model
{
    [Table("Products")]
    public class Products
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Mã loại sản phẩm")]
        [Required(ErrorMessage = "Mã loại sản phẩm không được để trống")]
        public int CatID { get; set; }

        [Display(Name = "Tên sản phẩm")]
        [Required(ErrorMessage = "Tên sản phẩm không được để trống")]
        public string Name { get; set; }

        [Display(Name = "Mã nhà cung cấp")]
        [Required(ErrorMessage = "Mã nhà cung cấp không được để trống")]
        public int SupplierID { get; set; }

        [Display(Name = "Liên kết")]
        public string Slug { get; set; }

        [Display(Name = "Chi tiết")]
        [Required(ErrorMessage = "Chi tiết không được để trống")]
        public string Detail { get; set; }

        [Display(Name = "Hình ảnh")]
        public string Image { get; set; }

        [Display(Name = "Giá bán")]
        [DisplayFormat(DataFormatString = "{0:C0}")]
        [Required(ErrorMessage = "Giá sản phẩm không được để trống")]
        public decimal Price { get; set; }

        [Display(Name = "Giá giảm")]
        [DisplayFormat(DataFormatString = "{0:C0}")]
        [Required(ErrorMessage = "Giá bán không được để trống")]
        public decimal SalePrice { get; set; }

        [Display(Name = "Số lượng")]
        [Required(ErrorMessage = "Số lượng không được để trống")]
        public decimal Amount { get; set; }

        [Display(Name = "Mô tả")]
        [Required(ErrorMessage = "Mô tả không được để trống")]
        public string MetaDesc { get; set; }

        [Display(Name = "Từ khóa")]
        [Required(ErrorMessage = "Từ khóa không được để trống")]
        public string MetaKey { get; set; }

        [Display(Name = "Người tạo")]
        [Required(ErrorMessage = "Người tạo không được để trống")]
        public int CreateBy { get; set; }

        [Display(Name = "Ngày tạo")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [Required(ErrorMessage = "Ngày tạo không được để trống")]
        public DateTime CreateAt { get; set; }

        [Display(Name = "Người cập nhật")]
        [Required(ErrorMessage = "Người cập nhật không được để trống")]
        public int UpdateBy { get; set; }

        [Display(Name = "Ngày cập nhật")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [Required(ErrorMessage = "Ngày cập nhật không được để trống")]
        public DateTime UpdateAt { get; set; }

        [Display(Name = "Trạng thái")]
        [Required(ErrorMessage = "Trạng thái không được để trống")]
        public int Status { get; set; }
    }
}
