using eMalicaMVC.Models;
using eMalicaMVC.ViewModels;
using eMalicaMVC.ViewModels.Reports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eMalicaMVC.Controllers
{
    [Authorize(Roles = "admin")]
    public class ReportController : Controller
    {
        MyDBHandle db = new MyDBHandle();

        // GET: Report
        public ActionResult Index(string weekParam, string monthParam, string dayParam)
        {
            bool isWeekParam = false;
            bool isMonthParam = false;
            bool isDayParam = false;

            DateTime selectedFrom;
            DateTime selectedTo;
            DateTime selectedDate;

            if (!String.IsNullOrEmpty(dayParam))
            {
                selectedDate = DateTime.Parse(dayParam);
                selectedFrom = selectedDate;
                selectedTo = selectedDate;
                isDayParam = true;
            }
            else if (!String.IsNullOrEmpty(weekParam))
            {
                selectedDate = DateTime.Parse(weekParam);
                selectedFrom = selectedDate.AddDays(-(int)selectedDate.DayOfWeek + (int)DayOfWeek.Monday);
                selectedTo = selectedFrom.AddDays(7);
                isWeekParam = true;
            }
            else if (!String.IsNullOrEmpty(monthParam))
            {
                selectedDate = DateTime.Parse(monthParam);
                selectedFrom = selectedDate.AddDays(-selectedDate.Day + 1);
                selectedTo = selectedDate.AddDays(DateTime.DaysInMonth(selectedDate.Year, selectedDate.Month) - 1);
                isMonthParam = true;
            }
            else
            {
                selectedDate = DateTime.Now;
                selectedFrom = selectedDate.AddDays(-(int)selectedDate.DayOfWeek + (int)DayOfWeek.Monday);
                selectedTo = selectedFrom.AddDays(7);
                isWeekParam = true;
            }

            ReportViewModel m = new ReportViewModel();
            var weeks = db.GetWeeks(selectedDate, true);
            var months = db.GetMonths(selectedDate, true);
            m.Months = months;
            m.Weeks = weeks;

            if (isDayParam)
                m.SelectedDate = dayParam;
            else
                m.SelectedDate = "";

            if (!isWeekParam)
            {
                weeks.ForEach(w => w.Selected = false);
                weeks.First(i => i.Selected = true);
            }
            if (!isMonthParam)
            {
                months.ForEach(w => w.Selected = false);
                months.First(i => i.Selected = true);
            }

            return View(m);
        }

        [HttpPost]
        public ActionResult LoadData(string weekParam, string monthParam, string dayParam)
        {
            //var path = Path.Combine(Server.MapPath("~/Export/"), "test.txt");
            // System.IO.File.AppendAllText(path, "LoadData calles!");
            var dateParam1 = Request.Form["dateParam"];

            DateTime selectedFrom;
            DateTime selectedTo;
            DateTime selectedDate;

            if (!String.IsNullOrEmpty(dayParam))
            {
                selectedDate = DateTime.Parse(dayParam);
                selectedFrom = selectedDate;
                selectedTo = selectedDate;

            }
            else if (!String.IsNullOrEmpty(weekParam))
            {
                selectedDate = DateTime.Parse(weekParam);
                selectedFrom = selectedDate.AddDays(-(int)selectedDate.DayOfWeek + (int)DayOfWeek.Monday);
                selectedTo = selectedDate.AddDays(-(int)selectedDate.DayOfWeek + (int)DayOfWeek.Friday);

            }
            else if (!String.IsNullOrEmpty(monthParam))
            {
                selectedDate = DateTime.Parse(monthParam);
                selectedFrom = selectedDate.AddDays(-selectedDate.Day + 1);
                selectedTo = selectedDate.AddDays(DateTime.DaysInMonth(selectedDate.Year, selectedDate.Month) - 1);

            }
            else
            {
                selectedDate = DateTime.Now;
                selectedFrom = selectedDate.AddDays(-(int)selectedDate.DayOfWeek + (int)DayOfWeek.Monday);
                selectedTo = selectedDate.AddDays(-(int)selectedDate.DayOfWeek + (int)DayOfWeek.Friday);

            }

            var draw = Request.Form.GetValues("draw").FirstOrDefault();
            var start = Request.Form.GetValues("start").FirstOrDefault();
            var length = Request.Form.GetValues("length").FirstOrDefault();
            var sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
            var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();
            var search = Request.Form.GetValues("search[value]").FirstOrDefault();

            MyDBHandle db = new MyDBHandle();
            IEnumerable<MenuReportViewModel> myMenus = db.GetMenusForReport("", selectedFrom, selectedTo);

            if (!string.IsNullOrWhiteSpace(search))
            {
                // Apply search   
                myMenus = myMenus.Where(p => (p.Name != null && p.Name.ToString().ToLower().Contains(search.ToLower())) ||
                    (p.Date != null && p.Date.ToString("dd.MM.yyyy").Contains(search.ToLower())) ||
                    (p.Price.ToString() != null && p.Price.ToString().ToLower().Contains(search.ToLower()))
                 ).ToList();
            }

            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            
            //Paging     
            var data = myMenus.Skip(skip).Take(pageSize).ToList();

            int recordsTotal = 0;
            //total number of rows count     
            recordsTotal = myMenus.Count();

            return Json(new { draw, recordsFiltered = recordsTotal, recordsTotal, data });
        }

        [HttpGet]
        public ActionResult Details(int menuId)
        {
            Menu m = db.GetMenu(menuId);

            IEnumerable<MenuOrderDetail> orderDetails = db.GetMenuOrderDetails(menuId);

            MenuOrderDetails details = new MenuOrderDetails();
            details.Details = orderDetails;
            details.Menu = m;

            return PartialView(details);
         }
    }
}