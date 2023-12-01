using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyClass.Model
{
    [Table("Posts")]
    public class Posts
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Chủ đề bài viết không được để trống")]
        [Display(Name = "Chủ đề bài viết")]
        public int TopID { get; set; }

        [Required(ErrorMessage = "Tên bài viết không được để trống")]
        [Display(Name = "Tên bài viết")]
        public string Title { get; set; }

        [Display(Name = "Liên kết")]
        public string Slug { get; set; }

        [Display(Name = "Chi tiết")]
        public string Detail { get; set; }

        [Display(Name = "Ảnh bài viết")]
        public string Image { get; set; }

        [Display(Name = "Kiểu bài viết")]
        public string PostType { get; set; }

        [Required(ErrorMessage = "Mô tả không được để trống")]
        [Display(Name = "Mô tả")]
        public string MetaDesc { get; set; }

        [Required(ErrorMessage = "Từ khóa không được để trống")]
        [Display(Name = "Từ khóa")]
        public string MetaKey { get; set; }

        [Required(ErrorMessage = "Người tạo không được để trống")]
        [Display(Name = "Người tạo")]
        public int CreateBy { get; set; }

        [Required(ErrorMessage = "Ngày tạo không được để trống")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [Display(Name = "Ngày tạo")]
        public DateTime CreateAt { get; set; }

        [Required(ErrorMessage = "Người cập nhật không được để trống")]
        [Display(Name = "Người cập nhật")]
        public int UpdateBy { get; set; }

        [Required(ErrorMessage = "Ngày cập nhật không được để trống")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [Display(Name = "Ngày cập nhật")]
        public DateTime UpdateAt { get; set; }

        [Required(ErrorMessage = "Trạng thái không được để trống")]
        [Display(Name = "Trạng thái")]
        public int Status { get; set; }
    }
}
