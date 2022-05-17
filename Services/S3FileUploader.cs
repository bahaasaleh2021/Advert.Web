using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Advert.Web.Services
{
    public class S3FileUploader : IFileUploader
    {
        private readonly IConfiguration _config;

        public S3FileUploader(IConfiguration config)
        {
            _config = config;
        }
        public async Task<bool> UploadFileAsync(string fileName, Stream storageStream)
        {
            if (string.IsNullOrWhiteSpace(fileName))
                throw new ArgumentException(message: "File name is required");

            var bucketName = _config["ImageS3Bucket"];

            using (var s3Client = new AmazonS3Client(new StoredProfileAWSCredentials()
                ,   RegionEndpoint.USEast2
                ))
            {
                if (storageStream.Length > 0)
                {
                    if (storageStream.CanSeek)
                        storageStream.Seek(offset: 0, origin: SeekOrigin.Begin);

                    var request = new PutObjectRequest
                    {
                        AutoCloseStream = true,
                        InputStream = storageStream,
                        Key = fileName,
                        BucketName = bucketName
                    };

                    var response = await s3Client.PutObjectAsync(request);
                    return response.HttpStatusCode == System.Net.HttpStatusCode.OK;
                }
            }

            return true;
        }
    }
}
