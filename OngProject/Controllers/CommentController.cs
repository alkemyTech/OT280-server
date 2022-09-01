using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OngProject.Repositories.Interfaces;
using OngProject.Services.Interfaces;

namespace OngProject.Controllers
{
    [Route("comments")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentService _commentService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CommentController(ICommentService commentService, IUnitOfWork unitOfWork, IMapper mapper)
        {
            this._commentService = commentService;
            this._unitOfWork = unitOfWork;
            this._mapper = mapper;
        }
    }
}
