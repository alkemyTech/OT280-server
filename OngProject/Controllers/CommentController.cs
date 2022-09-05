using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OngProject.Core.Models;
using OngProject.Core.Models.DTOs;
using OngProject.Repositories.Interfaces;
using OngProject.Services.Interfaces;

namespace OngProject.Controllers
{
    [Route("comments")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentService _commentService;
        private readonly IUserService _userService;
        private readonly INewService _newService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CommentController(ICommentService commentService, 
            INewService newService, IUserService userService, 
            IUnitOfWork unitOfWork, IMapper mapper)
        {
            this._commentService = commentService;
            this._userService = userService;
            this._newService = newService;
            this._unitOfWork = unitOfWork;
            this._mapper = mapper;
        }

        //[Authorize(Roles = "standard")]
        [HttpPost]
        public async Task<IActionResult> Create(CommentDTO comment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var _new = await _newService.GetById(comment.news_id);
            if (_new == null)
                return NotFound("New not found");

            var user = await _userService.GetById(comment.user_id);
            if (user == null)
                return NotFound("User not found");

            var _comment = _mapper.Map<Comments>(comment);
            var created = await _commentService.CreateAsync(_comment);

            if (created)
                _unitOfWork.Commit();

            return Created("Created", new { Response = StatusCode(201) });
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByNewId(int id)
        {
            var comment = await _commentService.GetAsync(x => x.news_id == id, null, "");

            if (comment == null)
            {
                return NotFound();
            }

            return new OkObjectResult(comment);
        }

        [HttpPut]
        [Route("/comments/{id}")]
        //[Authorize(Roles = "admin")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CommentDTO))]
        [ProducesResponseType(StatusCodes.Status403Forbidden)] 
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(int id, CommentDTO commentDTO)
        {
            var entity = await _commentService.GetById(id);

            var user = await _userService.GetById(commentDTO.user_id);

            if (ModelState.IsValid && entity != null)
            {
                if (user == null || entity.user_id != user.Id)
                    return StatusCode(StatusCodes.Status403Forbidden);
                else
                {
                    _commentService.UpdateComment(entity, commentDTO);
                    _unitOfWork.Commit();
                    return new OkObjectResult(commentDTO);
                }
            }
            
            return NotFound();                         
        }

        [HttpDelete]
        [Route("/comments/{id}")]
        //[Authorize(Roles = "admin")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CommentDTO))]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id, DeleteCommentDTO commentDTO)
        {
            var entity = await _commentService.GetById(id);

            var user = await _userService.GetById(commentDTO.user_id);

            if (ModelState.IsValid && entity != null)
            {
                if (user == null || entity.user_id != user.Id)
                    return StatusCode(StatusCodes.Status403Forbidden);
                else
                {
                    await _commentService.DeleteAsync(entity);
                    _unitOfWork.Commit();
                    return Ok();
                }
            }

            return NotFound();
        }
    }
}
