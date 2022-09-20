using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using OngProject.Core.Models;
using OngProject.Core.Models.DTOs;
using OngProject.Repositories.Interfaces;
using OngProject.Services.Interfaces;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Swashbuckle.AspNetCore.Annotations;

namespace OngProject.Controllers
{
    [ApiController]
    //[Route("api/")]
    [Authorize(Roles = "admin")]
    public class ActivityController : ControllerBase
    {
        private readonly IActivityService _activityService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ActivityController(IActivityService activityService, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _activityService = activityService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        #region Documentation
        [SwaggerOperation(Summary = "Create a Activity", Description = "Requires admin privileges.")]
        [SwaggerResponse(200, "Created. Returns the id of the created object.")]
        [SwaggerResponse(400, "BadRequest. Object not created, try again.")]
        [SwaggerResponse(401, "Unauthenticated or wrong jwt token.")]
        [SwaggerResponse(403, "Unauthorized user.")]
        [SwaggerResponse(500, "Internal server error. An error occurred while processing your request.")]
        #endregion
        [HttpPost]
        [Route("/activities")]
        
        public async Task<IActionResult> Create(ActivityDTO activityDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var activity = _mapper.Map<Activities>(activityDto);
            var created = await _activityService.CreateAsync(activity);

            if (created)
                _unitOfWork.Commit();

            return Created("Created", new { Response = StatusCode(201) });
        }

        #region Documentation
        [SwaggerOperation(Summary = "Update a Activity",Description = "Require admin privileges")]
        [SwaggerResponse(204, "Success. Returns nothing.")]
        [SwaggerResponse(400, "BadRequest. Something went wrong, try again")]
        [SwaggerResponse(401, "Unauthenticated user or wrong jwt token")]
        [SwaggerResponse(403, "Unauthorized user or wrong jwt token")]
        [SwaggerResponse(500, "Internal server error. An error occurred while processing your request.")]
        #endregion
        [HttpPut]
        [Route("/activities/{id}")]
        public async Task<ActionResult> Update(int id, ActivityDTO activity)
        {
            var entity = await _activityService.GetById(id);

            if (ModelState.IsValid && entity != null)
            {
                _activityService.UpdateActivity(entity, activity);
                _unitOfWork.Commit();

                return new OkObjectResult(activity);
            }

            return NotFound();
        }
    }
}

