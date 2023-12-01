using _63CNTT4N1.Library;
using MyClass.DAO;
using MyClass.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace _63CNTT4N1.Areas.Admin.Controllers
{
    public class DashboardController : Controller
    {
        UsersDAO usersDAO = new UsersDAO();
        // GET: Admin/Dashboard
        public ActionResult Index()
        {
            if (Session["UserAdmin"] == null)
            {
                return RedirectToAction("DangNhap");
            }
            else
            {
                return View();
            }
        }

        public ActionResult DangNhap()
        {
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult DangNhap(Users users)
        {
            Users row_user = usersDAO.getRow(users.Username, "admin");
            if (row_user == null)
            {
                TempData["message"] = new XMessage("danger", "Đăng nhập thất bại (Tên đăng nhập không tồn tại)");
                return RedirectToAction("DangNhap");
            }
            else
            {
                if (row_user.Password != users.Password)
                {
                    TempData["message"] = new XMessage("danger", "Đăng nhập thất bại (Mật khẩu không đúng)");
                    return RedirectToAction("DangNhap");
                }
                else
                {
                    Session["UserAdmin"] = row_user.Username;
                    return RedirectToAction("Index");
                }
            }
        }

        public ActionResult DangKy()
        {
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult DangKy(Users users)
        {
            if (ModelState.IsValid)
            {
                var listuser = usersDAO.getList().Select(m => m.Username);
                if (listuser.Contains(users.Username) && users.Role == "admin")
                {
                    TempData["message"] = new XMessage("danger", "Đăng ký thất bại (tên đăng nhập đã tồn tại)");
                    return RedirectToAction("DangKy");
                }
                users.Role = "admin";
                //CreateAt
                users.CreateAt = DateTime.Now;
                //CreateBy
                users.CreateBy = Convert.ToInt32(Session["UserID"]);
                //CreateAts
                users.UpdateAt = DateTime.Now;
                //CreateBy
                users.UpdateBy = Convert.ToInt32(Session["UserID"]);
                users.Status = 1;
                usersDAO.Insert(users);
                TempData["message"] = new XMessage("success", "Đăng ký thành công");
                return RedirectToAction("DangKy");
            }
            return View(users);
        }

        public ActionResult DangXuat()
        {
            Session.Remove("UserAdmin");
            FormsAuthentication.SignOut();
            return RedirectToAction("DangNhap");
        }
    }
}