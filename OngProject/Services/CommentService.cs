using OngProject.Core.Models.DTOs;
using OngProject.Core.Models;
using OngProject.Repositories.Interfaces;
using OngProject.Services.Interfaces;

namespace OngProject.Services
{
    public class CommentService : GenericService<Comments>, ICommentService
    {
        private ICommentRepository _commentRepository;
        public CommentService(ICommentRepository commentRepository) : base(commentRepository)
        {
            this._commentRepository = commentRepository;
        }

        public async void UpdateComment(Comments comment, CommentDTO commentDTO)
        {
            comment.body = commentDTO.body;

            await _commentRepository.Update(comment);

        }
    }
}
