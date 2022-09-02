using OngProject.Core.Models;
using OngProject.Core.Models.DTOs;

namespace OngProject.Services.Interfaces
{
    public interface ICommentService : IGenericService<Comments>
    {
        void UpdateComment(Comments comment, CommentDTO commentDTO);

    }
}
