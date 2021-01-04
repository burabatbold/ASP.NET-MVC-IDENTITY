using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication6.Models;

namespace WebApplication6.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        AdventureWorksLT2019Entities dt = new AdventureWorksLT2019Entities();

        public ActionResult Index(int page = 1)
        {
            int limit = 10;
            int totalData = dt.Products.Count();
            int totalPage = totalData / limit;
            int start = (page - 1) * limit + 1;
            int end = start + limit - 1;
            ViewBag.page = page;
            ViewBag.totalPage = totalPage;
            if (page == totalPage || page > totalPage)
            {
                this.ViewBag.limitPage = page;
            }
            else { this.ViewBag.limitPage = page + 2; }

            var data = dt.Products.OrderBy(el => el.ProductID).Skip(start - 1).Take(limit).ToList();
            return View(data);
        }
        

     
        public ActionResult Edit(int id)
        {
            var data = dt.Products.Where(el => el.ProductID == id).FirstOrDefault();
            var categoryList = dt.ProductCategories.ToList();
            var modelList = dt.ProductModels.ToList();
            ViewBag.ProductCategoryList = new SelectList(categoryList, "ProductCategoryID", "Name");
            ViewBag.ProductModelID = new SelectList(modelList, "ProductModelID", "Name");
            return View(data);
        }

        [HttpPost]
        public ActionResult Edit(Product data)
        {
            var v = dt.Products.Where(el => el.ProductID == data.ProductID).FirstOrDefault();
            if (v != null)
            {
                v.Name = data.Name;
                v.ProductNumber = data.ProductNumber;
                v.Color = data.Color;
                v.StandardCost = data.StandardCost;
                v.ListPrice = data.ListPrice;
                v.Size = data.Size;
                v.Weight = data.Weight;
                v.ProductCategoryID = data.ProductCategoryID;
                v.ProductModelID = data.ProductModelID;
                v.SellStartDate = data.SellStartDate;
                v.SellEndDate = data.SellEndDate;
                v.DiscontinuedDate = data.DiscontinuedDate;
                v.ThumbNailPhoto = data.ThumbNailPhoto;
                v.ThumbnailPhotoFileName = data.ThumbnailPhotoFileName;
                v.rowguid = data.rowguid;
                v.ModifiedDate = data.ModifiedDate;
                dt.SaveChanges();
            }
            return RedirectToAction("Index");

        }

        [HttpGet]
        public ActionResult Create()
        {
            var categoryList = dt.ProductCategories.ToList();
            var modelList = dt.ProductModels.ToList();
            ViewBag.ProductCategoryID = new SelectList(categoryList, "ProductCategoryID", "Name");
            ViewBag.ProductModelID = new SelectList(modelList, "ProductModelID", "Name");
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpPost]
        public ActionResult Create(Product data)
        {
            var list = dt.Products.Where(el => el.rowguid == data.rowguid).ToList();
            if (list == null)
            {
                dt.Products.Add(data);
                dt.SaveChanges();
            }
            else
            {
                ViewBag.Error = "Давхардаж байна !";
            }

            return RedirectToAction("Index");

        }

        public ActionResult Delete(int id)
        {
            var deleteing = dt.Products.Where(el => el.ProductID == id).FirstOrDefault();
            dt.Products.Remove(deleteing);
            dt.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}