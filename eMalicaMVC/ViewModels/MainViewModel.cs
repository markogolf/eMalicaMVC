using eMalicaMVC.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eMalicaMVC.ViewModels
{
    public class MainViewModel
    {
        public string UserId;
        public List<System.Web.Mvc.SelectListItem> Weeks = new List<System.Web.Mvc.SelectListItem>();
        public List<System.Web.Mvc.SelectListItem> Users = new List<System.Web.Mvc.SelectListItem>();

        //public DayOfWeekViewModel Monday;
        //public DayOfWeekViewModel Tuesday;
        //public DayOfWeekViewModel Wednesday;
        //public DayOfWeekViewModel Thursday;
        //public DayOfWeekViewModel Friday;

        // public bool Disabled => Menus.Any(m => m.MenuOrder != null && m.MenuOrder.Status != Enums.OrderStatus.Cancelled);
        public IEnumerable<SubViewModel> Menus { get; set; }

        public MainViewModel(string userId)
        {
            UserId = userId;
            //Monday = new DayOfWeekViewModel(userId);
            //Tuesday = new DayOfWeekViewModel(userId);
            //Wednesday = new DayOfWeekViewModel(userId);
            //Thursday = new DayOfWeekViewModel(userId);
            //Friday = new DayOfWeekViewModel(userId);
        }

        //public bool IsDayDisabled(DateTime date)
        //{
        //    bool res = Menus.Any(m => m.Date == date && m.MenuOrder != null && m.MenuOrder.Status != Enums.OrderStatus.Cancelled);
        //    return true;
        //}
    }
}