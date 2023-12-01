using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MyClass.Model;
using MyClass.DAO;
using _63CNTT4N1.Library;

namespace _63CNTT4N1.Areas.Admin.Controllers
{
    public class CategoryController : Controller
    {
        CategoriesDAO categoriesDAO = new CategoriesDAO();
        LinksDAO linksDAO = new LinksDAO();
        ProductsDAO productsDAO = new ProductsDAO();

        ////////////////////////////////////////////////////////////////////
        // GET: Admin/Category/Index
        public ActionResult Index()
        {
            return View(categoriesDAO.getList("Index"));
        }

        ////////////////////////////////////////////////////////////////////
        // GET: Admin/Category/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                //Hien thi thong bao
                TempData["message"] = new XMessage("danger", "Không tìm thấy loại sản phẩm");
                return RedirectToAction("Index");
            }
            Categories categories = categoriesDAO.getRow(id);
            if (categories == null)
            {
                //Hien thi thong bao
                TempData["message"] = new XMessage("danger", "Không tìm thấy loại sản phẩm");
                return RedirectToAction("Index");
            }
            //Hien thi ten cap cha
            if (categoriesDAO.getRow(categories.ParentId) == null)
            {
                ViewBag.ParentCat = "Cấp 0";
            }
            else
            {
                ViewBag.ParentCat = categoriesDAO.getRow(categories.ParentId).Name;
            }
            return View(categories);
        }

        ////////////////////////////////////////////////////////////////////
        // GET: Admin/Category/Create
        public ActionResult Create()
        {
            ViewBag.ListCat = new SelectList(categoriesDAO.getList("Index"), "Id", "Name");
            ViewBag.OrderList = new SelectList(categoriesDAO.getList("Index"), "Order", "Name");
            return View();
        }

        // POST: Admin/Category/Create/5
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Categories categories)
        {
            if (ModelState.IsValid)
            {
                //Chinh sua tu dong cho cac truong sau:
                //CreateAt
                categories.CreateAt = DateTime.Now;
                //CreateBy
                categories.CreateBy = Convert.ToInt32(Session["UserID"]);
                //Slug
                categories.Slug = XString.Str_Slug(categories.Name);
                //ParentID
                if(categories.ParentId == null)
                {
                    categories.ParentId = 0;
                }
                //Order
                if(categories.Order == null)
                {
                    categories.Order = 1;
                }
                else
                {
                    categories.Order += 1;
                }
                //UpdateAt
                categories.UpdateAt = DateTime.Now;
                //UpdateBy
                categories.UpdateBy = Convert.ToInt32(Session["UserID"]);
                //Xu ly cho muc Topics
                if (categoriesDAO.Insert(categories) == 1)//khi them du lieu thanh cong
                {
                    Links links = new Links();
                    links.Slug = categories.Slug;
                    links.TableId = categories.Id;
                    links.Type = "category";
                    linksDAO.Insert(links);
                }
                //Hien thi thong bao 
                TempData["message"] = new XMessage("success", "Tạo mới loại sản phẩm thành công");
                return RedirectToAction("Index");
            }
            ViewBag.ListCat = new SelectList(categoriesDAO.getList("Index"), "Id", "Name");
            ViewBag.OrderList = new SelectList(categoriesDAO.getList("Index"), "Order", "Name");
            return View(categories);
        }

        ////////////////////////////////////////////////////////////////////
        // GET: Admin/Category/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                //Hien thi thong bao
                TempData["message"] = new XMessage("danger", "Cập nhật loại sản phẩm thất bại");
                return RedirectToAction("Index");
            }
            Categories categories = categoriesDAO.getRow(id);
            if (categories == null)
            {
                //Hien thi thong bao
                TempData["message"] = new XMessage("danger", "Cập nhật loại sản phẩm thất bại");
                return RedirectToAction("Index");
            }
            ViewBag.ListCat = new SelectList(categoriesDAO.getList("Index"), "Id", "Name");
            ViewBag.OrderList = new SelectList(categoriesDAO.getList("Index"), "Order", "Name");
            return View(categories);
        }

        // POST: Admin/Category/Edit/5
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Categories categories)
        {
            if (ModelState.IsValid)
            {
                //Chinh sua tu dong cho cac truong sau:
                //Slug
                categories.Slug = XString.Str_Slug(categories.Name);
                //ParentID
                if (categories.ParentId == null)
                {
                    categories.ParentId = 0;
                }
                //Order
                if (categories.Order == null)
                {
                    categories.Order = 1;
                }
                else
                {
                    categories.Order += 1;
                }
                //UpdateAt
                categories.UpdateAt = DateTime.Now;
                //UpdateBy
                categories.UpdateBy = Convert.ToInt32(Session["UserID"]);            
                //Cap nhat du lieu, sua them cho phan Links phuc vu cho Topics
                if (categoriesDAO.Update(categories) == 1)
                {
                    //Neu trung khop thong tin: Type = category va TableID = categories.ID
                    Links links = linksDAO.getRow(categories.Id, "category");
                    //Cap nhat lai thong tin
                    links.Slug = categories.Slug;
                    linksDAO.Update(links);
                }
                //Hien thi thong bao 
                TempData["message"] = new XMessage("success", "Cập nhật loại sản phẩm thành công");
                return RedirectToAction("Index");
            }
            ViewBag.ListCat = new SelectList(categoriesDAO.getList("Index"), "Id", "Name");
            ViewBag.OrderList = new SelectList(categoriesDAO.getList("Index"), "Order", "Name");
            return View(categories);
        }

        ////////////////////////////////////////////////////////////////////
        // GET: Admin/Category/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                //Hien thi thong bao
                TempData["message"] = new XMessage("danger", "Xóa loại sản phẩm thất bại");
                return RedirectToAction("Trash");
            }
            Categories categories = categoriesDAO.getRow(id);
            if (categories == null)
            {
                //Hien thi thong bao
                TempData["message"] = new XMessage("danger", "Xóa loại sản phẩm thất bại");
                return RedirectToAction("Trash");
            }
            //Hien thi ten cap cha
            if (categoriesDAO.getRow(categories.ParentId) == null)
            {
                ViewBag.ParentCat = "Cấp 0";
            }
            else
            {
                ViewBag.ParentCat = categoriesDAO.getRow(categories.ParentId).Name;
            }
            return View(categories);
        }

        // POST: Admin/Category/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Categories categories = categoriesDAO.getRow(id);
            var listCat = productsDAO.getList().Select(m => m.CatID);
            if (listCat.Contains(categories.Id))
            {
                //Hien thi thong bao
                TempData["message"] = new XMessage("danger", "Xóa các sản phẩm liên quan");
                return RedirectToAction("Delete", id);
            }
            //Tim thay mau tin thi xoa, cap nhat cho Links
            if (categoriesDAO.Delete(categories) == 1)
            {
                Links links = linksDAO.getRow(categories.Id, "category");
                //Xoa luon cho Links
                linksDAO.Delete(links);
            }
            //Hien thi thong bao
            TempData["message"] = new XMessage("success", "Xóa loại sản phẩm thành công");
            return RedirectToAction("Trash");
        }

        ////////////////////////////////////////////////////////////////////
        // GET: Admin/Category/Status/5
        public ActionResult Status(int? id)
        {
            if (id == null)
            {
                //Hien thi thong bao
                TempData["message"] = new XMessage("danger", "Cập nhật trạng thái thất bại");
                return RedirectToAction("Index");
            }
            Categories categories = categoriesDAO.getRow(id);
            if (categories == null)
            {
                //Hien thi thong bao
                TempData["message"] = new XMessage("danger", "Cập nhật trạng thái thất bại");
                return RedirectToAction("Index");
            }
            //Cap nhat trang thai
            categories.Status = (categories.Status == 1) ? 2 : 1;
            //Cap nhat UpdateAt
            categories.UpdateAt = DateTime.Now;
            //Cap nhat UpdateBy
            categories.UpdateBy = Convert.ToInt32(Session["UserID"]);
            //Cap nhat du lieu DB
            categoriesDAO.Update(categories);
            //Hien thi thong bao
            TempData["message"] = new XMessage("success", "Cập nhật trạng thái thành công");
            return RedirectToAction("Index");
        }

        ////////////////////////////////////////////////////////////////////
        // GET: Admin/Category/DelTrash/5
        public ActionResult DelTrash(int? id)
        {
            if (id == null)
            {
                //Hien thi thong bao
                TempData["message"] = new XMessage("danger", "Xóa loại sản phẩm thất bại");
                return RedirectToAction("Index");
            }
            Categories categories = categoriesDAO.getRow(id);
            if (categories == null)
            {
                //Hien thi thong bao
                TempData["message"] = new XMessage("danger", "Xóa loại sản phẩm thất bại");
                return RedirectToAction("Index");
            }
            //Cap nhat trang thai
            categories.Status = 0;
            //Cap nhat UpdateAt
            categories.UpdateAt = DateTime.Now;
            //Cap nhat UpdateBy
            categories.UpdateBy = Convert.ToInt32(Session["UserID"]);
            //Cap nhat du lieu DB
            categoriesDAO.Update(categories);
            //Hien thi thong bao
            TempData["message"] = new XMessage("success", "Xóa loại sản phẩm thành công");
            return RedirectToAction("Index");
        }

        ////////////////////////////////////////////////////////////////////
        // GET: Admin/Category/Trash
        public ActionResult Trash()
        {
            return View(categoriesDAO.getList("Trash"));
        }

        ////////////////////////////////////////////////////////////////////
        // GET: Admin/Category/Undo
        public ActionResult Undo(int? id)
        {
            if (id == null)
            {
                //Hien thi thong bao
                TempData["message"] = new XMessage("danger", "Phục hồi loại sản phẩm thất bại");
                return RedirectToAction("Trash");
            }
            Categories categories = categoriesDAO.getRow(id);
            if (categories == null)
            {
                //Hien thi thong bao
                TempData["message"] = new XMessage("danger", "Phục hồi loại sản phẩm thất bại");
                return RedirectToAction("Trash");
            }
            //Cap nhat trang thai
            categories.Status = 2;
            //Cap nhat UpdateAt
            categories.UpdateAt = DateTime.Now;
            //Cap nhat UpdateBy
            categories.UpdateBy = Convert.ToInt32(Session["UserID"]);
            //Cap nhat du lieu DB
            categoriesDAO.Update(categories);
            //Hien thi thong bao
            TempData["message"] = new XMessage("success", "Phục hồi loại hàng thành công");
            return RedirectToAction("Trash");
        }
    }
}