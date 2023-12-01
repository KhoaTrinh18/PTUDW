using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using _63CNTT4N1.Library;
using MyClass.Model;
using MyClass.DAO;
using System.Web.Services.Description;

namespace _63CNTT4N1.Areas.Admin.Controllers
{
    public class SliderController : Controller
    {
        SlidersDAO slidersDAO = new SlidersDAO();

        ////////////////////////////////////////////////////////////////////
        // GET: Admin/Supplier = INDEX
        public ActionResult Index()
        {
            return View(slidersDAO.getList("Index"));
        }

        ////////////////////////////////////////////////////////////////////
        // GET: Admin/Slider/Create
        public ActionResult Create()
        {
            ViewBag.OrderList = new SelectList(slidersDAO.getList("Index"), "Order", "Name");
            return View();
        }

        // POST: Admin/Slider/Create
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Sliders sliders)
        {
            if (ModelState.IsValid)
            {
                //Xu ly cho muc Order
                if (sliders.Order == null)
                {
                    sliders.Order = 1;
                }
                else
                {
                    sliders.Order = sliders.Order + 1;
                }
                //Xu ly cho muc Slug
                string slug = XString.Str_Slug(sliders.Name);
                //xu ly cho phan upload hình ảnh
                var img = Request.Files["img"];//Lay thong tin file
                if (img.ContentLength != 0)
                {
                    string[] FileExtentions = new string[] { ".jpg", ".jpeg", ".png", ".gif" };
                    //Kiem tra tap tin co hay khong
                    if (FileExtentions.Contains(img.FileName.Substring(img.FileName.LastIndexOf("."))))//Lay phan mo rong cua tap tin
                    {
                        //Ten file = Slug + phan mo rong cua tap tin
                        string imgName = slug + img.FileName.Substring(img.FileName.LastIndexOf("."));
                        sliders.Image = imgName;
                        //Upload hinh
                        string PathDir = "~/Public/img/slider/";
                        string PathFile = Path.Combine(Server.MapPath(PathDir), imgName);
                        img.SaveAs(PathFile);
                    }
                }//Ket thuc phan upload hinh anh
                //Xu ly cho muc CreateAt
                sliders.CreateAt = DateTime.Now;
                //Xu ly cho muc CreateBy
                sliders.CreateBy = Convert.ToInt32(Session["UserId"]);
                //Xu ly cho muc UpdateAt
                sliders.UpdateAt = DateTime.Now;
                //Xu ly cho muc UpdateBy
                sliders.UpdateBy = Convert.ToInt32(Session["UserId"]);
                slidersDAO.Insert(sliders);
                //Hien thi thong bao
                TempData["message"] = new XMessage("success", "Thêm slider thành công");
                return RedirectToAction("Index");
            }
            ViewBag.OrderList = new SelectList(slidersDAO.getList("Index"), "Order", "Name");
            return View(sliders);
        }

        ////////////////////////////////////////////////////////////////////
        // GET: Admin/Supplier/Staus/5:Thay doi trang thai cua mau tin
        public ActionResult Status(int? id)
        {
            if (id == null)
            {
                //Hien thi thong bao
                TempData["message"] = new XMessage("danger", "Cập nhật trạng thái thất bại");
                //Chuyen huong trang
                return RedirectToAction("Index", "Slider");
            }
            //Khi nhap nut thay doi Status cho mot mau tin
            Sliders sliders = slidersDAO.getRow(id);
            //Kiem tra id cua sliders co ton tai?
            if (sliders == null)
            {
                //Hien thi thong bao
                TempData["message"] = new XMessage("danger", "Cập nhật trạng thái thất bại");
                //chuyen huong trang
                return RedirectToAction("Index", "Slider");
            }
            //Thay doi trang thai Status tu 1 thanh 2 va nguoc lai
            sliders.Status = (sliders.Status == 1) ? 2 : 1;
            //Cap nhat gia tri cho UpdateAt/By
            sliders.UpdateBy = Convert.ToInt32(Session["UserId"].ToString());
            sliders.UpdateAt = DateTime.Now;
            //Goi ham Update trong CategoryDAO
            slidersDAO.Update(sliders);
            //Hien thi thong bao
            TempData["message"] = new XMessage("success", "Cập nhật trạng thái thành công");
            //Khi cap nhat xong thi chuyen ve Index
            return RedirectToAction("Index", "Slider");
        }

