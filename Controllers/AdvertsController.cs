using Advert.Web.HttpClients;
using Advert.Web.HttpClients.Models;
using Advert.Web.Models;
using Advert.Web.Services;
using AdvertApi.Models;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Advert.Web.Controllers
{
    public class AdvertsController : Controller
    {
        private readonly IFileUploader _uploader;
        private readonly IAdvertApiClient _advertApiClient;
        private readonly IMapper _mapper;

        public AdvertsController(IFileUploader uploader,IAdvertApiClient advertApiClient,IMapper mapper)
        {
            _uploader = uploader;
            _advertApiClient = advertApiClient;
            _mapper = mapper;
        }


        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateAdvertisementModel model,IFormFile imageFile)
        {
            if (!ModelState.IsValid)
                return View();

            var createAdvertModel = _mapper.Map<CreateAdvertModel>(model);
            var response = await _advertApiClient.Create(createAdvertModel);
            var id = response.Id;

            string fileName = "";
            if (imageFile != null)
            {
                fileName = string.IsNullOrWhiteSpace(fileName) ? id : Path.GetFileName(imageFile.FileName);
                var filePath = $"{id}/{fileName}";

                try
                {
                    using (var stream=imageFile.OpenReadStream())
                    {
                        var res = await _uploader.UploadFileAsync(fileName, stream);
                        if (!res)
                            throw new Exception(message: "could not upload image file to storage ,see error details");

                       

                    }

                    //confirm advert after photo uploaded successfully
                    var confirmed = await _advertApiClient.Confirm(new ConfirmAdvertRequestModel
                    {
                        Id = id,
                        Status = AdvertStatus.Active
                    });

                    if (!confirmed)
                        throw new Exception(message: $"advert with id : ${id} failed to confirm!");

                     return RedirectToAction("Index", "Home");
                }
                catch(Exception ex)
                {
                    //call advert api to cancel the advert
                    await _advertApiClient.Confirm(new ConfirmAdvertRequestModel
                    {
                        Id = id,
                        Status = AdvertStatus.Pending
                    });

                    Console.WriteLine(ex);
                }
            }

           

            ModelState.AddModelError("Title", "You must upload image");
            return View(model);

        }
    }
}
