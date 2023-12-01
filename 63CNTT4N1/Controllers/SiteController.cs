using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyClass.DAO;
using MyClass.Model;

namespace _63CNTT4N1.Controllers
{
    public class SiteController : Controller
    {
        LinksDAO linksDAO = new LinksDAO();
      
        // GET: Site
        public ActionResult Index(string slug = null)
        {
            if (slug == null)
            {
                //Chuyen ve trang chu
                return RedirectToAction("Home");
            }
            else
            {
                //Tim slug co trong bang link
                Links links = linksDAO.getRow(slug);
                //Kiem tra slug co ton tai trong bang Links hay khong
                if (links != null)
                {
                    //Lay ra Type trong bang link
                    string typelink = links.Type;
                    switch (typelink)
                    {
                        case "category":
                            {
                                return this.ProductCategory(slug);
                            }
                        case "topic":
                            {
                                return this.PostTopic();
                            }
                        case "page":
                            {
                                return this.PostPage(slug);
                            }
                        default:
                            {
                                return this.Error404();
                            }
                    }
                }
                else
                {
                    //Slug khong co trong bang Links
                    //Slug co trong bang product?
                    //Slug co trong bang Post voi PostType==post?
                    ProductsDAO productsDAO = new ProductsDAO();
                    PostsDAO postsDAO = new PostsDAO();
                    //Tim slug co trong bang Products
                    Products products = productsDAO.getRow(slug);
                    if (products != null)
                    {
                        return this.ProductDetail(slug);
                    }
                    else
                    {
                        Posts posts = postsDAO.getRow(slug);
                        if (posts != null)
                        {
                            return this.PostDetail();
                        }
                        else
                        {
                            return this.Error404();
                        }
                    }
                }
            }
        }

        //Trang chu
        public ActionResult Home()
        {
            CategoriesDAO categoriesDAO = new CategoriesDAO();
            List<Categories> list = categoriesDAO.getListByPareantId();
            return View("Home", list);
        }

        //Site/Product
        public ActionResult Product()
        {
            ProductsDAO productsDAO = new ProductsDAO();
            List<ProductInfo> list = productsDAO.getListBylimit(8);
            return View("Product", list);
        }

        //Site/Post
        public ActionResult Post()
        {
            return View("Post");
        }

        //Site/ProductCategory
        public ActionResult ProductCategory(string slug)
        {
            //Lay categories dua vao Slug
            CategoriesDAO categoriesDAO = new CategoriesDAO();
            Categories categories = categoriesDAO.getRow(slug);       
            //Hien thi noi dung cua mau tin
            ViewBag.Categories = categories;
            //Hien thi toan bo cac shan pham ung voi tung loai
            //Hien thi theo 3 cap: cha - con - con cua con
            //Cap 1
            List<int> listcatid = new List<int>() { categories.Id };
            //Cap 2
            List<Categories> listcategories2 = categoriesDAO.getListByPareantId(categories.Id);
            if (listcategories2.Count() != 0)
            {
                foreach (var categories2 in listcategories2)
                {
                    listcatid.Add(categories2.Id);
                    //cap 3
                    List<Categories> listcategories3 = categoriesDAO.getListByPareantId(categories2.Id);
                    if (listcategories3.Count() != 0)
                    {
                        foreach (var categories3 in listcategories3)
                        {
                            listcatid.Add(categories3.Id);
                        }
                    }
                }
            }
            ProductsDAO productsDAO = new ProductsDAO();
            List<ProductInfo> list = productsDAO.getListByListCatId(listcatid, 10);
            return View("ProductCategory",list);
        }

        //Site/PostTopic
        public ActionResult PostTopic()
        {
            return View("PostTopic");
        }

        //Site/PostPage
        public ActionResult PostPage(string slug)
        {
            PostsDAO postsDAO = new PostsDAO();
            Posts posts = postsDAO.getRow(slug);
            return View("PostPage", posts);
        }

        //Site/Error404
        public ActionResult Error404()
        {
            return View("Error404");
        }

        //Product/Details
        public ActionResult ProductDetail(string slug)
        {
            //Hien thi noi dung cua san pham
            ProductsDAO productsDAO = new ProductsDAO();
            List<ProductInfo> list = productsDAO.GetProductDetailBySlug(slug);
            //Lay CatID cua san pham hien tai
            int catid = list.First().CatID;
            //Lay danh sach san pham cung loai (related products)
            List<ProductInfo> relatedProducts = productsDAO.GetProductDetailByCategoryId(catid);
            //Truyen danh sach san pham cung loai cho View
            ViewBag.RelatedProducts = relatedProducts;
            //Tra ve danh sach cho tiet san pham cho list
            return View("ProductDetail", list);
        }

        //Post/Details
        public ActionResult PostDetail()
        {
            return View("PostDetail");
        }

        //HomeProduct
        public ActionResult HomeProduct(int id)
        {
            CategoriesDAO categoriesDAO = new CategoriesDAO();
            Categories categories = categoriesDAO.getRow(id);
            ViewBag.Categories = categories;
            //Hien thi toan bo cac shan pham ung voi tung loai
            //Hien thi theo 3 cap: cha - con - con cua con
            //Cap 1
            List<int> listcatid = new List<int>() { id }; 
            //Cap 2
            List<Categories> listcategories2 = categoriesDAO.getListByPareantId(id);
            if (listcategories2.Count() != 0)
            {
                foreach (var categories2 in listcategories2)
                {
                    listcatid.Add(categories2.Id);
                    //Cap 3
                    List<Categories> listcategories3 = categoriesDAO.getListByPareantId(categories2.Id);
                    if (listcategories3.Count() != 0)
                    {
                        foreach (var categories3 in listcategories3)
                        {
                            listcatid.Add(categories3.Id);
                        }
                    }
                }
            }
            ProductsDAO productsDAO = new ProductsDAO();
            List<ProductInfo> list = productsDAO.getListByListCatId(listcatid, 10);
            return View("HomeProduct", list);
        }
    }
}