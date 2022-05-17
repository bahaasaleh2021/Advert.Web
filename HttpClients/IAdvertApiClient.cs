using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Advert.Web.HttpClients.Models;
using AdvertApi.Models;

namespace Advert.Web.HttpClients
{
    public interface IAdvertApiClient
    {
        Task<AdvertResponseModel> Create(CreateAdvertModel model);

        Task<bool> Confirm(ConfirmAdvertRequestModel model);
    }
}
