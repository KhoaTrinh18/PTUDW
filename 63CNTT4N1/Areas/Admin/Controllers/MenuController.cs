using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using _63CNTT4N1.Library;
using MyClass.DAO;
using MyClass.Model;

namespace _63CNTT4N1.Areas.Admin.Controllers
{
    public class MenuController : Controller
    {
        CategoriesDAO categoriesDAO = new CategoriesDAO();
        SuppliersDAO suppliersDAO = new SuppliersDAO();
        ProductsDAO productsDAO = new ProductsDAO();
        MenusDAO menusDAO = new MenusDAO();
        TopicsDAO topicsDAO = new TopicsDAO();
        PostsDAO postsDAO = new PostsDAO();

        ////////////////////////////////////////////////////////////////
        // GET: Admin/Menu/Index
        public ActionResult Index()
        {
            ViewBag.CatList = categoriesDAO.getList("Index");
            ViewBag.SupList = suppliersDAO.getList("Index");
            ViewBag.ProList = productsDAO.getList("Index");
            ViewBag.TopList = topicsDAO.getList("Index");
            ViewBag.PosList = postsDAO.getList("Index", "Page");
            return View(menusDAO.getList("Index"));
        }

        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Index(FormCollection form)
        {
            //Them loai san pham
            if (!string.IsNullOrEmpty(form["ThemCategory"]))
            {
                //Kiem tra dau check cua muc con
                if (!string.IsNullOrEmpty(form["nameCategory"]))
                {
                    var listitem = form["nameCategory"];
                    //Chuyen danh sach thanh dang mang: 1,2,3,4...
                    var listarr = listitem.Split(',');//Ngat mang thanh tung phan tu cach nhau boi dau ,
                    foreach (var row in listarr)
                    {
                        int id = int.Parse(row);//Ep kieu int
                                                //Lay 1 ban ghi
                        Categories categories = categoriesDAO.getRow(id);
                        //Tao ra menu
                        Menus menu = new Menus();
                        menu.Name = categories.Name;
                        menu.Link = categories.Slug;
                        menu.TypeMenu = "category";
                        menu.Position = form["Position"];
                        menu.ParentID = 0;
                        menu.Order = 0;
                        menu.CreateAt = DateTime.Now;
                        menu.CreateBy = Convert.ToInt32(Session["UserID"].ToString());
                        menu.UpdateAt = DateTime.Now;
                        menu.UpdateBy = Convert.ToInt32(Session["UserID"].ToString());
                        menu.Status = 2; //Tam thoi chua xuat ban
                                         //Them vao DB
                        menusDAO.Insert(menu);
                    }
                    TempData["message"] = new XMessage("success", "Thêm vào menu thành công");
                }
                else
                {
                    TempData["message"] = new XMessage("danger", "Chưa chọn loại sản phẩm");
                }
            }
            //Them nha cung cap
            if (!string.IsNullOrEmpty(form["ThemSupplier"]))
            {
                //Kiem tra dau check cua muc con
                if (!string.IsNullOrEmpty(form["nameSupplier"]))
                {
                    var listitem = form["nameSupplier"];
                    //Chuyen danh sach thanh dang mang: 1,2,3,4...
                    var listarr = listitem.Split(',');//ngat mang thanh tung phan tu cach nhau boi dau ,
                    foreach (var row in listarr)
                    {
                        int id = int.Parse(row);//Ep kieu int
                                                //Lay 1 ban ghi
                        Suppliers suppliers = suppliersDAO.getRow(id);
                        //Tao ra menu
                        Menus menu = new Menus();
                        menu.Name = suppliers.Name;
                        menu.Link = suppliers.Slug;
                        menu.TypeMenu = "supplier";
                        menu.Position = form["Position"];
                        menu.ParentID = 0;
                        menu.Order = 0;
                        menu.CreateAt = DateTime.Now;
                        menu.CreateBy = Convert.ToInt32(Session["UserID"].ToString());
                        menu.UpdateAt = DateTime.Now;
                        menu.UpdateBy = Convert.ToInt32(Session["UserID"].ToString());
                        menu.Status = 2; //Tam thoi chua xuat ban
                                         //Them vao DB
                        menusDAO.Insert(menu);
                    }
                    TempData["message"] = new XMessage("success", "Thêm vào menu thành công");
                }
                else
                {
                    TempData["message"] = new XMessage("danger", "Chưa chọn nhà cung cấp");
                }
            }
            //Them san pham
            if (!string.IsNullOrEmpty(form["ThemProduct"]))
            {
                //kiem tra dau check cua muc con
                if (!string.IsNullOrEmpty(form["nameProduct"]))
                {
                    var listitem = form["nameProduct"];
                    //Chuyen danh sach thanh dang mang: 1,2,3,4...
                    var listarr = listitem.Split(',');//Ngat mang thanh tung phan tu cach nhau boi dau ,
                    foreach (var row in listarr)
                    {
                        int id = int.Parse(row);//Ep kieu int
                                                //Lay 1 ban ghi
                        Products products = productsDAO.getRow(id);
                        //Tao ra menu
                        Menus menu = new Menus();
                        menu.Name = products.Name;
                        menu.Link = products.Slug;
                        menu.TypeMenu = "product";
                        menu.Position = form["Position"];
                        menu.ParentID = 0;
                        menu.Order = 0;
                        menu.CreateAt = DateTime.Now;
                        menu.CreateBy = Convert.ToInt32(Session["UserID"].ToString());
                        menu.UpdateAt = DateTime.Now;
                        menu.UpdateBy = Convert.ToInt32(Session["UserID"].ToString());
                        menu.Status = 2; //Tam thoi chua xuat ban
                                         //Them vao DB
                        menusDAO.Insert(menu);
                    }
                    TempData["message"] = new XMessage("success", "Thêm vào menu thành công");
                }
                else
                {
                    TempData["message"] = new XMessage("danger", "Chưa chọn sản phẩm");
                }
            }
            //Them chu de
            if (!string.IsNullOrEmpty(form["ThemTopic"]))//nut ThemCategory duoc nhan
            {
                if (!string.IsNullOrEmpty(form["nameTopic"]))//check box được nhấn
                {
                    var listitem = form["nameTopic"];
                    //chuyen danh sach thanh dang mang: vi du 1,2,3,...
                    var listarr = listitem.Split(',');//cat theo dau ,
                    foreach (var row in listarr)//row = id cua các mau tin
                    {
                        int id = int.Parse(row);//ep kieu int
                                                //lay 1 ban ghi
                        Topics topics = topicsDAO.getRow(id);
                        //tao ra menu
                        Menus menu = new Menus();
                        menu.Name = topics.Name;
                        menu.Link = topics.Slug;
                        menu.TableID = topics.Id;
                        menu.TypeMenu = "topic";
                        menu.Position = form["Position"];
                        menu.ParentID = 0;
                        menu.Order = 0;
                        menu.CreateBy = Convert.ToInt32(Session["UserId"].ToString());
                        menu.CreateAt = DateTime.Now;
                        menu.UpdateAt = DateTime.Now;
                        menu.UpdateBy = Convert.ToInt32(Session["UserID"].ToString());
                        menu.Status = 2;//chưa xuất bản
                        menusDAO.Insert(menu);
                    }
                    TempData["message"] = new XMessage("success", "Thêm menu chủ đề bài viết thành công");
                }
                else
                {
                    TempData["message"] = new XMessage("danger", "Chưa chọn danh mục chủ đề bài viết");
                }
            }
            //Them trang don
            if (!string.IsNullOrEmpty(form["ThemPage"]))
            {
                if (!string.IsNullOrEmpty(form["namePage"]))//check box được nhấn tu phia Index
                {
                    var listitem = form["namePage"];
                    //chuyen danh sach thanh dang mang: vi du 1,2,3,...
                    var listarr = listitem.Split(',');//cat theo dau ,
                    foreach (var row in listarr)//row = id cua các mau tin
                    {
                        int id = int.Parse(row);//ep kieu int
                        Posts post = postsDAO.getRow(id);
                        //tao ra menu
                        Menus menu = new Menus();
                        menu.Name = post.Title;
                        menu.Link = post.Slug;
                        menu.TableID = post.Id;
                        menu.TypeMenu = "page";
                        menu.Position = form["Position"];
                        menu.ParentID = 0;
                        menu.Order = 0;
                        menu.CreateBy = Convert.ToInt32(Session["UserId"].ToString());
                        menu.CreateAt = DateTime.Now;
                        menu.UpdateAt = DateTime.Now;
                        menu.UpdateBy = Convert.ToInt32(Session["UserID"].ToString());
                        menu.Status = 2;//chưa xuất bản
                        menusDAO.Insert(menu);
                    }
                    TempData["message"] = new XMessage("success", "Thêm menu bài viết thành công");
                }
                else//check box chưa được nhấn
                {
                    TempData["message"] = new XMessage("danger", "Chưa chọn danh mục trang đơn");
                }
            }
            //Them custom
            if (!string.IsNullOrEmpty(form["ThemCustom"]))
            {
                //Kiem tra dau check cua muc con
                if (!string.IsNullOrEmpty(form["nameCustom"]) && !string.IsNullOrEmpty(form["linkCustom"]))
                {
                    //Tao ra menu custom
                    Menus menu = new Menus();
                    menu.Name = form["nameCustom"];
                    menu.Link = form["linkCustom"];
                    menu.TypeMenu = "custom";
                    menu.Position = form["Position"];
                    menu.ParentID = 0;
                    menu.Order = 0;
                    menu.CreateAt = DateTime.Now;
                    menu.CreateBy = Convert.ToInt32(Session["UserID"].ToString());
                    menu.UpdateAt = DateTime.Now;
                    menu.UpdateBy = Convert.ToInt32(Session["UserID"].ToString());
                    menu.Status = 2; //tam thoi chua xuat ban
                                     //Them vao DB
                    menusDAO.Insert(menu);

                    TempData["message"] = new XMessage("success", "Thêm vào menu thành công");
                }
                else
                {
                    TempData["message"] = new XMessage("danger", "Chưa đầy đủ thông tin cho menu");
                }
            }
            //Tra ve trang Index
            return RedirectToAction("Index", "Menu");
        }

