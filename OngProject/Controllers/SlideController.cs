using Amazon.S3;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using OngProject.Core.Models;
using OngProject.Repositories.Interfaces;
using OngProject.Services.Interfaces;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Linq;
using OngProject.Core.Models.DTOs.Slide;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Xml.Linq;
using Swashbuckle.AspNetCore.Annotations;
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

        #region Sample request
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
        #endregion
        #region Documentation
        [SwaggerOperation(Summary = "Create Slides with ImageB64", Description = "Requires admin privileges")]
        [SwaggerResponse(200, "Created. Returns the id of the created object.")]
        [SwaggerResponse(400, "BadRequest. Object not created, try again.")]
        [SwaggerResponse(401, "Unauthenticated or wrong jwt token.")]
        [SwaggerResponse(403, "Unauthorized user.")]
        [SwaggerResponse(500, "Internal server error. An error occurred while processing your request.")]
        #endregion
        [HttpPost]
        public async Task<IActionResult> CreateSlideImageB64(SlideCreateDTO newSlide)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var putRequest = _awsS3Service.PutObjectRequestImageBase64(newSlide.ImageBase64, newSlide.ImageName);

            var result = await _amazonS3.PutObjectAsync(putRequest);

            if (result.HttpStatusCode.ToString() != "OK")
                return BadRequest();

            var _slide = _mapper.Map<Slide>(newSlide);

            var slide = await _slideService.GetAllAsync();
            
            if (!slide.Any()) 
                _slide.Order = 1;
            else
            {
                var slideTable = _mapper.Map<Slide>(slide.OrderByDescending(x => x.Order).First());
                _slide.Order = slideTable.Order + 1;
            }

            _slide.ImageUrl = _awsS3Service.GetUrlRequest("slides/" + newSlide.ImageName);

            var created = await _slideService.CreateAsync(_slide);

            if (created)
                _unitOfWork.Commit();

            return Created("Created", new { Response = StatusCode(201) });
        }

        #region Sample request
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /slide
        ///     {
        ///         upload file
        ///     }
        /// </remarks>
        #endregion
        #region Documentation
        [SwaggerOperation(Summary = "Create Slides", Description = "Requires admin privileges")]
        [SwaggerResponse(200, "Created. Returns the id of the created object.")]
        [SwaggerResponse(400, "BadRequest. Object not created, try again.")]
        [SwaggerResponse(401, "Unauthenticated or wrong jwt token.")]
        [SwaggerResponse(403, "Unauthorized user.")]
        [SwaggerResponse(500, "Internal server error. An error occurred while processing your request.")]
        #endregion
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
       
        #region Documentation
        [SwaggerOperation(Summary = "List of all Slides", Description = "Requires admin privileges")]
        [SwaggerResponse(200, "Success. Returns a list of existing Slides")]
        [SwaggerResponse(401, "Unauthenticated user or wrong jwt token")]
        [SwaggerResponse(403, "Unauthorized user")]
        [SwaggerResponse(500, "Internal server error. An error occurred while processing your request.")]
        #endregion
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var slide = _unitOfWork.Context.Slides.AsQueryable();

            slide = slide.OrderBy(s => s.Order);

            var slides = await slide.AsNoTracking().ToListAsync();

            var slideDto = _mapper.Map<IEnumerable<SlideListDTO>>(slides);

            return new OkObjectResult(slideDto);
        }
        
        #region Documentation
        [SwaggerOperation(Summary = "Get slide details by id", Description = "Requires admin privileges")]
        [SwaggerResponse(200, "Success. Returns the slide details.")]
        [SwaggerResponse(404, "NotFound. Entity id not found.")]
        [SwaggerResponse(401, "Unauthenticated user or wrong jwt token.")]
        [SwaggerResponse(403, "Unauthorized user.")]
        [SwaggerResponse(500, "Internal server error. An error occurred while processing your request.")]
        #endregion
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SlideDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var slide = await _slideService.GetById(id);
            if (slide == null)
                return NotFound();

            var slideDto = _mapper.Map<SlideDTO>(slide);

            return new OkObjectResult(slideDto);
        }
<<<<<<< HEAD

=======
        
        #region Documentation
        [SwaggerOperation(Summary = "Get Image details by id", Description = "Requires admin privileges")]
        [SwaggerResponse(200, "Success. Returns the slide details.")]
        [SwaggerResponse(404, "NotFound. Entity id not found.")]
        [SwaggerResponse(401, "Unauthenticated user or wrong jwt token.")]
        [SwaggerResponse(403, "Unauthorized user.")]
        [SwaggerResponse(500, "Internal server error. An error occurred while processing your request.")]
        #endregion
>>>>>>> develop
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
        
        #region Documentation
        [SwaggerOperation(Summary = "Modifies an existing Slide", Description = "Requires admin privileges")]
        [SwaggerResponse(200, "Updated. Returns nothing")]
        [SwaggerResponse(400, "BadRequest. Something went wrong, try again.")]
        [SwaggerResponse(401, "Unauthenticated or wrong jwt token.")]
        [SwaggerResponse(403, "Unauthorized user.")]
        [SwaggerResponse(404, "NotFound. Entity id not found.")]
        [SwaggerResponse(500, "Internal server error. An error occurred while processing your request.")]
        #endregion
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, SlideCreateDTO slideCreateDto)
        {
            var entity = await _slideService.GetById(id);

<<<<<<< HEAD
            if (entity is not null)
=======
            if (entity is null) 
>>>>>>> develop
                return NotFound();

            await _slideService.UpdateSlide(entity, slideCreateDto);
            var slideDto = _mapper.Map<SlideCreateDTO>(entity);
            _unitOfWork.Commit();

            return new OkObjectResult(slideDto);
        }
        
        #region Documentation
        [SwaggerOperation(Summary = "Soft delete an existing Slide", Description = "Requires admin privileges")]
        [SwaggerResponse(204, "Deleted. Returns nothing.")]
        [SwaggerResponse(401, "Unauthenticated user or wrong jwt token.")]
        [SwaggerResponse(403, "Unauthorized user.")]
        [SwaggerResponse(404, "NotFound. Entity id not found.")]
        [SwaggerResponse(500, "Internal server error. An error occurred while processing your request.")]
        #endregion
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
        
    }
}
