using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace OngProject.Repositories.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();

        Task<IEnumerable<T>> GetAsync(Expression<Func<T, bool>> whereCondition = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string includeProperties = "");

        Task<T> GetById(int id);
        Task<T> GetById(string id);
        Task<bool> CreateAsync(T entity);
        Task<bool> Update(T entity);
        Task<int> DeleteAsync(T entity);
        void Delete(T entity);
        bool Delete_(T entity);

    }
}
