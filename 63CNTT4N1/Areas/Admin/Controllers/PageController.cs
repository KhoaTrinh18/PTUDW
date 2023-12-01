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
using MyClass.DAO;
using MyClass.Model;

namespace _63CNTT4N1.Areas.Admin.Controllers
{
    public class PageController : Controller
    {
        PostsDAO postsDAO = new PostsDAO();
        LinksDAO linksDAO = new LinksDAO();

        ////////////////////////////////////////////////////////////////////
        // Admin/Post/Index: Tra ve danh sach cac mau tin
        public ActionResult Index()
        {
            return View(postsDAO.getList("Index", "Page"));
        }

        ////////////////////////////////////////////////////////////////////
        // GET: Admin/Page/Create: Them moi mot mau tin
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Post/Create: Them moi mot mau tin
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Posts posts)
        {
            if (ModelState.IsValid)
            {
                //Xu ly cho muc Slug
                posts.Slug = XString.Str_Slug(posts.Title);
                //Chuyen doi dua vao truong Name de loai bo dau, khoang cach = dau -
                //Xu ly cho phan upload hình ảnh
                var img = Request.Files["img"];//Lay thong tin file
                if (img.ContentLength != 0)
                {
                    string[] FileExtentions = new string[] { ".jpg", ".jpeg", ".png", ".gif" };
                    //Kiem tra tap tin co hay khong
                    if (FileExtentions.Contains(img.FileName.Substring(img.FileName.LastIndexOf("."))))//Lay phan mo rong cua tap tin
                    {
                        string slug = posts.Slug;
                        //Ten file = Slug + Id + phan mo rong cua tap tin
                        string imgName = slug + img.FileName.Substring(img.FileName.LastIndexOf("."));
                        posts.Image = imgName;
                        string PathDir = "~/Public/img/page/";
                        string PathFile = Path.Combine(Server.MapPath(PathDir), imgName);
                        //upload hinh
                        img.SaveAs(PathFile);
                    }
                }//Ket thuc phan upload hinh anh
                //Xu ly cho muc PostType = page (doi voi Page)
                posts.PostType = "page";
                //Xu ly cho muc CreateAt
                posts.CreateAt = DateTime.Now;
                //Xu ly cho muc CreateBy
                posts.CreateBy = Convert.ToInt32(Session["UserId"]);
                //Xu ly cho muc UpdateAt
                posts.UpdateAt = DateTime.Now;
                //Xu ly cho muc UpdateBy
                posts.UpdateBy = Convert.ToInt32(Session["UserId"]);
                //Xu ly cho muc Topics
                if (postsDAO.Insert(posts) == 1)//Khi them du lieu thanh cong
                {
                    Links links = new Links();
                    links.Slug = posts.Slug;
                    links.TableId = posts.Id;
                    //Cap nhat link cho page
                    links.Type = "page";
                    linksDAO.Insert(links);
                }
                //Hien thi thong bao
                TempData["message"] = new XMessage("success", "Thêm trang đơn thành công");
                return RedirectToAction("Index");
            }
            return View(posts);
        }

        ////////////////////////////////////////////////////////////////////
        // GET: Admin/Page/Staus/5:Thay doi trang thai cua mau tin
        public ActionResult Status(int? id)
        {
            if (id == null)
            {
                //Hien thi thong bao
                TempData["message"] = new XMessage("danger", "Cập nhật trạng thái thất bại");
                //Chuyen huong trang
                return RedirectToAction("Index", "Page");
            }

            //Khi nhap nut thay doi Status cho mot mau tin
            Posts posts = postsDAO.getRow(id);
            //Kiem tra id cua posts co ton tai?
            if (posts == null)
            {
                //Hien thi thong bao
                TempData["message"] = new XMessage("danger", "Cập nhật trạng thái thất bại");

                //Chuyen huong trang
                return RedirectToAction("Index", "Page");
            }
            //Thay doi trang thai Status tu 1 thanh 2 va nguoc lai
            posts.Status = (posts.Status == 1) ? 2 : 1;
            //Cap nhat gia tri cho UpdateAt/By
            posts.UpdateBy = Convert.ToInt32(Session["UserId"].ToString());
            posts.UpdateAt = DateTime.Now;
            //Goi ham Update trong PostDAO
            postsDAO.Update(posts);
            //Hien thi thong bao
            TempData["message"] = new XMessage("success", "Cập nhật trạng thái thành công");
            //Khi cap nhat xong thi chuyen ve Index
            return RedirectToAction("Index", "Page");
        }

