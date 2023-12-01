using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices.ComTypes;
using System.Web;
using System.Web.Mvc;
using _63CNTT4N1.Library;
using MyClass.DAO;
using MyClass.Model;

namespace _63CNTT4N1.Areas.Admin.Controllers
{
    public class TopicController : Controller
    {
        TopicsDAO topicsDAO = new TopicsDAO();
        LinksDAO linksDAO = new LinksDAO();
        PostsDAO postsDAO = new PostsDAO();

        ////////////////////////////////////////////////////////////////////
        // Admin/Topic/Index: Tra ve danh sach cac mau tin
        public ActionResult Index()
        {
            return View(topicsDAO.getList("Index"));
        }

        ////////////////////////////////////////////////////////////////////
        // GET: Admin/Topic/Create: Them moi mot mau tin
        public ActionResult Create()
        {
            ViewBag.ListTopic = new SelectList(topicsDAO.getList("Index"), "Id", "Name");
            ViewBag.OrderTopic = new SelectList(topicsDAO.getList("Index"), "Order", "Name");
            return View();
        }

        // POST: Admin/Topic/Create: Them moi mot mau tin
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Topics topics)
        {
            if (ModelState.IsValid)
            {
                //Xu ly cho muc Slug
                topics.Slug = XString.Str_Slug(topics.Name);
                //Chuyen doi dua vao truong Name de loai bo dau, khoang cach = dau -
                //Xu ly cho muc ParentId
                if (topics.ParentId == null)
                {
                    topics.ParentId = 0;
                }
                //Xu ly cho muc Order
                if (topics.Order == null)
                {
                    topics.Order = 1;
                }
                else
                {
                    topics.Order = topics.Order + 1;
                }
                //Xu ly cho muc CreateAt
                topics.CreateAt = DateTime.Now;
                //Xu ly cho muc CreateBy
                topics.CreateBy = Convert.ToInt32(Session["UserId"]);
                //Xu ly cho muc UpdateAt
                topics.UpdateAt = DateTime.Now;
                //Xu ly cho muc UpdateBy
                topics.UpdateBy = Convert.ToInt32(Session["UserId"]);
                //xu ly cho muc Topics
                if (topicsDAO.Insert(topics) == 1)//Khi them du lieu thanh cong
                {
                    Links links = new Links();
                    links.Slug = topics.Slug;
                    links.TableId = topics.Id;
                    links.Type = "topic";
                    linksDAO.Insert(links);
                }
                //Hien thi thong bao
                TempData["message"] = new XMessage("success", "Thêm chủ đề thành công");
                return RedirectToAction("Index");
            }
            ViewBag.ListTopic = new SelectList(topicsDAO.getList("Index"), "Id", "Name");
            ViewBag.OrderTopic = new SelectList(topicsDAO.getList("Index"), "Order", "Name");
            return View(topics);
        }

        ////////////////////////////////////////////////////////////////////
        // GET: Admin/Topic/Staus/5:Thay doi trang thai cua mau tin
        public ActionResult Status(int? id)
        {
            if (id == null)
            {
                //Hien thi thong bao
                TempData["message"] = new XMessage("danger", "Cập nhật trạng thái thất bại");
                //Chuyen huong trang
                return RedirectToAction("Index", "Topic");
            }
            //Khi nhap nut thay doi Status cho mot mau tin
            Topics topics = topicsDAO.getRow(id);
            //kiem tra id cua topics co ton tai?
            if (topics == null)
            {
                //Hien thi thong bao
                TempData["message"] = new XMessage("danger", "Cập nhật trạng thái thất bại");
                //Chuyen huong trang
                return RedirectToAction("Index", "Topic");
            }
            //thay doi trang thai Status tu 1 thanh 2 va nguoc lai
            topics.Status = (topics.Status == 1) ? 2 : 1;
            //cap nhat gia tri cho UpdateAt/By
            topics.UpdateBy = Convert.ToInt32(Session["UserId"].ToString());
            topics.UpdateAt = DateTime.Now;
            //Goi ham Update trong TopicDAO
            topicsDAO.Update(topics);
            //Hien thi thong bao
            TempData["message"] = new XMessage("success", "Cập nhật trạng thái thành công");
            //khi cap nhat xong thi chuyen ve Index
            return RedirectToAction("Index", "Topic");
        }

