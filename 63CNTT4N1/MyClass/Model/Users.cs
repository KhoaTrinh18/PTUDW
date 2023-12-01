using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MyClass.Model
{
    [Table("Users")]
    public class Users
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Tên người dùng")]
        [Required(ErrorMessage = "Tên người dùng không được để trống")]
        public string Username { get; set; }

        [RegularExpression("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$", ErrorMessage = "Mật khẩu phải bao gồm chữ, số, ít nhất 1 ký tự viết hoa và 1 ký tự đặc biệt")]
        [Display(Name = "Mật khẩu")]
        [Required(ErrorMessage = "Mật khẩu không được để trống")] 
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "Xác nhận mật khẩu không đúng")]
        [Display(Name = "Xác nhận mật khẩu")]
        [Required(ErrorMessage = "Xác nhận mật khẩu không được để trống")] 
        public string ConfPassword { get; set; }

        [RegularExpression("[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?", ErrorMessage = "Email không hợp lệ")]
        [Display(Name = "Email")]
        [Required(ErrorMessage = "Email không được để trống")]
        public string Email { get; set; }

        [Display(Name = "Vai trò")]
        public string Role { get; set; }

        [Display(Name = "Ngày tạo")]
        public DateTime? CreateAt { get; set; }

        [Display(Name = "Người tạo")]
        public int? CreateBy { get; set; }

        [Display(Name = "Ngày cập nhật")]
        public DateTime? UpdateAt { get; set; }

        [Display(Name = "Người cập nhật")]
        public int? UpdateBy { get; set; }

        [Display(Name = "Trạng thái")]
        public int? Status { get; set; }
    }
}
