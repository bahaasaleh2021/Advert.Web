using Advert.Web.HttpClients.Models;
using Advert.Web.Models;
using AdvertApi.Models;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Advert.Web
{
    public class AutoMapperProfile:Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<CreateAdvertModel, AdvertModel>().ReverseMap();
            CreateMap<CreateAdvertModel, CreateAdvertisementModel>().ReverseMap();
            CreateMap<CreateAdvertResponseModel, AdvertResponseModel>().ReverseMap();
            CreateMap<ConfirmAdvertRequestModel, ConfirmAdvertModel>().ReverseMap();

        }
    }
}
