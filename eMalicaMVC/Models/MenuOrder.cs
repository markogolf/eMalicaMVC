using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace eMalicaMVC.Models
{
    public class MenuOrder
    {
        public int ID { get; set; }
        public string UserId { get; set; }
        public int MenuId { get; set; }
        public string Status { get; set; }
        public string Comment { get; set; }

        public Menu Menu { get; set; }
    }
}
