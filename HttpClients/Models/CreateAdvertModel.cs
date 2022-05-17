using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Advert.Web.HttpClients.Models
{
    public class CreateAdvertModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
    }
}
