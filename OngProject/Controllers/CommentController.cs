using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OngProject.Core.Models;
using OngProject.Core.Models.DTOs;
using OngProject.Repositories.Interfaces;
using OngProject.Services.Interfaces;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Annotations;

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
            _commentService = commentService;
            _userService = userService;
            _newService = newService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        //[Authorize(Roles = "admin")]

        #region Documentation
        [SwaggerOperation(Summary = "List of all Comments", Description = "Requires admin privileges")]
        [SwaggerResponse(200, "Success")]
        [SwaggerResponse(400, "BadRequest. Something went wrong, try again")]
        [SwaggerResponse(401, "Unauthenticated user or wrong jwt token")]
        [SwaggerResponse(403, "Unauthorized user")]
        #endregion
        [Authorize(Roles = "admin")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var commentsIq = _unitOfWork.Context.Comments.AsQueryable();

            commentsIq = commentsIq.OrderBy(c => c.Date_Create);

            var comments = await commentsIq.AsNoTracking().ToListAsync();
            var commentDto = _mapper.Map<IEnumerable<CommentGetAllDTO>>(comments);

            return new OkObjectResult(commentDto);
        }
        
        #region Documentation
        [SwaggerOperation(Summary = "Create Comment.",Description = "Requires user privileges.")]
        [SwaggerResponse(200, "Created. Returns the id of the created object.")]
        [SwaggerResponse(400, "BadRequest. Object not created, try again.")]
        [SwaggerResponse(401, "Unauthenticated or wrong jwt token.")]
        [SwaggerResponse(500, "Internal server error. An error occurred while processing your request.")]
        #endregion
        [Authorize(Roles = "admin, standard")]
        [HttpPost]
        public async Task<IActionResult> Create(CommentDTO comment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var news = await _newService.GetById(comment.news_id);
            if (news == null)
                return NotFound("New not found");

            var user = await _userService.GetById(comment.user_id);
            if (user == null)
                return NotFound("User not found");

            var comments = _mapper.Map<Comments>(comment);

            comments.Date_Create = DateTime.Now;

            var created = await _commentService.CreateAsync(comments);

            if (created)
                _unitOfWork.Commit();

            return Created("Created", new { Response = StatusCode(201) });
        }
        
        #region Documentation
        [SwaggerOperation(Summary = "Get Comments details by Id", Description = "Requires admin privileges")]
        [SwaggerResponse(200, "Success. Returns Comments details")]
        [SwaggerResponse(401, "Unauthenticated user or wrong jwt token")]
        [SwaggerResponse(403, "Unauthorized user")]
        [SwaggerResponse(404, "NotFound. Entity id not found.")]
        [SwaggerResponse(500, "Internal server error. An error occurred while processing your request.")]
        #endregion
        [HttpGet]
        [Route("~/news/{id}/comments")]
        [Authorize(Roles = "admin, standard")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByNewId(int id)
        {
            var comment = await _commentService.GetAsync(x => x.news_id == id);

            if (comment == null)
            {
                return NotFound();
            }

            return new OkObjectResult(comment);
        }
        
        #region Documentation
        [SwaggerOperation(Summary = "Modifies an existing Comment", Description = "Requires admin/user privileges")]
        [SwaggerResponse(204, "Updated. Returns nothing.")]
        [SwaggerResponse(400, "BadRequest. Something went wrong, try again.")]
        [SwaggerResponse(401, "Unauthenticated or wrong jwt token.")]
        [SwaggerResponse(403, "Unauthorized user.")]
        [SwaggerResponse(404, "NotFound. Entity id not found.")]
        [SwaggerResponse(500, "Internal server error. An error occurred while processing your request.")]
        #endregion
        [HttpPut]
        [Route("/comments/{id}")]
        [Authorize(Roles = "admin, standard")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CommentDTO))]
        [ProducesResponseType(StatusCodes.Status403Forbidden)] 
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(int id, CommentDTO commentDto)
        {
            var entity = await _commentService.GetById(id);

            var user = await _userService.GetById(commentDto.user_id);

            if (ModelState.IsValid && entity != null)
            {
                if (user == null || entity.user_id != user.Id)
                    return StatusCode(StatusCodes.Status403Forbidden);
                else
                {
                    _commentService.UpdateComment(entity, commentDto);
                    _unitOfWork.Commit();
                    return new OkObjectResult(commentDto);
                }
            }
            
            return NotFound();                         
        }
        
        #region Documentation
        [SwaggerOperation(Summary = "Soft Delete an existing Comment", Description = "Requires admin/user privileges")]
        [SwaggerResponse(204, "Deleted. Returns nothing.")]
        [SwaggerResponse(401, "Unauthenticated user or wrong jwt token.")]
        [SwaggerResponse(403, "Unauthorized user.")]
        [SwaggerResponse(404, "NotFound. Entity id not found.")]
        [SwaggerResponse(500, "Internal server error. An error occurred while processing your request.")]
        #endregion
        [HttpDelete]
        [Route("/comments/{id}")]
        [Authorize(Roles = "admin, standard")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CommentDTO))]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id, DeleteCommentDTO commentDto)
        {
            var entity = await _commentService.GetById(id);

            var user = await _userService.GetById(commentDto.user_id);

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
