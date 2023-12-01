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
    public class ProductController : Controller
    {
        ProductsDAO productsDAO = new ProductsDAO();
        CategoriesDAO categoriesDAO = new CategoriesDAO();
        SuppliersDAO suppliersDAO = new SuppliersDAO();

        ////////////////////////////////////////////////////////////////////
        // GET: Admin/Product/Index
        public ActionResult Index()
        {
            return View(productsDAO.getList("Index"));
        }

        ////////////////////////////////////////////////////////////////////
        // GET: Admin/Product/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                //Hien thi thong bao
                TempData["message"] = new XMessage("danger", "Không tìm thấy sản phẩm");
                return RedirectToAction("Index");
            }
            Products products = productsDAO.getRow(id);
            if (products == null)
            {
                //Hien thi thong bao
                TempData["message"] = new XMessage("danger", "Không tìm thấy sản phẩm");
                return RedirectToAction("Index");
            }
            //Hien thi ten loai san pham
            ViewBag.NameCat = categoriesDAO.getRow(products.CatID).Name;
            //Hien thi ten nha cung cap
            ViewBag.NameSup = suppliersDAO.getRow(products.SupplierID).Name;
            return View(products);
        }

        ////////////////////////////////////////////////////////////////////
        // GET: Admin/Product/Create
        public ActionResult Create()
        {
            ViewBag.ListCatId = new SelectList(categoriesDAO.getList("Index"), "Id", "Name");
            ViewBag.ListSupId = new SelectList(suppliersDAO.getList("Index"), "Id", "Name");
            return View();
        }

        // POST: Admin/Product/Create
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Products products)
        {
            if (ModelState.IsValid)
            {
                //Chinh sua tu dong cho cac truong sau:
                //CreateAt
                products.CreateAt = DateTime.Now;
                //CreateBy
                products.CreateBy = Convert.ToInt32(Session["UserID"]);
                //UpdateAt
                products.UpdateAt = DateTime.Now;
                //UpdateBy
                products.UpdateBy = Convert.ToInt32(Session["UserID"]);
                //Them du lieu vao DB
                productsDAO.Insert(products);
                //Slug 
                products.Slug = XString.Str_Slug(products.Name) + products.Id.ToString();
                //Xu ly thong tin cho hinh anh
                var img = Request.Files["img"];//Lay thong tin fLle
                if (img.ContentLength != 0)
                {
                    string[] FileExtentions = new string[] { ".jpg", ".jpeg", ".png", ".gif" };
                    //Kiem tra tap tin co hay khong
                    if (FileExtentions.Contains(img.FileName.Substring(img.FileName.LastIndexOf("."))))//Lay phan mo rong cua tap tin
                    { 
                        string slug = products.Slug;
                        //Ten file = Slug + phan mo rong cua tap tin
                        string imgName = slug + img.FileName.Substring(img.FileName.LastIndexOf("."));
                        products.Image = imgName;
                        //Upload hinh
                        string PathDir = "~/Public/img/product/";
                        string PathFile = Path.Combine(Server.MapPath(PathDir), imgName);
                        img.SaveAs(PathFile);
                    }
                }
                //Cap nhat du lieu DB
                productsDAO.Update(products);
                //Hien thi thong bao
                TempData["message"] = new XMessage("success", "Thêm mới sản phẩm thành công");
                return RedirectToAction("Index");
            }
            ViewBag.ListCatId = new SelectList(categoriesDAO.getList("Index"), "Id", "Name");
            ViewBag.ListSupId = new SelectList(suppliersDAO.getList("Index"), "Id", "Name");
            return View(products);
        }

        ////////////////////////////////////////////////////////////////////
        // GET: Admin/Product/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                //Hien thi thong bao
                TempData["message"] = new XMessage("danger", "Không tìm thấy sản phẩm");
                return RedirectToAction("Index");
            }
            Products products = productsDAO.getRow(id);
            if (products == null)
            {
                //Hien thi thong bao
                TempData["message"] = new XMessage("danger", "Không tìm thấy sản phẩm");
                return RedirectToAction("Index");
            }
            ViewBag.ListCatId = new SelectList(categoriesDAO.getList("Index"), "Id", "Name");
            ViewBag.ListSupId = new SelectList(suppliersDAO.getList("Index"), "Id", "Name");
            return View(products);
        }

        // POST: Admin/Product/Edit/5
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Products products)
        {
            if (ModelState.IsValid)
            {
                //Chinh sua tu dong cho cac truong sau:
                products.Slug = XString.Str_Slug(products.Name) + products.Id.ToString();
                //UpdateAt
                products.UpdateAt = DateTime.Now;
                //UpdateBy
                products.UpdateBy = Convert.ToInt32(Session["UserID"]);
                //Xu ly thong tin cho hinh anh
                var img = Request.Files["img"];//Lay thong tin file
                if (img.ContentLength != 0)
                {
                    string[] FileExtentions = new string[] { ".jpg", ".jpeg", ".png", ".gif" };
                    //Liem tra tap tin co hay khong
                    if (FileExtentions.Contains(img.FileName.Substring(img.FileName.LastIndexOf("."))))//Lay phan mo rong cua tap tin
                    {
                        string slug = products.Slug;
                        //Ten file = Slug + phan mo rong cua tap tin
                        string imgName = slug + img.FileName.Substring(img.FileName.LastIndexOf("."));
                        products.Image = imgName;
                        //Upload hinh
                        string PathDir = "~/Public/img/product/";
                        string PathFile = Path.Combine(Server.MapPath(PathDir), imgName);
                        img.SaveAs(PathFile);
                    }
                }
                //Cap nhat du lieu DB
                productsDAO.Update(products);
                //Hien thi thong bao
                TempData["message"] = new XMessage("success", "Cập nhật sản phẩm thành công");
                return RedirectToAction("Index");
            }
            return View(products);
        }

        ////////////////////////////////////////////////////////////////////
        // GET: Admin/Product/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                //Hien thi thong bao
                TempData["message"] = new XMessage("danger", "Không tìm thấy sản phẩm");
                return RedirectToAction("Trash");

            }
            Products products = productsDAO.getRow(id);
            if (products == null)
            {
                //Hien thi thong bao
                TempData["message"] = new XMessage("danger", "Không tìm thấy sản phẩm");
                return RedirectToAction("Trash");
            }
            //Hien thi ten loai san pham
            ViewBag.NameCat = categoriesDAO.getRow(products.CatID).Name;
            //Hien thi ten nha cung cap
            ViewBag.NameSup = suppliersDAO.getRow(products.SupplierID).Name;
            return View(products);
        }

        // POST: Admin/Product/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Products products = productsDAO.getRow(id);
            //Tìm hình và xoá hình cua mau tin
            if (productsDAO.Delete(products) == 1)
            {
                string PathDir = "~/Public/img/product/";
                if (products.Image != null)
                {
                    string fileName = System.IO.Path.GetFileNameWithoutExtension(products.Image);
                    //Tim tat ca hinh anh co ten giong nhau 
                    string[] DelPath = Directory.GetFiles(Server.MapPath(PathDir),  fileName + ".*");
                    //Xoa toan bo hinh anh cua mau tin
                    foreach (string dp in DelPath)
                    {
                        System.IO.File.Delete(dp);
                    }
                }
            }
            TempData["message"] = new XMessage("success", "Xóa sản phẩm thành công");
            return RedirectToAction("Trash");
        }

        ////////////////////////////////////////////////////////////////////
        // GET: Admin/Product/Status/5
        public ActionResult Status(int? id)
        {
            if (id == null)
            {
                //Hien thi thong bao
                TempData["message"] = new XMessage("danger", "Cập nhật trạng thái thất bại");
                return RedirectToAction("Index");
            }
            Products products = productsDAO.getRow(id);
            if (products == null)
            {
                //Hien thi thong bao
                TempData["message"] = new XMessage("danger", "Cập nhật trạng thái thất bại");
                return RedirectToAction("Index");
            }
            //Cap nhat trang thai
            products.Status = (products.Status == 1) ? 2 : 1;
            //Cap nhat UpdateAt
            products.UpdateAt = DateTime.Now;
            //Cap nhat UpdateBy
            products.UpdateBy = Convert.ToInt32(Session["UserID"]);
            //Cap nhat du lieu DB
            productsDAO.Update(products);
            //Hien thi thong bao
            TempData["message"] = new XMessage("success", "Cập nhật trạng thái thành công");
            return RedirectToAction("Index");
        }

        ////////////////////////////////////////////////////////////////////
        // GET: Admin/Product/DelTrash/5
        public ActionResult DelTrash(int? id)
        {
            if (id == null)
            {
                //Hien thi thong bao
                TempData["message"] = new XMessage("danger", "Xóa sản phẩm thất bại");
                return RedirectToAction("Index");
            }
            Products products = productsDAO.getRow(id);
            if (products == null)
            {
                //Hien thi thong bao
                TempData["message"] = new XMessage("danger", "Xóa sản phẩm thất bại");
                return RedirectToAction("Index");
            }
            //Cap nhat trang thai
            products.Status = 0;
            //Cap nhat UpdateAt
            products.UpdateAt = DateTime.Now;
            //Cap nhat UpdateBy
            products.UpdateBy = Convert.ToInt32(Session["UserID"]);
            //Cap nhat du lieu DB
            productsDAO.Update(products);
            //Hien thi thong bao
            TempData["message"] = new XMessage("success", "Xóa sản phẩm thành công");
            return RedirectToAction("Index");
        }

        ////////////////////////////////////////////////////////////////////
        // GET: Admin/Product/Trash
        public ActionResult Trash()
        {
            return View(productsDAO.getList("Trash"));
        }

        ////////////////////////////////////////////////////////////////////
        // GET: Admin/Product/Trash
        public ActionResult Undo(int? id)
        {
            if (id == null)
            {
                //Hien thi thong bao
                TempData["message"] = new XMessage("danger", "Phục hổi sản phẩm thất bại");
                return RedirectToAction("Trash");
            }
            Products products = productsDAO.getRow(id);
            if (products == null)
            {
                //Hien thi thong bao
                TempData["message"] = new XMessage("danger", "Phục hồi sản phẩm thất bại");
                return RedirectToAction("Trash");
            }
            //Cap nhat trang thai
            products.Status = 2;
            //Cap nhat UpdateAt
            products.UpdateAt = DateTime.Now;
            //Cap nhat UpdateBy
            products.UpdateBy = Convert.ToInt32(Session["UserID"]);
            //Cap nhat du lieu DB
            productsDAO.Update(products);
            //Hien thi thong bao
            TempData["message"] = new XMessage("success", "Phục hồi sản phẩm thành công");
            return RedirectToAction("Trash");
        }
    }
}