        ////////////////////////////////////////////////////////////////
        // GET: Admin/Menu/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                //Hien thi thong bao
                TempData["message"] = new XMessage("danger", "Không tìm thấy menu");
                //Chuyen huong trang
                return RedirectToAction("Index", "Menu");
            }
            Menus menus = menusDAO.getRow(id);
            if (menus == null)
            {
                //Hien thi thong bao
                TempData["message"] = new XMessage("danger", "Không tìm thấy menu");
                //Chuyen huong trang
                return RedirectToAction("Index", "Menu");
            }
            //Hien thi ten cap cha
            if (menusDAO.getRow(menus.ParentID) == null)
            {
                ViewBag.ParentMenu = "Cấp 0";
            }
            else
            {
                ViewBag.ParentMenu = menusDAO.getRow(menus.ParentID).Name;
            }
            return View(menus);
        }

        ////////////////////////////////////////////////////////////////
        // GET: Admin/Menu/Edit/5
        public ActionResult Edit(int? id)
        {
            ViewBag.ParentList = new SelectList(menusDAO.getList("Index"), "Id", "Name");
            ViewBag.OrderList = new SelectList(menusDAO.getList("Index"), "Order", "Name");
            if (id == null)
            {
                //Hien thi thong bao
                TempData["message"] = new XMessage("danger", "Cập nhật menu thất bại");
                //Chuyen huong trang
                return RedirectToAction("Index", "Menu");
            }
            Menus menus = menusDAO.getRow(id);
            if (menus == null)
            {
                //Hien thi thong bao
                TempData["message"] = new XMessage("danger", "Cập nhật menu thất bại");
                //Chuyen huong trang
                return RedirectToAction("Index", "Menu");
            }
            return View(menus);
        }

        // POST: Admin/Menu/Edit/5
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Menus menus)
        {
            if (ModelState.IsValid)
            {
                //Chinh sua tu dong cho cac truong sau:
                //ParentID
                if (menus.ParentID == null)
                {
                    menus.ParentID = 0;
                }
                //Order
                if (menus.Order == null)
                {
                    menus.Order = 1;
                }
                else
                {
                    menus.Order += 1;
                }
                //UpdateAt
                menus.UpdateAt = DateTime.Now;
                //UpdateBy
                menus.UpdateBy = Convert.ToInt32(Session["UserID"]);
                //Cap nhat du lieu DB
                menusDAO.Update(menus);
                //Hien thi thong bao
                TempData["message"] = new XMessage("success", "Cập nhật menu thành công");
                return RedirectToAction("Index");
            }
            ViewBag.ParentList = new SelectList(menusDAO.getList("Index"), "Id", "Name");
            ViewBag.OrderList = new SelectList(menusDAO.getList("Index"), "Order", "Name");
            return View(menus);
        }

        ////////////////////////////////////////////////////////////////
        // GET: Admin/Menu/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                //Hien thi thong bao
                TempData["message"] = new XMessage("danger", "Xóa Menu thất bại");
                //Chuyen huong trang
                return RedirectToAction("Trash", "Menu");
            }
            Menus menus = menusDAO.getRow(id);
            if (menus == null)
            {
                //Hien thi thong bao
                TempData["message"] = new XMessage("danger", "Xóa Menu thất bại");
                //Chuyen huong trang
                return RedirectToAction("Trash", "Menu");
            }
            //Hien thi ten cap cha
            if (menusDAO.getRow(menus.ParentID) == null)
            {
                ViewBag.ParentMenu = "Cấp 0";
            }
            else
            {
                ViewBag.ParentMenu = menusDAO.getRow(menus.ParentID).Name;
            }
            return View(menus);
        }

        // POST: Admin/Menu/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Menus menus = menusDAO.getRow(id);
            menusDAO.Delete(menus);
            //Hien thi thong bao
            TempData["message"] = new XMessage("success", "Xóa Menu thành công");
            return RedirectToAction("Trash");
        }

        ////////////////////////////////////////////////////////////////
        //GET: Admin/Menu/Status/5
        public ActionResult Status(int? id)
        {
            if (id == null)
            {
                //Hien thi thong bao
                TempData["message"] = new XMessage("danger", "Cập nhật trạng thái thất bại");
                return RedirectToAction("Index");
            }
            //Truy van dong co id = id yeu cau
            Menus menus = menusDAO.getRow(id);
            if (menus == null)
            {
                //Hien thi thong bao
                TempData["message"] = new XMessage("danger", "Cập nhật trạng thái thất bại");
                return RedirectToAction("Index");
            }
            else
            {
                //Chuyen doi trang thai cua Satus tu 1<->2
                menus.Status = (menus.Status == 1) ? 2 : 1;
                //Cap nhat gia tri UpdateAt
                menus.UpdateAt = DateTime.Now;
                //Cap nhat lai DB
                menusDAO.Update(menus);
                //Hien thi thong bao
                TempData["message"] = TempData["message"] = new XMessage("success", "Cập nhật trạng thái thành công");
                return RedirectToAction("Index");
            }
        }

        ////////////////////////////////////////////////////////////////
        // GET: Admin/Menu/DelTrash/5
        public ActionResult DelTrash(int? id)
        {
            //Khi nhap nut thay doi Status cho mot mau tin
            Menus menus = menusDAO.getRow(id);
            //Thay doi trang thai Status tu 1,2 thanh 0
            menus.Status = 0;
            //Cap nhat gia tri cho UpdateAt/By
            menus.UpdateBy = Convert.ToInt32(Session["UserId"].ToString());
            menus.UpdateAt = DateTime.Now;
            //Goi ham Update trong MenusDAO
            menusDAO.Update(menus);
            //Hien thi thong bao
            TempData["message"] = new XMessage("success", "Xóa Menu thành công");
            //Khi cap nhat xong thi chuyen ve Index
            return RedirectToAction("Index", "Menu");
        }

        ////////////////////////////////////////////////////////////////
        // GET: Admin/Menus/Trash/5
        public ActionResult Trash()
        {
            return View(menusDAO.getList("Trash"));
        }

        ////////////////////////////////////////////////////////////////
        // GET: Admin/Menu/Recover/5
        public ActionResult Undo(int? id)
        {
            if (id == null)
            {
                //Hien thi thong bao
                TempData["message"] = new XMessage("danger", "Phục hồi menu thất bại");
                //Chuyen huong trang
                return RedirectToAction("Trash");
            }
            //Khi nhap nut thay doi Status cho mot mau tin
            Menus menus = menusDAO.getRow(id);
            //Kiem tra id cua menus co ton tai?
            if (menus == null)
            {
                //Hien thi thong bao
                TempData["message"] = new XMessage("danger", "Phục hồi menu thất bại");
                //Chuyen huong trang
                return RedirectToAction("Trash");
            }
            //Thay doi trang thai Status = 2
            menus.Status = 2;
            //Cap nhat gia tri cho UpdateAt/By
            menus.UpdateBy = Convert.ToInt32(Session["UserId"].ToString());
            menus.UpdateAt = DateTime.Now;
            //Goi ham Update trong menusDAO
            menusDAO.Update(menus);
            //Thong bao thanh cong
            TempData["message"] = new XMessage("success", "Phục hồi menu thành công");
            //Khi cap nhat xong thi chuyen ve Trash de phuc hoi tiep
            return RedirectToAction("Trash");
        }
    }
}
