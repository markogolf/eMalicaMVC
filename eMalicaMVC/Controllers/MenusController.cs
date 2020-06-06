
using eMalicaMVC.Common;
using eMalicaMVC.Models;
using eMalicaMVC.ViewModels;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eMalicaMVC.Controllers
{
    [Authorize]
    public class MenusController : Controller
    {
        MyDBHandle db = new MyDBHandle();

        //public ActionResult Admin(string userId, string dateParam)
        //{
        //    return LoadMyMenus(userId, dateParam);
        //}
        
        
        //public ActionResult Index(string dateParam)
        //{
        //    string userId = User.Identity.GetUserId();
        //    return LoadMyMenus(userId, dateParam);
        //}

        public ActionResult Index(string userId, string dateParam)
        {
            string user;

            if (!string.IsNullOrWhiteSpace(userId) && User.IsInRole("admin"))
            {
                user = userId;
            }
            else
            {
                user = User.Identity.GetUserId();
            }

            MainViewModel menuMainViewModel = new MainViewModel(user);
            
            DateTime selectedFrom;
            DateTime selectedTo;
            DateTime selectedDate;

            if (String.IsNullOrEmpty(dateParam))
            {
                selectedDate = DateTime.Now;
            }
            else
            {
                selectedDate = DateTime.Parse(dateParam);
            }
            selectedFrom = selectedDate.AddDays(-(int)selectedDate.DayOfWeek + (int)DayOfWeek.Monday);
            selectedTo = selectedDate.AddDays(-(int)selectedDate.DayOfWeek + (int)DayOfWeek.Friday);


            IEnumerable<SubViewModel> menus = db.GetMenus(user, selectedFrom, selectedTo);
            menuMainViewModel.Weeks = db.GetWeeks(selectedDate, false);
            menuMainViewModel.Users = db.GetUsers(user);
            menuMainViewModel.Menus = menus;

            foreach (var oneM in menus)
            {
                bool res = oneM.IsDisabled = menus.Any(m => m.Date == oneM.Date && m.MenuOrder != null && m.MenuOrder.Status != Enums.OrderStatus.Cancelled);
                oneM.IsDisabled = res;
            }

            return View(menuMainViewModel);
        }

        [HttpPost, Authorize(Roles = "admin")]
        public ActionResult SetOrderStatus(int orderId, string status)
        {
            var order = db.GetOrder(orderId);

            db.SetOrderStatus(orderId, status);

            return RedirectToAction("Index", new { userId = order.UserId });
        }
      
        [HttpPost]
        public ActionResult Order(int id, string comment, string userId, string dateParam)
        {
            string user;

            if (!string.IsNullOrWhiteSpace(userId) && User.IsInRole("admin"))
            {
                user = userId;
            }
            else {
                user = User.Identity.GetUserId();
            }

            var menuOrder = db.GetUserMenuOrder(id, userId);

            if (menuOrder == null)
            {
                db.CreateOrder(id, comment, user);
            }
            else 
            {
                db.SetOrderStatus(menuOrder.ID, Enums.OrderStatus.Ordered, comment);
            }

        ViewBag.Message = "Your application description page.";

            return RedirectToAction("Index", new { userId, dateParam });
        }

        public ActionResult CreateOrEdit(int id = 0)
        {
            Menu menu;
            if (id == 0)//create
            {
                menu = new Menu();
                menu.Date = DateTime.Now;
                menu.Price = 5;
            }
            else
            {
                menu = db.GetMenu(id);
            }

            return View(menu);
        }

        [HttpPost]
        public ActionResult CreateOrEdit(Menu menu, int id = 0)
        {
            if (ModelState.IsValid)
            {
                if (id == 0)//create
                {
                    db.AddMenu(menu);
                }
                else
                {
                    db.UpdateMenu(menu);
                }
                return RedirectToAction("Index", new { dateParam = menu.Date.ToString("dd-MM-yyyy")});
            }

            return View(menu);
        }

        // POST: Home/Create
        [HttpGet]
        public ActionResult Cancel(int id, string userId, string dateParam)
        {
            string user;
            if (!string.IsNullOrWhiteSpace(userId) && User.IsInRole("admin"))
            {
                user = userId;
            }
            else {
                user = User.Identity.GetUserId();
            }

            db.CancelOrder(id);
            return RedirectToAction("Index", new { userId = user, dateParam});
        }



        [HttpGet]
        public ActionResult Edit(int id, string userId)
        {
            Menu m = db.GetMenu(id);
            return RedirectToAction("CreateOrEdit", new { m });
        }

        [HttpPost]
        public bool Delete(int id, string userId)
        {
            bool ok = false;
            if (User.IsInRole("admin"))
            {
                ok = db.DeleteMenu(id);
            }
            return ok;
        }
        //// POST: Home/Create
        //[HttpPost]
        //public ActionResult Create(Menu menu)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            if (menu.Id == 0)
        //            {
        //                db.AddMenu(menu);
        //            }
        //            else
        //            {
        //                db.UpdateMenu(menu);
        //            }

        //            return RedirectToAction("Index");
        //        }
        //        catch
        //        {
        //            return View();
        //        }
        //    }
        //    else
        //    {
        //        return View(menu);
        //    }
        //}
    }
}