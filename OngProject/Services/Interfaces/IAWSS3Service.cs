using Amazon.S3.Model;
using Microsoft.AspNetCore.Http;

namespace OngProject.Services.Interfaces
{
    public interface IAWSS3Service
    {
        public PutObjectRequest PutRequest(IFormFile file);
        public PutObjectRequest PutObjectRequestImageBase64(string ImageBase64, string ImageName);
        public ListObjectsV2Request ListObjectV2(string prefix);
        public string GetUrlRequest(string ImageName);
    }
}
