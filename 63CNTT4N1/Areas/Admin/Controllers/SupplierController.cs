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
    public class SupplierController : Controller
    {
        SuppliersDAO suppliersDAO = new SuppliersDAO();
        ProductsDAO productsDAO = new ProductsDAO();

        ////////////////////////////////////////////////////////////////////
        // GET: Admin/Supplier/Index
        public ActionResult Index()
        {
            return View(suppliersDAO.getList("Index"));
        }

        ////////////////////////////////////////////////////////////////////
        // GET: Admin/Supplier/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                //Hien thi thong bao
                TempData["message"] = new XMessage("danger", "Không tìm thấy nhà cung cấp");
                return RedirectToAction("Index");
            }
            Suppliers suppliers = suppliersDAO.getRow(id);
            if (suppliers == null)
            {
                //Hien thi thong bao
                TempData["message"] = new XMessage("danger", "Không tìm thấy nhà cung cấp");
                return RedirectToAction("Index");
            }
            return View(suppliers);
        }

        ////////////////////////////////////////////////////////////////////
        // GET: Admin/Supplier/Create
        public ActionResult Create()
        {
            ViewBag.OrderList = new SelectList(suppliersDAO.getList("Index"), "Order", "Name");
            return View();
        }

        // POST: Admin/Supplier/Create
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Suppliers suppliers)
        {
            if (ModelState.IsValid)
            {
                //Chinh sua tu dong cho cac truong sau:
                //CreateAt
                suppliers.CreateAt = DateTime.Now;
                //CreateBy
                suppliers.CreateBy = Convert.ToInt32(Session["UserID"]);
                //Slug
                suppliers.Slug = XString.Str_Slug(suppliers.Name);
                //Order
                if (suppliers.Order == null)
                {
                    suppliers.Order = 1;
                }
                else
                {
                    suppliers.Order += 1;
                }
                //UpdateAt
                suppliers.UpdateAt = DateTime.Now;
                //UpdateBy
                suppliers.UpdateBy = Convert.ToInt32(Session["UserID"]);
                //Xu ly thong tin cho hinh anh
                var img = Request.Files["img"];//Lay thong tin file
                if (img.ContentLength != 0)
                {
                    string[] FileExtentions = new string[] { ".jpg", ".jpeg", ".png", ".gif" };
                    //Kiem tra tap tin co hay khong
                    if (FileExtentions.Contains(img.FileName.Substring(img.FileName.LastIndexOf("."))))//Lay phan mo rong cua tap tin
                    {
                        string slug = suppliers.Slug;
                        //ten file = Slug + phan mo rong cua tap tin
                        string imgName = slug + img.FileName.Substring(img.FileName.LastIndexOf("."));
                        suppliers.Image = imgName;
                        //Upload hinh
                        string PathDir = "~/Public/img/supplier/";
                        string PathFile = Path.Combine(Server.MapPath(PathDir), imgName);
                        img.SaveAs(PathFile);
                    }
                }
                //Them du lieu vao DB
                suppliersDAO.Insert(suppliers);
                //Hien thi thong bao
                TempData["message"] = new XMessage("success", "Thêm mới nhà cung cấp thành công");
                return RedirectToAction("Index");
            }
            ViewBag.OrderList = new SelectList(suppliersDAO.getList("Index"), "Order", "Name");
            return View(suppliers);
        }

        ////////////////////////////////////////////////////////////////////
        // GET: Admin/Supplier/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                //Hien thi thong bao
                TempData["message"] = new XMessage("danger", "Không tìm thấy nhà cung cấp");
                return RedirectToAction("Index");
            }
            Suppliers suppliers = suppliersDAO.getRow(id);
            if (suppliers == null)
            {
                //Hien thi thong bao
                TempData["message"] = new XMessage("danger", "Không tìm thấy nhà cung cấp");
                return RedirectToAction("Index");
            }
            ViewBag.OrderList = new SelectList(suppliersDAO.getList("Index"), "Order", "Name");
            return View(suppliers);
        }

        // POST: Admin/Supplier/Edit/5
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Suppliers suppliers)
        {
            if (ModelState.IsValid)
            {
                //Chinh sua tu dong cho cac truong sau:
                //slug
                suppliers.Slug = XString.Str_Slug(suppliers.Name);
                //Order
                if (suppliers.Order == null)
                {
                    suppliers.Order = 1;
                }
                else
                {
                    suppliers.Order += 1;
                }
                //UpdateAt
                suppliers.UpdateAt = DateTime.Now;
                //UpdateBy
                suppliers.UpdateBy = Convert.ToInt32(Session["UserID"]);
                //Xu ly thong tin cho hinh anh
                var img = Request.Files["img"];//Lay thong tin file
                if (img.ContentLength != 0)
                {
                    string[] FileExtentions = new string[] { ".jpg", ".jpeg", ".png", ".gif" };
                    //Kiem tra tap tin co hay khong
                    if (FileExtentions.Contains(img.FileName.Substring(img.FileName.LastIndexOf("."))))//Lay phan mo rong cua tap tin
                    {
                        string slug = suppliers.Slug;
                        //Ten file = Slug + phan mo rong cua tap tin
                        string imgName = slug + img.FileName.Substring(img.FileName.LastIndexOf("."));
                        suppliers.Image = imgName;
                        //Upload hinh
                        string PathDir = "~/Public/img/supplier/";
                        string PathFile = Path.Combine(Server.MapPath(PathDir), imgName);
                        img.SaveAs(PathFile);
                    }
                }
                //Cap nhat du lieu vao DB
                suppliersDAO.Update(suppliers);
                //Hien thi thong bao
                TempData["message"] = new XMessage("success", "Cập nhật nhà cung cấp thành công");
                return RedirectToAction("Index");
            }
            ViewBag.OrderList = new SelectList(suppliersDAO.getList("Index"), "Order", "Name");
            return View(suppliers);
        }

        ////////////////////////////////////////////////////////////////////
        // GET: Admin/Supplier/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                //Hien thi thong bao
                TempData["message"] = new XMessage("danger", "Không tìm thấy nhà cung cấp");
                return RedirectToAction("Trash");
            }
            Suppliers suppliers = suppliersDAO.getRow(id);
            if (suppliers == null)
            {
                //Hien thi thong bao
                TempData["message"] = new XMessage("danger", "Không tìm thấy nhà cung cấp");
                return RedirectToAction("Trash");
            }
            return View(suppliers);
        }

        // POST: Admin/Supplier/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Suppliers suppliers = suppliersDAO.getRow(id);
            var listSup = productsDAO.getList().Select(m => m.SupplierId);
            if (listSup.Contains(suppliers.Id))
            {
                //Hien thi thong bao
                TempData["message"] = new XMessage("danger", "Xóa các sản phẩm liên quan");
                return RedirectToAction("Delete", id);
            }
            //Tìm hình và xoá hình cua mau tin
            if (suppliersDAO.Delete(suppliers) == 1)
            {
                string PathDir = "~/Public/img/supplier/";
                if (suppliers.Image != null)
                {
                    //Tim tat ca hinh anh co ten slug giong nhau 
                    string[] DelPath = Directory.GetFiles(Server.MapPath(PathDir), suppliers.Slug + ".*");
                    //Xoa toan bo hinh anh cua mau tin
                    foreach (string dp in DelPath)
                    {
                        System.IO.File.Delete(dp);
                    }
                }
            }
            //Hien thi thong bao
            TempData["message"] = new XMessage("success", "Xoá nhà cung cấp thành công");
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
            Suppliers suppliers = suppliersDAO.getRow(id);
            if (suppliers == null)
            {
                //Hien thi thong bao
                TempData["message"] = new XMessage("danger", "Cập nhật trạng thái thất bại");
                return RedirectToAction("Index");
            }
            //Cap nhat trang thai
            suppliers.Status = (suppliers.Status == 1) ? 2 : 1;
            //Cap nhat UpdateAt
            suppliers.UpdateAt = DateTime.Now;
            //Cap nhat UpdateBy
            suppliers.UpdateBy = Convert.ToInt32(Session["UserID"]);
            //Cap nhat du lieu DB
            suppliersDAO.Update(suppliers);
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
                TempData["message"] = new XMessage("danger", "Xóa nhà cung cấp thất bại");
                return RedirectToAction("Index");
            }
            Suppliers suppliers = suppliersDAO.getRow(id);
            if (suppliers == null)
            {
                //Hien thi thong bao
                TempData["message"] = new XMessage("danger", "Xóa nhà cung cấp thất bại");
                return RedirectToAction("Index");
            }
            //Cap nhat trang thai
            suppliers.Status = 0;
            //Cap nhat UpdateAt
            suppliers.UpdateAt = DateTime.Now;
            //Cap nhat UpdateBy
            suppliers.UpdateBy = Convert.ToInt32(Session["UserID"]);
            //Cap nhat du lieu DB
            suppliersDAO.Update(suppliers);
            //Hien thi thong bao
            TempData["message"] = new XMessage("success", "Xóa nhà cung cấp thành công");
            return RedirectToAction("Index");
        }

        ////////////////////////////////////////////////////////////////////
        // GET: Admin/Category/Trash
        public ActionResult Trash()
        {
            return View(suppliersDAO.getList("Trash"));
        }

        ////////////////////////////////////////////////////////////////////
        // GET: Admin/Category/Undo
        public ActionResult Undo(int? id)
        {
            if (id == null)
            {
                //Hien thi thong bao
                TempData["message"] = new XMessage("danger", "Phục hồi nhà cung cấp thất bại");
                return RedirectToAction("Trash");
            }
            Suppliers suppliers = suppliersDAO.getRow(id);
            if (suppliers == null)
            {
                //Hien thi thong bao
                TempData["message"] = new XMessage("danger", "Phục hồi nhà cung cấp thất bại");
                return RedirectToAction("Trash");
            }
            //Cap nhat trang thai
            suppliers.Status = 2;
            //Cap nhat UpdateAt
            suppliers.UpdateAt = DateTime.Now;
            //Cap nhat UpdateBy
            suppliers.UpdateBy = Convert.ToInt32(Session["UserID"]);
            //Cap nhat du lieu DB
            suppliersDAO.Update(suppliers);
            //Hien thi thong bao
            TempData["message"] = new XMessage("success", "Phục hồi nhà cung cấp thành công");
            return RedirectToAction("Trash");
        }
    }
}
