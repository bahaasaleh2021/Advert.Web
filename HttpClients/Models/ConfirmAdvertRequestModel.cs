using AdvertApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Advert.Web.HttpClients.Models
{
    public class ConfirmAdvertRequestModel
    {
        public string Id { get; set; }
        public AdvertStatus Status { get; set; }
    }
}
