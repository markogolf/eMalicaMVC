using eMalicaMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eMalicaMVC.ViewModels.Reports
{
    public class MenuOrderDetails
    {
        public Menu Menu { get; set; }
        public IEnumerable<MenuOrderDetail> Details { get; set; }
    }
}