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
using OngProject.Services;
using Microsoft.IdentityModel.Tokens;
using EllipticCurve.Utils;
using Microsoft.EntityFrameworkCore;
using OngProject.Core.Models.DTOs;
using System.Collections.Generic;
using System.Xml.Linq;
using Microsoft.AspNetCore.Authorization;

namespace OngProject.Controllers
{
    [Authorize(Roles = "admin")]
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

        /// <summary>
        /// Crear Slide con imagen en base64
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /slide
        ///     {
        ///         "imageBase64": "stringBase64",
        ///         "imageName": "Juguetes1.jpg",
        ///         "text": "string",
        ///         "organization": {
        ///           "name": "string",
        ///           "image": "string",
        ///           "address": "string",
        ///           "phone": 1234,
        ///           "email": "user@example.com",
        ///           "welcomeText": "string",
        ///           "aboutUsText": "string",
        ///           "facebookUrl": "string",
        ///           "linkedinUrl": "string",
        ///           "instagramUrl": "string"
        ///         }
        ///     }
        /// </remarks>
        /// <response code="201">Creada con exito</response>
        [HttpPost()]
        public async Task<IActionResult> CreateSlideImageB64(SlideCreateDTO newSlide)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var putRequest = _awsS3Service.PutObjectRequestImageBase64(newSlide.ImageBase64, newSlide.ImageName);

            var result = await _amazonS3.PutObjectAsync(putRequest);

            if (result.HttpStatusCode.ToString() != "OK")
                return BadRequest();

            var slide = await _slideService.GetAllAsync();
            var slideTable = _mapper.Map<Slide>(slide.OrderByDescending(x => x.Order).First());

            var _slide = _mapper.Map<Slide>(newSlide);

            if (slide == null)
                _slide.Order = 1;
            else
                _slide.Order = slideTable.Order + 1;

            _slide.ImageUrl = _awsS3Service.GetUrlRequest("slides/" + newSlide.ImageName);
            //_slide.ImageUrl = newSlide.ImageName;

            var created = await _slideService.CreateAsync(_slide);

            if (created)
                _unitOfWork.Commit();

            return Created("Created", new { Response = StatusCode(201) });
        }

        /// <summary>
        /// Subir imagen con upload al bucket - jpg o png
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /slide
        ///     {
        ///         upload file
        ///     }
        /// </remarks>
        /// <response code="201">Creada con exito</response>
        [HttpPost("File")]
        public async Task<IActionResult> CreateSlideFile(IFormFile file)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var putRequest = _awsS3Service.PutRequest(file);

            var result = await _amazonS3.PutObjectAsync(putRequest);
            return Ok(result);
        }

        #region No se puede combinar multipart/form data con IFormFile
        //IFormFile es solo para solicitudes codificadas de datos de formulario/varias partes,
        //y no puede mezclar y combinar codificaciones de cuerpo de solicitud cuando usa ModelBinder.
        //Por lo tanto, tendría que dejar de usar JSON o enviar el archivo en el objeto JSON,
        //como se describe anteriormente, en lugar de usar IFormFile.
        //[HttpPost("file")]
        //public async Task<IActionResult> CreateSlideFile(IFormFile file, SlideCreateDTO newSlide)
        //{
        //    if (!ModelState.IsValid)
        //        return BadRequest();

        //    var _slide = _mapper.Map<Slide>(newSlide);

        //    var putRequest = _awsS3Service.PutObjectRequestImageBase64(newSlide.ImageBase64, newSlide.ImageName);

        //    var result = await _amazonS3.PutObjectAsync(putRequest);

        //    if (result.HttpStatusCode.ToString() != "OK")
        //        return BadRequest();

        //    _slide.ImageUrl = newSlide.ImageName;


        //    var created = await _slideService.CreateAsync(_slide);

        //    if (created)
        //        _unitOfWork.Commit();

        //    return Created("Created", new { Response = StatusCode(201) });
        //}
        #endregion

        #region imagen con nombre del objeto en el bucket
        //[HttpGet("Image")]
        //public async Task<IActionResult> Get(string imageName)
        //{
        //    var response = await _amazonS3.GetObjectAsync(BucketName, "slides/" + imageName);
        //    return File(response.ResponseStream, response.Headers.ContentType);
        //}
        #endregion

        #region test para ver la urls de una lista de objetos
        // test para ver la urls de una lista de objetos
        // tiene time expire
        //[HttpGet("GetUrls")]
        //public async Task<IActionResult> GetList(string prefix)
        //{
        //    var request = _awsS3Service.ListObjectV2(prefix);

        //    var response = await _amazonS3.ListObjectsV2Async(request);
        //    var preSignedUrls = response.S3Objects.Select(o =>
        //    {
        //        var request = new GetPreSignedUrlRequest()
        //        {
        //            BucketName = BucketName,
        //            Key = o.Key,
        //            Expires = System.DateTime.UtcNow.AddDays(1)
        //        };
        //        return _amazonS3.GetPreSignedURL(request);
        //    });

        //    return Ok(preSignedUrls);
        //}
        #endregion

        [HttpGet()]
        public async Task<IActionResult> GetAll()
        {
            var slide = _unitOfWork.Context.Slides.AsQueryable();

            slide = slide.OrderBy(s => s.Order);

            var slides = await slide.AsNoTracking().ToListAsync();

            var slideDTO = _mapper.Map<IEnumerable<SlideListDTO>>(slides);

            return new OkObjectResult(slideDTO);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SlideDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var slide = await _slideService.GetById(id);
            if (slide == null)
                return NotFound();

            var slideDTO = _mapper.Map<SlideDTO>(slide);

            return new OkObjectResult(slideDTO);
        }
        
        [HttpGet("ImageById/{id}")]
        public async Task<IActionResult> GetImageById(int id)
        {
            var slide = await _slideService.GetById(id);
            if (slide is null)
                return NotFound();

            string[] split1 = slide.ImageUrl.Split("?");
            string[] image = split1[0].Split("/");

            var response = await _amazonS3.GetObjectAsync(BucketName, "slides/" + image[4].ToString());
            return File(response.ResponseStream, response.Headers.ContentType);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, SlideCreateDTO slideCreateDto)
        {
            var entity = await _slideService.GetById(id);

            if (entity is not null) 
                return NotFound();

            await _slideService.UpdateSlide(entity, slideCreateDto);
            var slideDto = _mapper.Map<SlideCreateDTO>(entity);
            _unitOfWork.Commit();

            return new OkObjectResult(slideDto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var slide = await _slideService.GetById(id);

            if (slide is null)
                return NotFound(nameof(slide));

            await _slideService.DeleteAsync(slide);
            _unitOfWork.Commit();

            return Ok(slide);
        }

        //[HttpDelete("DeleteImage")]
        //public async Task<IActionResult> Delete(string imageName)
        //{
        //    var response = await _amazonS3.GetObjectAsync(BucketName, "slides/" + imageName);
        //    return File(response.ResponseStream, response.Headers.ContentType);
        //}
    }
}
