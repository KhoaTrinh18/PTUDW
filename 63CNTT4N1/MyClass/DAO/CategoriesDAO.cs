using MyClass.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyClass.DAO
{
    public class CategoriesDAO
    {
        private MyDBContext db = new MyDBContext();

        //Index
        public List<Categories> getList()
        {
            return db.Categories.ToList();
        }

        public List<Categories> getListByPareantId(int parentid = 0)
        {
            return db.Categories
                .Where(m => m.ParentId == parentid && m.Status == 1)
                .OrderBy(m => m.Order)
                .ToList();
        }

        //Index chi hien thi cac mau tin co status = 1,2, con Trash chi hien thi cac mau tin co status = 0
        public List<Categories> getList(string status = "All")
        {
            List<Categories> list = null;
            switch (status)
            {
                case "Index":
                    {
                        list = db.Categories
                            .Where(m => m.Status != 0)
                            .ToList();
                        break;
                    }
                case "Trash":
                    {
                        list = db.Categories
                            .Where(m => m.Status == 0)
                            .ToList();
                        break;
                    }
                default:
                    {
                        list = db.Categories.ToList();
                        break;
                    }
            }
            return list;
        }

        //Details
        public Categories getRow(int? id)
        {
            if (id == null)
            {
                return null;
            }
            else
            {
                return db.Categories.Find(id);
            }
        }

        //Hien thi danh sach 1 mau tin (ban ghi) voi kieu string = slug
        public Categories getRow(string slug)
        {
            return db.Categories
                .Where(m => m.Slug == slug && m.Status == 1)
                .FirstOrDefault();
        }

        //Create
        public int Insert(Categories row)
        {
            db.Categories.Add(row);
            return db.SaveChanges();
        }

        //Update
        public int Update(Categories row)
        {
            db.Entry(row).State = EntityState.Modified;
            return db.SaveChanges();
        }

        //Delete
        public int Delete(Categories row)
        {
            db.Categories.Remove(row);
            return db.SaveChanges();
        }
    }
}