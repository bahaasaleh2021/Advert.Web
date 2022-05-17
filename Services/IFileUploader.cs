using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Advert.Web.Services
{
    public interface IFileUploader
    {
        Task<bool> UploadFileAsync(string name, Stream storageStream);
    }
}
