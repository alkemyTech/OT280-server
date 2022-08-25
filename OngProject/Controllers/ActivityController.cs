using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OngProject.Core.Models;
using OngProject.Core.Models.DTOs;
using OngProject.Repositories.Interfaces;
using OngProject.Services.Interfaces;
using System.Threading.Tasks;

namespace OngProject.Controllers
{
    [ApiController]
    public class ActivityController : ControllerBase
    {
        private readonly IActivityService _activityService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ActivityController(IActivityService activityService, IUnitOfWork unitOfWork, IMapper mapper)
        {
            this._activityService = activityService;
            this._unitOfWork = unitOfWork;
            this._mapper = mapper;
        }

        [HttpPost]
        [Route("/activities")]
        public async Task<IActionResult> Create(ActivityDTO activity)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var _activity = _mapper.Map<Activities>(activity);
            var created = await _activityService.CreateAsync(_activity);

            if (created)
                _unitOfWork.Commit();

            return Created("Created", new { Response = StatusCode(201) });
        }

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