        ////////////////////////////////////////////////////////////////////
        // Admin/Post/Detail: Hien thi mot mau tin
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                //Hien thi thong bao
                TempData["message"] = new XMessage("danger", "Không tìm thấy trang đơn");
                //Chuyen huong trang
                return RedirectToAction("Index", "Page");
            }
            Posts posts = postsDAO.getRow(id);
            if (posts == null)
            {
                //Hien thi thong bao
                TempData["message"] = new XMessage("danger", "Không tìm thấy trang đơn");
                //Chuyen huong trang
                return RedirectToAction("Index", "Page");
            }
            return View(posts);
        }

        ////////////////////////////////////////////////////////////////////
        // GET: Admin/Page/Edit/5: Cap nhat mau tin
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                //Hien thi thong bao
                TempData["message"] = new XMessage("danger", "Cập nhật trang đơn thất bại");
                //Chuyen huong trang
                return RedirectToAction("Index", "Page");
            }
            Posts posts = postsDAO.getRow(id);
            if (posts == null)
            {
                //Hien thi thong bao
                TempData["message"] = new XMessage("danger", "Cập nhật trang đơn thất bại");
                //Chuyen huong trang
                return RedirectToAction("Index", "Page");
            }
            return View(posts);
        }

        // POST: Admin/Page/Edit/5: Cap nhat mau tin
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Posts posts)
        {
            if (ModelState.IsValid)
            {
                //Xu ly cho muc Slug
                posts.Slug = XString.Str_Slug(posts.Title);
                //Chuyen doi dua vao truong Name de loai bo dau, khoang cach = dau -
                //Xu ly cho phan upload hình ảnh
                var img = Request.Files["img"];//Lay thong tin file
                if (img.ContentLength != 0)
                {
                    string[] FileExtentions = new string[] { ".jpg", ".jpeg", ".png", ".gif" };
                    //kiem tra tap tin co hay khong
                    if (FileExtentions.Contains(img.FileName.Substring(img.FileName.LastIndexOf("."))))//Lay phan mo rong cua tap tin
                    {
                        string slug = posts.Slug;
                        //Chinh sua sau khi phat hien dieu chua dung cua Edit: them Id
                        //Ten file = Slug + Id + phan mo rong cua tap tin
                        string imgName = slug + img.FileName.Substring(img.FileName.LastIndexOf("."));
                        posts.Image = imgName;
                        string PathDir = "~/Public/img/page/";
                        string PathFile = Path.Combine(Server.MapPath(PathDir), imgName);
                        //upload hinh
                        img.SaveAs(PathFile);
                    }
                }//Ket thuc phan upload hinh anh
                //Xu ly cho muc PostType = page (doi voi Page)
                posts.PostType = "page";
                //Xu ly cho muc UpdateAt
                posts.UpdateAt = DateTime.Now;
                //Xu ly cho muc UpdateBy
                posts.UpdateBy = Convert.ToInt32(Session["UserId"]);
                //Xu ly cho muc Links
                if (postsDAO.Update(posts) == 1)//khi sua du lieu thanh cong
                {
                    Links links = new Links();
                    links.Slug = posts.Slug;
                    links.TableId = posts.Id;
                    //Thoi doi thong tin kieu Page
                    links.Type = "page";
                    linksDAO.Insert(links);
                }
                //Hien thi thong bao
                TempData["message"] = new XMessage("success", "Cập nhật trang đơn thành công");
                return RedirectToAction("Index");
            }
            return View(posts);
        }

        ////////////////////////////////////////////////////////////////////
        // GET: Admin/Page/DelTrash/5:Thay doi trang thai cua mau tin = 0
        public ActionResult DelTrash(int? id)
        {
            //Khi nhap nut thay doi Status cho mot mau tin
            Posts posts = postsDAO.getRow(id);
            //Thay doi trang thai Status tu 1,2 thanh 0
            posts.Status = 0;
            //Cap nhat gia tri cho UpdateAt/By
            posts.UpdateBy = Convert.ToInt32(Session["UserId"].ToString());
            posts.UpdateAt = DateTime.Now;
            //Goi ham Update trong PostDAO
            postsDAO.Update(posts);
            //Hien thi thong bao
            TempData["message"] = new XMessage("success", "Xóa trang đơn thành công");
            //Khi cap nhat xong thi chuyen ve Index
            return RedirectToAction("Index", "Page");
        }

        ////////////////////////////////////////////////////////////////////
        // GET: Admin/Posts/Trash/5:Hien thi cac mau tin có gia tri la 0
        public ActionResult Trash()
        {
            return View(postsDAO.getList("Trash", "page"));
        }

        ////////////////////////////////////////////////////////////////////
        // GET: Admin/Page/Recover/5:Chuyen trang thai Status = 0 thanh =2
        public ActionResult Undo(int? id)
        {
            if (id == null)
            {
                //Hien thi thong bao
                TempData["message"] = new XMessage("danger", "Phục hồi trang đơn thất bại");
                //Chuyen huong trang
                return RedirectToAction("Index", "Page");
            }

            //khi nhap nut thay doi Status cho mot mau tin
            Posts posts = postsDAO.getRow(id);
            //kiem tra id cua categories co ton tai?
            if (posts == null)
            {
                //Hien thi thong bao
                TempData["message"] = new XMessage("danger", "Phục hồi trang đơn thất bại");
                //Chuyen huong trang
                return RedirectToAction("Index", "Page");
            }
            //Thay doi trang thai Status tu 1 thanh 2 va nguoc lai
            posts.Status = 2;
            //Cap nhat gia tri cho UpdateAt/By
            posts.UpdateBy = Convert.ToInt32(Session["UserId"].ToString());
            posts.UpdateAt = DateTime.Now;
            //Goi ham Update trong postsDAO
            postsDAO.Update(posts);
            //Hien thi thong bao
            TempData["message"] = new XMessage("success", "Phục hồi trang đơn thành công");
            //Khi cap nhat xong thi chuyen ve Trash
            return RedirectToAction("Trash", "Page");
        }

        ////////////////////////////////////////////////////////////////////
        // GET: Admin/Page/Delete/5:Xoa mot mau tin ra khoi CSDL
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                //Hien thi thong bao
                TempData["message"] = new XMessage("danger", "Xóa trang đơn thất bại");
                //Chuyen huong trang
                return RedirectToAction("Trash", "Page");
            }
            Posts posts = postsDAO.getRow(id);
            if (posts == null)
            {
                //Hien thi thong bao
                TempData["message"] = new XMessage("danger", "Xóa trang đơn thất bại");
                //Chuyen huong trang
                return RedirectToAction("Trash", "Page");
            }
            return View(posts);
        }

        // POST: Admin/Page/Delete/5:Xoa mot mau tin ra khoi CSDL
        [HttpPost, ActionName("Delete")]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Posts posts = postsDAO.getRow(id);
            //Tim thay mau tin thi xoa, cap nhat cho Links
            if (postsDAO.Delete(posts) == 1)
            {
                Links links = linksDAO.getRow(posts.Id, "page");
                //Xoa luon cho Links
                linksDAO.Delete(links);
                //Duong dan den anh can xoa
                string PathDir = "~/Public/img/page/";
                //Tim tat ca hinh anh co ten slug giong nhau 
                string[] DelPath = Directory.GetFiles(Server.MapPath(PathDir), posts.Slug + ".*");
                //Xoa toan bo hinh anh cua mau tin
                foreach (string dp in DelPath)
                {
                    System.IO.File.Delete(dp);
                }
            }
            //Hien thi thong bao
            TempData["message"] = new XMessage("success", "Xóa trang đơn thành công");
            //O lai trang thung rac
            return RedirectToAction("Trash");
        }

    }
}
