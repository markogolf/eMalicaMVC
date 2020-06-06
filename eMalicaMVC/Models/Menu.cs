using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace eMalicaMVC.Models
{
    public class Menu
    {
        public int Id { get; set; }

        [Display(Name = "Datum")]
        [Required(ErrorMessage = "Vnesite pravilen datum")]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }

        public int SortId { get; set; }

        [Display(Name = "Malica")]
        [Required]
        public string Name { get; set; }

        [Display(Name = "Cena")]
        [Required]
        [Range(1, 999)]
        public decimal Price { get; set; }


    }
}
