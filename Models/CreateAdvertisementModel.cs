using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Advert.Web.Models
{
    public class CreateAdvertisementModel
    {
        [Required(ErrorMessage ="This filed is required")]
        public string Title { get; set; }

        public string Description { get; set; }

         [Required(ErrorMessage ="This filed is required")]
         [DataType(DataType.Currency)]
        public decimal Price { get; set; }
    }
}
