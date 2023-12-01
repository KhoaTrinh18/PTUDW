using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyClass.Model
{
    [Table("Topics")]
    public class Topics
    {
        [Key]
        public int Id { get; set; }
        
        [Display(Name = "Tên chủ đề")]
        [Required(ErrorMessage = "Tên chủ đề không được để trống")]
        public string Name { get; set; }

        [Display(Name = "Liên kết")]
        public string Slug { get; set; }

        [Display(Name = "Cấp cha")]
        public int? ParentId { get; set; }

        [Display(Name = "Sắp xếp")]
        public int? Order { get; set; }

        [Display(Name = "Mô tả")]
        [Required(ErrorMessage = "Mô tả không được để trống")]
        public string MetaDesc { get; set; }

        [Display(Name = "Từ khóa")]
        [Required(ErrorMessage = "Từ khóa không được để trống")]
        public string MetaKey { get; set; }

        [Display(Name = "Người tạo")]
        [Required(ErrorMessage = "Người tạo không được để trống")]
        public int CreateBy { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [Display(Name = "Ngày tạo")]
        [Required(ErrorMessage = "Ngày tạo không được để trống")]
        public DateTime CreateAt { get; set; }

        [Display(Name = "Người cập nhật")]
        [Required(ErrorMessage = "Người cập nhật không được để trống")]
        public int UpdateBy { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [Display(Name = "Ngày cập nhật")]
        [Required(ErrorMessage = "Ngày cập nhật không được để trống")]
        public DateTime UpdateAt { get; set; }

        [Display(Name = "Trạng thái")]
        [Required(ErrorMessage = "Trạng thái không được để trống")]
        public int Status { get; set; }
    }
}
