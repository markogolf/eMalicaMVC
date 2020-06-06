using eMalicaMVC.Models;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eMalicaMVC.ViewModels
{
    public class MenuReportViewModel : Menu
    {
        public int NumOfOrders { get; set; }
        public List<MenuOrder> MenuOrders { get; set; }
    }
}