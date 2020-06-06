using eMalicaMVC.Common;
using eMalicaMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eMalicaMVC.ViewModels
{
    public class SubViewModel : Menu
    {
        public List<System.Web.Mvc.SelectListItem> Weeks = new List<System.Web.Mvc.SelectListItem>();
        public IEnumerable<System.Web.Mvc.SelectListItem> Statuses {
            get {
                return Utils.GetExtraStatusList(MenuOrder?.Status);
            }
        }
        public MenuOrder MenuOrder { get; set; }
        public bool IsDisabled { get; set; }
        public string UserId;
    }
}