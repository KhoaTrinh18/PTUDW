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
    public class PostController : Controller
    {
        PostsDAO postsDAO = new PostsDAO();
        LinksDAO linksDAO = new LinksDAO();
        TopicsDAO topicsDAO = new TopicsDAO();

        ////////////////////////////////////////////////////////////////////
        // Admin/Post/Index: Tra ve danh sach cac mau tin
        public ActionResult Index()
        {
            return View(postsDAO.getList("Index", "Post"));
        }

        ////////////////////////////////////////////////////////////////////
        // GET: Admin/Post/Create: Them moi mot mau tin
        public ActionResult Create()
        {
            ViewBag.TopList = new SelectList(topicsDAO.getList("Index"), "Id", "Name");
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
                    //kiem tra tap tin co hay khong
                    if (FileExtentions.Contains(img.FileName.Substring(img.FileName.LastIndexOf("."))))//Lay phan mo rong cua tap tin
                    {
                        string slug = posts.Slug;
                        //Ten file = Slug + Id + phan mo rong cua tap tin
                        string imgName = slug + img.FileName.Substring(img.FileName.LastIndexOf("."));
                        posts.Image = imgName;
                        string PathDir = "~/Public/img/post/";
                        string PathFile = Path.Combine(Server.MapPath(PathDir), imgName);
                        //Upload hinh
                        img.SaveAs(PathFile);
                    }
                }//Ket thuc phan upload hinh anh
                //Xu ly cho muc PostType
                posts.PostType = "post";
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
                    links.Type = "post";
                    linksDAO.Insert(links);
                }
                //Hien thi thong bao
                TempData["message"] = new XMessage("success", "Thêm bài viết thành công");
                return RedirectToAction("Index");
            }
            ViewBag.TopList = new SelectList(topicsDAO.getList("Index"), "Id", "Name");
            return View(posts);
        }

        ////////////////////////////////////////////////////////////////////
        // GET: Admin/Post/Staus/5:Thay doi trang thai cua mau tin
        public ActionResult Status(int? id)
        {
            if (id == null)
            {
                //Hien thi thong bao
                TempData["message"] = new XMessage("danger", "Cập nhật trạng thái thất bại");
                //Chuyen huong trang
                return RedirectToAction("Index", "Post");
            }
            //Khi nhap nut thay doi Status cho mot mau tin
            Posts posts = postsDAO.getRow(id);
            //Kiem tra id cua posts co ton tai?
            if (posts == null)
            {
                //Hien thi thong baoi
                TempData["message"] = new XMessage("danger", "Cập nhật trạng thái thất bại");
                //Chuyen huong trang
                return RedirectToAction("Index", "Post");
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
            //khi cap nhat xong thi chuyen ve Index
            return RedirectToAction("Index", "Post");
        }

        ////////////////////////////////////////////////////////////////////
        // Admin/Post/Detail: Hien thi mot mau tin
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                //Hien thi thong bao
                TempData["message"] = new XMessage("danger", "Không tìm thấy bài viết");
                //Chuyen huong trang
                return RedirectToAction("Index", "Post");
            }
            Posts posts = postsDAO.getRow(id);
            if (posts == null)
            {
                //Hien thi thong bao
                TempData["message"] = new XMessage("danger", "Không tìm thấy bài viết");
                //Chuyen huong trang
                return RedirectToAction("Index", "Post");
            }
            //Hien thi ten chu de
            ViewBag.NameTop = topicsDAO.getRow(posts.TopID).Name;
            return View(posts);
        }

        ////////////////////////////////////////////////////////////////////
        // GET: Admin/Category/Edit/5: Cap nhat mau tin
        public ActionResult Edit(int? id)
        {
            ViewBag.TopList = new SelectList(topicsDAO.getList("Index"), "Id", "Name");

            if (id == null)
            {
                //Hien thi thong bao
                TempData["message"] = new XMessage("danger", "Cập nhật bài viết thất bại");
                //Chuyen huong trang
                return RedirectToAction("Index", "Post");
            }
            Posts posts = postsDAO.getRow(id);
            if (posts == null)
            {
                //Hien thi thong bao
                TempData["message"] = new XMessage("danger", "Cập nhật bài viết thất bại");
                //Chuyen huong trang
                return RedirectToAction("Index", "Post");
            }
            return View(posts);
        }

        // POST: Admin/Post/Edit/5: Cap nhat mau tin
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
                        string PathDir = "~/Public/img/post/";
                        string PathFile = Path.Combine(Server.MapPath(PathDir), imgName);
                        //Upload hinh
                        img.SaveAs(PathFile);
                    }
                }//Ket thuc phan upload hinh anh
                //Xu ly cho muc PostType
                posts.PostType = "post";
                //Xu ly cho muc UpdateAt
                posts.UpdateAt = DateTime.Now;
                //Xu ly cho muc UpdateBy
                posts.UpdateBy = Convert.ToInt32(Session["UserId"]);
                //Xu ly cho muc Links
                if (postsDAO.Update(posts) == 1)//Khi sua du lieu thanh cong
                {
                    Links links = new Links();
                    links.Slug = posts.Slug;
                    links.TableId = posts.Id;
                    links.Type = "post";
                    linksDAO.Insert(links);
                }
                //Hien thi thong bao
                TempData["message"] = new XMessage("success", "Cập nhật bài viết thành công");
                return RedirectToAction("Index");
            }
            ViewBag.TopList = new SelectList(topicsDAO.getList("Index"), "Id", "Name");
            return View(posts);
        }

        ////////////////////////////////////////////////////////////////////
        // GET: Admin/Category/DelTrash/5:Thay doi trang thai cua mau tin = 0
        public ActionResult DelTrash(int? id)
        {
            //Khi nhap nut thay doi Status cho mot mau tin
            Posts posts = postsDAO.getRow(id);
            //Thay doi trang thai Status tu 1,2 thanh 0
            posts.Status = 0;
            //Cap nhat gia tri cho UpdateAt/By
            posts.UpdateBy = Convert.ToInt32(Session["UserId"].ToString());
            posts.UpdateAt = DateTime.Now;
            //Goi ham Update trong CategoryDAO
            postsDAO.Update(posts);
            //Hien thi thong bao
            TempData["message"] = new XMessage("success", "Xóa bài viết thành công");
            //Khi cap nhat xong thi chuyen ve Index
            return RedirectToAction("Index", "Post");
        }

        ////////////////////////////////////////////////////////////////////
        // GET: Admin/Posts/Trash/5:Hien thi cac mau tin có gia tri la 0
        public ActionResult Trash()
        {
            return View(postsDAO.getList("Trash"));
        }

        ////////////////////////////////////////////////////////////////////
        // GET: Admin/Post/Recover/5:Chuyen trang thai Status = 0 thanh =2
        public ActionResult Undo(int? id)
        {
            if (id == null)
            {
                //Hien thi thong bao
                TempData["message"] = new XMessage("danger", "Phục hồi bài viết thất bại");
                //Chuyen huong trang
                return RedirectToAction("Trash", "Post");
            }

            //khi nhap nut thay doi Status cho mot mau tin
            Posts posts = postsDAO.getRow(id);
            //kiem tra id cua categories co ton tai?
            if (posts == null)
            {
                //Hien thi thong bao
                TempData["message"] = new XMessage("danger", "Phục hồi bài viết thất bại");
                //Chuyen huong trang
                return RedirectToAction("Trash", "Post");
            }
            //Thay doi trang thai Status tu 1 thanh 2 va nguoc lai
            posts.Status = 2;
            //Cap nhat gia tri cho UpdateAt/By
            posts.UpdateBy = Convert.ToInt32(Session["UserId"].ToString());
            posts.UpdateAt = DateTime.Now;
            //Goi ham Update trong postsDAO
            postsDAO.Update(posts);
            //Hien thi thong bao
            TempData["message"] = new XMessage("success", "Phục hồi bài viết thành công");
            //khi cap nhat xong thi chuyen ve Trash
            return RedirectToAction("Trash", "Post");
        }

        ////////////////////////////////////////////////////////////////////
        // GET: Admin/Post/Delete/5:Xoa mot mau tin ra khoi CSDL
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                //Hien thi thong bao
                TempData["message"] = new XMessage("danger", "Xóa bài viết thất bại");
                //Chuyen huong trang
                return RedirectToAction("Trash", "Post");
            }
            Posts posts = postsDAO.getRow(id);
            if (posts == null)
            {
                //Hien thi thong bao
                TempData["message"] = new XMessage("danger", "Xóa bài viết thất bại");
                //Chuyen huong trang
                return RedirectToAction("Trash", "Post");
            }
            //Hien thi ten chu de
            ViewBag.NameTop = topicsDAO.getRow(posts.TopID).Name;
            return View(posts);
        }

        // POST: Admin/Category/Delete/5:Xoa mot mau tin ra khoi CSDL
        [HttpPost, ActionName("Delete")]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Posts posts = postsDAO.getRow(id);
            //tim thay mau tin thi xoa, cap nhat cho Links
            if (postsDAO.Delete(posts) == 1)
            {
                Links links = linksDAO.getRow(posts.Id, "post");
                //Xoa luon cho Links
                linksDAO.Delete(links);
                //Duong dan den anh can xoa
                string PathDir = "~/Public/img/post/";
                //Tim tat ca hinh anh co ten slug giong nhau 
                string[] DelPath = Directory.GetFiles(Server.MapPath(PathDir), posts.Slug + ".*");
                //Xoa toan bo hinh anh cua mau tin
                foreach (string dp in DelPath)
                {
                    System.IO.File.Delete(dp);
                }
            }
            //Hien thi thong bao
            TempData["message"] = new XMessage("success", "Xóa bài viết thành công");
            //O lai trang thung rac
            return RedirectToAction("Trash");
        }
    }
}
