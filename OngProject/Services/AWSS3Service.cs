using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.AspNetCore.Http;
using OngProject.Repositories.Interfaces;
using OngProject.Services.Interfaces;
using System.IO;
using System;

namespace OngProject.Services
{
    public class AWSS3Service : IAWSS3Service
    {
        private readonly string BucketName = "cohorte-agosto-38d749a7";

        private readonly IUnitOfWork _unitOfWork;

        public AWSS3Service(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public PutObjectRequest PutRequest(IFormFile file)
        {
            return new PutObjectRequest()
            {
                BucketName = BucketName,
                Key = file.FileName,
                InputStream = file.OpenReadStream(),
                ContentType = file.ContentType,
            };
        }

        public ListObjectsV2Request ListObjectV2(string prefix)
        {
            return new ListObjectsV2Request()
            {
                BucketName = BucketName,
                Prefix = prefix
            };
        }

        public PutObjectRequest PutObjectRequestImageBase64(string ImageBase64, string ImageName)
        {
            byte[] bytes = Convert.FromBase64String(ImageBase64);
            MemoryStream stream = new MemoryStream(bytes);

            var contentType = "image/jpg";
            string[] split = ImageName.Split(".");
            
            switch (split[1].ToLower())
            {
                case "jpg":
                    contentType = "image/jpg";
                    break;
                case "png":
                    contentType = "image/png";
                    break;
                default:
                    break;
            }
            
            IFormFile file;
            file = new FormFile(stream, 0, bytes.Length, ImageName, "slides/" +ImageName)
            {
                Headers = new HeaderDictionary(),
                ContentType = contentType
            };

            return PutRequest(file);
        }

        public string GetUrlRequest(string ImageName)
        {
            // Create a client
            AmazonS3Client client = new();

            // Create a CopyObject request
            GetPreSignedUrlRequest request = new()
            {
                BucketName = BucketName,
                Key = ImageName,
                Expires = DateTime.Now.AddMonths(2)
            };

            // Get path for request
            return  client.GetPreSignedURL(request);

        }


    }
}