        ////////////////////////////////////////////////////////////////////
        // Admin/Topic/Detail: Hien thi mot mau tin
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                //Hien thi thong bao
                TempData["message"] = new XMessage("danger", "Không tìm thấy chủ đề");
                //Chuyen huong trang
                return RedirectToAction("Index", "Topic");
            }
            Topics topics = topicsDAO.getRow(id);
            if (topics == null)
            {
                //Hien thi thong bao
                TempData["message"] = new XMessage("danger", "Không tìm thấy chủ để");
                //Chuyen huong trang
                return RedirectToAction("Index", "Topic");
            }
            //Hien thi ten cap cha
            if (topicsDAO.getRow(topics.ParentId) == null)
            {
                ViewBag.ParentTop = "Cấp 0";
            }
            else
            {
                ViewBag.ParentTop = topicsDAO.getRow(topics.ParentId).Name;
            }
            return View(topics);
        }

        ////////////////////////////////////////////////////////////////////
        // GET: Admin/Topic/Edit/5: Cap nhat mau tin
        public ActionResult Edit(int? id)
        {
            ViewBag.ListTopic = new SelectList(topicsDAO.getList("Index"), "Id", "Name");
            ViewBag.OrderTopic = new SelectList(topicsDAO.getList("Index"), "Order", "Name");
            if (id == null)
            {
                //Hien thi thong bao
                TempData["message"] = new XMessage("danger", "Cập nhật chủ đề thất bại");
                //Chuyen huong trang
                return RedirectToAction("Index", "Topic");
            }
            Topics topics = topicsDAO.getRow(id);

            if (topics == null)
            {
                //Hien thi thong bao
                TempData["message"] = new XMessage("danger", "Cập nhật chủ đề thất bại");
                //Chuyen huong trang
                return RedirectToAction("Index", "Topic");
            }
            return View(topics);
        }

        // POST: Admin/Topic/Edit/5: Cap nhat mau tin
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Topics topics)
        {
            if (ModelState.IsValid)
            {
                //Xu ly cho muc Slug
                topics.Slug = XString.Str_Slug(topics.Name);
                //chuyen doi dua vao truong Name de loai bo dau, khoang cach = dau 
                //Xu ly cho muc ParentId
                if (topics.ParentId == null)
                {
                    topics.ParentId = 0;
                }
                //Xu ly cho muc Order
                if (topics.Order == null)
                {
                    topics.Order = 1;
                }
                else
                {
                    topics.Order = topics.Order + 1;
                }
                //Xy ly cho muc UpdateAt
                topics.UpdateAt = DateTime.Now;
                //Xy ly cho muc UpdateBy
                topics.UpdateBy = Convert.ToInt32(Session["UserId"]);
                //Cap nhat du lieu, sua them cho phan Links phuc vu cho Topics
                if (topicsDAO.Update(topics) == 1)
                {
                    //Neu trung khop thong tin: Type = category va TableID = categories.ID
                    Links links = linksDAO.getRow(topics.Id, "topic");
                    //Cap nhat lai thong tin
                    links.Slug = topics.Slug;
                    linksDAO.Update(links);
                }
                //Hien thi thong bao
                TempData["message"] = new XMessage("success", "Cập nhật chủ đề thành công");
                return RedirectToAction("Index");
            }
            return View(topics);
        }

        ////////////////////////////////////////////////////////////////////
        // GET: Admin/Topic/DelTrash/5:Thay doi trang thai cua mau tin = 0
        public ActionResult DelTrash(int? id)
        {
            //Khi nhap nut thay doi Status cho mot mau tin
            Topics topics = topicsDAO.getRow(id);
            //Thay doi trang thai Status tu 1,2 thanh 0
            topics.Status = 0;
            //Cap nhat gia tri cho UpdateAt/By
            topics.UpdateBy = Convert.ToInt32(Session["UserId"].ToString());
            topics.UpdateAt = DateTime.Now;
            //Goi ham Update trong TopicDAO
            topicsDAO.Update(topics);
            //Thong bao thanh cong
            TempData["message"] = new XMessage("success", "Xóa chủ đề thành công");
            //khi cap nhat xong thi chuyen ve Index
            return RedirectToAction("Index", "Topic");
        }

        ////////////////////////////////////////////////////////////////////
        // GET: Admin/Topic/Trash/5:Hien thi cac mau tin có gia tri la 0
        public ActionResult Trash()
        {
            return View(topicsDAO.getList("Trash"));
        }

        ////////////////////////////////////////////////////////////////////
        // GET: Admin/Topic/Recover/5:Chuyen trang thai Status = 0 thanh =2
        public ActionResult Undo(int? id)
        {
            if (id == null)
            {
                //Hien thi thong bao
                TempData["message"] = new XMessage("danger", "Phục hồi chủ đề thất bại");
                //Chuyen huong trang
                return RedirectToAction("Trash", "Topic");
            }
            //Khi nhap nut thay doi Status cho mot mau tin
            Topics topics = topicsDAO.getRow(id);
            //Kiem tra id cua categories co ton tai?
            if (topics == null)
            {
                //Hien thi thong bao
                TempData["message"] = new XMessage("danger", "Phục hồi chủ đề thất bại");
                //Chuyen huong trang
                return RedirectToAction("Trash", "Topic");
            }
            //Thay doi trang thai Status tu 1 thanh 2 va nguoc lai
            topics.Status = 2;
            //Cap nhat gia tri cho UpdateAt/By
            topics.UpdateBy = Convert.ToInt32(Session["UserId"].ToString());
            topics.UpdateAt = DateTime.Now;
            //Goi ham Update trong TopicDAO
            topicsDAO.Update(topics);
            //Hien thi thong bao
            TempData["message"] = new XMessage("success", "Phục hồi chủ đề thành công");
            //khi cap nhat xong thi chuyen ve Trash
            return RedirectToAction("Trash", "Topic");
        }

        ////////////////////////////////////////////////////////////////////
        // GET: Admin/Topic/Delete/5:Xoa mot mau tin ra khoi CSDL
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                //Hien thi thong bao
                TempData["message"] = new XMessage("danger", "Xóa chủ đề thất bại");
                //Chuyen huong trang
                return RedirectToAction("Trash", "Topic");
            }
            Topics topics = topicsDAO.getRow(id);
            if (topics == null)
            {
                //Hien thi thong bao
                TempData["message"] = new XMessage("danger", "Xóa chủ đề thất bại");
                //Chuyen huong trang
                return RedirectToAction("Trash", "Topic");
            }
            //Hien thi ten cap cha
            if (topicsDAO.getRow(topics.ParentId) == null)
            {
                ViewBag.ParentTop = "Cấp 0";
            }
            else
            {
                ViewBag.ParentTop = topicsDAO.getRow(topics.ParentId).Name;
            }
            return View(topics);
        }

        // POST: Admin/Category/Delete/5:Xoa mot mau tin ra khoi CSDL
        [HttpPost, ActionName("Delete")]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Topics topics = topicsDAO.getRow(id);
            var listPos = postsDAO.getList().Select(m => m.TopID);
            if (listPos.Contains(topics.Id))
            {
                //Hien thi thong bao
                TempData["message"] = new XMessage("danger", "Xóa các bài viết liên quan");
                return RedirectToAction("Delete", id);
            }
            //Tim thay mau tin thi xoa, cap nhat cho Links
            if (topicsDAO.Delete(topics) == 1)
            {
                Links links = linksDAO.getRow(topics.Id, "topic");
                //Xoa luon cho Links
                linksDAO.Delete(links);
            }
            //Hien thi thong bao
            TempData["message"] = new XMessage("success", "Xóa chủ đề thành công");
            //O lai trang thung rac
            return RedirectToAction("Trash");
        }
    }
}