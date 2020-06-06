using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eMalicaMVC.ViewModels
{
    public class ReportViewModel
    {
        public List<System.Web.Mvc.SelectListItem> Weeks = new List<System.Web.Mvc.SelectListItem>();
        public List<System.Web.Mvc.SelectListItem> Months = new List<System.Web.Mvc.SelectListItem>();
        public string SelectedDate;
        //List<MenuReportViewModel> menus { get; set; }
    }
}