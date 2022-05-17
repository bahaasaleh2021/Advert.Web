using Advert.Web.HttpClients.Models;
using AdvertApi.Models;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Advert.Web.HttpClients
{
    public class AdvertApiclient : IAdvertApiClient
    {
        private readonly IConfiguration _config;
        private readonly HttpClient _client;
        private readonly IMapper _mapper;
        public AdvertApiclient(IConfiguration config,HttpClient client,IMapper mapper)
        {
           _config = config;
           _client = client;
           _mapper = mapper;

            _client.BaseAddress=new Uri( _config["AdvertApi:baseUrl"]);
           // _client.DefaultRequestHeaders.Add("content-type", "application/json");

        }

        public IMapper Mapper { get; }

       

        public async Task<AdvertResponseModel> Create(CreateAdvertModel model)
        {
            var advertModel = _mapper.Map<AdvertModel>(model);
            var advertJson = JsonSerializer.Serialize(advertModel);

            var jsonResponse = await _client.PostAsync(_client.BaseAddress+"create", new StringContent(advertJson));
            var jsonString = await jsonResponse.Content.ReadAsStringAsync();
            var advertResponse =  JsonSerializer.Deserialize<CreateAdvertResponseModel>(jsonString);
            var response = _mapper.Map<AdvertResponseModel>(advertResponse);

            return response;
        }

        public async Task<bool> Confirm(ConfirmAdvertRequestModel model)
        {
            var confirmModel = _mapper.Map<ConfirmAdvertModel>(model);
            var confirmJson = JsonSerializer.Serialize(confirmModel);

            var response = await _client.PutAsync(_client.BaseAddress+"confirm", new StringContent(confirmJson));

            return response.StatusCode == System.Net.HttpStatusCode.OK;
        }
    }
}
