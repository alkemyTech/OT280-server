using OngProject.Core.Models;
using OngProject.Repositories.Interfaces;

namespace OngProject.Repositories
{
    public class CommentRepository : GenericRepository<Comments>, ICommentRepository
    {
        public CommentRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }
    }
}
