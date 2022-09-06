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
        private readonly IAWSS3Service _awsS3Service;
        private readonly string BucketName = "cohorte-agosto-38d749a7";

        public SlideController(ISlideService slideService,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IAmazonS3 amazonS3,
            IAWSS3Service awsS3Service)
        {
            _slideService = slideService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _amazonS3 = amazonS3;
            _awsS3Service = awsS3Service;
        }


        [HttpPost]
        public async Task<IActionResult> CreateSlide(SlideCreateDTO newSlide)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var _slide = _mapper.Map<Slide>(newSlide);

            var putRequest = _awsS3Service.PutObjectRequestImageBase64(newSlide.ImageBase64, newSlide.ImageName);

            var result = await _amazonS3.PutObjectAsync(putRequest);

            if (result.HttpStatusCode.ToString() != "OK")
                return BadRequest();

            _slide.ImageUrl = newSlide.ImageName;


            var created = await _slideService.CreateAsync(_slide);

            if (created)
                _unitOfWork.Commit();

            return Created("Created", new { Response = StatusCode(201) });
        }

        [HttpGet]
        public async Task<IActionResult> Get(string imageName)
        {
            var response = await _amazonS3.GetObjectAsync(BucketName, imageName);
            return File(response.ResponseStream, response.Headers.ContentType);
        }

        // test para ver la urls de una lista de objetos
        // tiene time expire
        [HttpGet("GetUrls")]
        public async Task<IActionResult> GetList(string prefix)
        {
            var request = _awsS3Service.ListObjectV2(prefix);

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

        #region asignaron a otro compa la tarea
        // Dejo comentado porque no esta terminado y puede ser util y se puede reutilizar.

        //[HttpGet("{id}")]
        //[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SlideDTO))]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //public async Task<IActionResult> GetById(int id)
        //{
        //    var slide = await _slideService.GetById(id);

        //    if (slide == null)
        //        return NotFound();

        //    var response = await _amazonS3.GetObjectAsync(BucketName, slide.ImageUrl);

        //    var request = new GetPreSignedUrlRequest()
        //    {
        //        BucketName = BucketName,
        //        Key = response.Key,
        //        Expires = System.DateTime.UtcNow.AddSeconds(40)
        //    };
        //    var preSignedUrls = _amazonS3.GetPreSignedURL(request);


        //    var slideDTO = _mapper.Map<SlideDTO>(slide);

        //    return new OkObjectResult(slideDTO);
        //}
        #endregion

        #region IFormFile
        // Prueba con IFormFile OK
        // La tarea dice usar string Base64 - analizar que es mas conveniente
        // El string base64 es muy extenso.

        //[HttpPost("CreateIFormFile")]
        //public async Task<IActionResult> Create(IFormFile file)
        //{
        //    if (!ModelState.IsValid)
        //        return BadRequest();

        //    var putRequest = _awsS3Service.PutRequest(file);

        //    var result = await _amazonS3.PutObjectAsync(putRequest);
        //    return Ok(result);
        //}
        #endregion

    }
}
