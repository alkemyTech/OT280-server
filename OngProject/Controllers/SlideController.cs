using Amazon.S3;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using OngProject.Core.Models;
using OngProject.Repositories.Interfaces;
using OngProject.Services.Interfaces;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Amazon.S3.Model;
using System.IO;
using System.Linq;
using OngProject.Core.Models.DTOs.Slide;
using System;
using System.Drawing;


namespace OngProject.Controllers
{
    //[Authorize(Roles = "Administrator")]
    [Route("[controller]")]
    [ApiController]
    public class SlideController : Controller
    {
        private readonly ISlideService _slideService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAmazonS3 _amazonS3;

        public SlideController(ISlideService slideService,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IAmazonS3 amazonS3)
        {
            _slideService = slideService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _amazonS3 = amazonS3;
        }

        public string BucketName = "cohorte-agosto-38d749a7";

        // test para ver la urls
        [HttpGet("testUrls")]
        public async Task<IActionResult> GetList(string prefix)
        {
            var request = new ListObjectsV2Request()
            {
                BucketName = BucketName,
                Prefix = prefix
            };
            var response = await _amazonS3.ListObjectsV2Async(request);
            var preSignedUrls = response.S3Objects.Select(o =>
            {
                var request = new GetPreSignedUrlRequest()
                {
                    BucketName = BucketName,
                    Key = o.Key,
                    Expires = System.DateTime.UtcNow.AddSeconds(30)
                };
                return _amazonS3.GetPreSignedURL(request);
            });

            return Ok(preSignedUrls);
        }

        [HttpGet]
        public async Task<IActionResult> Get(string imageName)
        {
            var response = await _amazonS3.GetObjectAsync(BucketName, imageName);
            return File(response.ResponseStream, response.Headers.ContentType);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SlideDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var slide = await _slideService.GetById(id);

            if (slide == null)
                return NotFound();

            var response = await _amazonS3.GetObjectAsync(BucketName, slide.ImageUrl);

            var request = new GetPreSignedUrlRequest()
            {
                BucketName = BucketName,
                Key = response.Key,
                Expires = System.DateTime.UtcNow.AddSeconds(40)
            };
            var preSignedUrls = _amazonS3.GetPreSignedURL(request);


            var slideDTO = _mapper.Map<SlideDTO>(slide);

            return new OkObjectResult(slideDTO);
        }

        [HttpPost("CreateIFormFile")]
        public async Task<IActionResult> Create(IFormFile file)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var putRequest = new PutObjectRequest()
            {
                BucketName = BucketName,
                Key = file.FileName,
                InputStream = file.OpenReadStream(),
                ContentType = file.ContentType,
            };

            var result = await _amazonS3.PutObjectAsync(putRequest);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateSlide(SlideCreateDTO newSlide)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var _slide = _mapper.Map<Slide>(newSlide);

            //byte[] bytes = Convert.FromBase64String(_slide.ImageUrl);
            //using (MemoryStream ms = new(bytes))
            //{
            //    Image pic = Image.FromStream(ms);
            //}
            //var ms = new MemoryStream(bytes);
            //var pic = Image.FromStream(ms);

            //byte[] bytes = Convert.FromBase64String(newSlide.ImageBase64);
            //using (MemoryStream stream = new(bytes))
            //{
            //    var file = new FormFile(stream, 0, stream.Length, null, newSlide.ImageName)
            //    {
            //        Headers = new HeaderDictionary(),
            //        ContentType = "application/png"
            //    };
            //}

            byte[] bytes = Convert.FromBase64String(newSlide.ImageBase64);
            MemoryStream stream = new MemoryStream(bytes);

            IFormFile file;
            file = new FormFile(stream, 0, bytes.Length, newSlide.ImageName, newSlide.ImageName)
            {
                Headers = new HeaderDictionary(),
                ContentType = "image/png"
            };


            var putRequest = new PutObjectRequest()
            {
                BucketName = BucketName,
                Key = file.FileName,
                InputStream = file.OpenReadStream(),
                ContentType = file.ContentType,
            };

            var result = await _amazonS3.PutObjectAsync(putRequest);
            //return Ok(result);
            if (result.HttpStatusCode.ToString() != "Ok")
                return BadRequest();


            var created = await _slideService.CreateAsync(_slide);

            if (created)
                _unitOfWork.Commit();

            return Created("Created", new { Response = StatusCode(201) });

        }
        //[HttpPost]
        //public async Task<IActionResult> Create(CategoryDTO categoryDTO)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest();
        //    }

        //    var _category = _mapper.Map<Categories>(categoryDTO);
        //    var created = await _categoryService.CreateAsync(_category);

        //    if (created)
        //        _unitOfWork.Commit();

        //    return Created("Created", new { Response = StatusCode(201) });
        //}

    }
}