        ////////////////////////////////////////////////////////////////////
        // GET: Admin/Slider/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                //Hien thi thong bao
                TempData["message"] = new XMessage("danger", "Không tìm thấy slider");
                //Chuyen huong trang
                return RedirectToAction("Index", "Slider");
            }
            Sliders sliders = slidersDAO.getRow(id);
            if (sliders == null)
            {
                //Hien thi thong bao
                TempData["message"] = new XMessage("danger", "Không tìm thấy slider");
                //Chuyen huong trang
                return RedirectToAction("Index", "Slider");
            }
            return View(sliders);
        }

        ////////////////////////////////////////////////////////////////////
        // GET: Admin/Slider/Edit/5
        public ActionResult Edit(int? id)
        {
            ViewBag.OrderList = new SelectList(slidersDAO.getList("Index"), "Order", "Name");
            if (id == null)
            {
                //Hien thi thong bao
                TempData["message"] = new XMessage("danger", "Cập nhật slider thất bại");
                //Chuyen huong trang
                return RedirectToAction("Index", "Slider");
            }
            Sliders sliders = slidersDAO.getRow(id);
            if (sliders == null)
            {
                //Hien thi thong bao
                TempData["message"] = new XMessage("danger", "Cập nhật slider thất bại");
                //Chuyen huong trang
                return RedirectToAction("Index", "Slider");
            }
            return View(sliders);
        }

        // POST: Admin/Slider/Edit/5
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Sliders sliders)
        {
            if (ModelState.IsValid)
            {
                //Xu ly cho muc Slug
                string slug = XString.Str_Slug(sliders.Name);
                //Chuyen doi dua vao truong Name de loai bo dau, khoang cach = dau -
                //Xu ly cho muc Order
                if (sliders.Order == null)
                {
                    sliders.Order = 1;
                }
                else
                {
                    sliders.Order = sliders.Order + 1;
                }
                //xu ly cho phan upload hình ảnh
                var img = Request.Files["img"];//lay thong tin file
                if (img.ContentLength != 0)
                {
                    string[] FileExtentions = new string[] { ".jpg", ".jpeg", ".png", ".gif" };
                    //kiem tra tap tin co hay khong
                    if (FileExtentions.Contains(img.FileName.Substring(img.FileName.LastIndexOf("."))))//lay phan mo rong cua tap tin
                    {
                        //ten file = Slug + phan mo rong cua tap tin
                        string imgName = slug + sliders.Id.ToString()+ img.FileName.Substring(img.FileName.LastIndexOf("."));
                        sliders.Image = imgName;
                        //upload hinh
                        string PathDir = "~/Public/img/slider/";
                        string PathFile = Path.Combine(Server.MapPath(PathDir), imgName);
                        img.SaveAs(PathFile);
                    }
                }//ket thuc phan upload hinh anh
                //Xu ly cho muc UpdateAt
                sliders.UpdateAt = DateTime.Now;
                //Xu ly cho muc UpdateBy
                sliders.UpdateBy = Convert.ToInt32(Session["UserId"]);
                slidersDAO.Update(sliders);
                //Hien thi thong bao
                TempData["message"] = new XMessage("success", "Cập nhật slider thành công");
                return RedirectToAction("Index");
            }
            return View(sliders);
        }

        ////////////////////////////////////////////////////////////////
        // GET: Admin/Slider/DelTrash/5:Thay doi trang thai cua mau tin = 0
        public ActionResult DelTrash(int? id)
        {
            //Khi nhap nut thay doi Status cho mot mau tin
            Sliders sliders = slidersDAO.getRow(id);
            //Thay doi trang thai Status tu 1,2 thanh 0
            sliders.Status = 0;
            //Cap nhat gia tri cho UpdateAt/By
            sliders.UpdateBy = Convert.ToInt32(Session["UserId"].ToString());
            sliders.UpdateAt = DateTime.Now;
            //Goi ham Update trong SupplierDAO
            slidersDAO.Update(sliders);
            //Hien thi thong bao
            TempData["message"] = new XMessage("success", "Xóa slider thành công");
            //Khi cap nhat xong thi chuyen ve Index
            return RedirectToAction("Index", "Slider");
        }

        ////////////////////////////////////////////////////////////////
        // GET: Admin/Slider/Trash/5:Hien thi cac mau tin có gia tri la 0
        public ActionResult Trash()
        {
            return View(slidersDAO.getList("Trash"));
        }

        ////////////////////////////////////////////////////////////////
        // GET: Admin/Slider/Recover/5:Thay doi trang thai cua mau tin
        public ActionResult Undo(int? id)
        {
            if (id == null)
            {
                //Hien thi thong bao
                TempData["message"] = new XMessage("danger", "Phục hồi slider thất bại");
                //Chuyen huong trang
                return RedirectToAction("Trash", "Slider");
            }
            //Khi nhap nut thay doi Status cho mot mau tin
            Sliders sliders = slidersDAO.getRow(id);
            //kiem tra id cua topics co ton tai?
            if (sliders == null)
            {
                //Hien thi thong bao
                TempData["message"] = new XMessage("danger", "Phục hồi slider thất bại");
                //Chuyen huong trang
                return RedirectToAction("Trash", "Slider");
            }
            //Thay doi trang thai Status = 2
            sliders.Status = 2;
            //Cap nhat gia tri cho UpdateAt/By
            sliders.UpdateBy = Convert.ToInt32(Session["UserId"].ToString());
            sliders.UpdateAt = DateTime.Now;
            //Goi ham Update trong PostsDAO
            slidersDAO.Update(sliders);
            //Hien thi thong bao
            TempData["message"] = new XMessage("success", "Phục hồi slider thành công");
            //Khi cap nhat xong thi chuyen ve Trash de phuc hoi tiep
            return RedirectToAction("Trash", "Slider");
        }

        ////////////////////////////////////////////////////////////////
        // GET: Admin/Slider/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                //Hien thi thong bao
                TempData["message"] = new XMessage("danger", "Xóa slider thất bại");
                //Chuyen huong trang
                return RedirectToAction("Trash", "Slider");
            }
            //Truy van mau tin theo Id
            Sliders sliders = slidersDAO.getRow(id);

            if (sliders == null)
            {
                //Hien thi thong bao
                TempData["message"] = new XMessage("danger", "Xóa slider thất bại");
                //Chuyen huong trang
                return RedirectToAction("Trash", "Slider");
            }
            return View(sliders);
        }

        // POST: Admin/Slider/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            //Truy van mau tin theo Id
            Sliders sliders = slidersDAO.getRow(id);

            if (slidersDAO.Delete(sliders) == 1)
            {
                //Duong dan den anh can xoa
                string PathDir = "~/Public/img/slider/";
                string slug = XString.Str_Slug(sliders.Name);
                if (sliders.Image != null)
                {
                    //Tim tat ca hinh anh co ten slug giong nhau 
                    string[] DelPath = Directory.GetFiles(Server.MapPath(PathDir), slug + ".*");
                    //Xoa toan bo hinh anh cua mau tin
                    foreach (string dp in DelPath)
                    {
                        System.IO.File.Delete(dp);
                    }
                }
            }
            //Hien thi thong bao
            TempData["message"] = new XMessage("success", "Xóa slider thành công");
            //O lai trang thung rac
            return RedirectToAction("Trash");
        }
    }
}
