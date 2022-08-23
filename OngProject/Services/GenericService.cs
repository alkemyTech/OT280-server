using OngProject.Repositories.Interfaces;
using OngProject.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace OngProject.Services
{
    public class GenericService<T> : IGenericService<T> where T : class
    {

        private IGenericRepository<T> _genericRepository;

        public GenericService(IGenericRepository<T> genericRepository)
        {
            this._genericRepository = genericRepository;
        }

        public async Task<bool> CreateAsync(T entity)
        {
            return await _genericRepository.CreateAsync(entity);
        }

        public void Delete(T entity)
        {
            _genericRepository.Delete(entity);
        }

        public async Task<int> DeleteAsync(T entity)
        {
            return await _genericRepository.DeleteAsync(entity);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _genericRepository.GetAllAsync();
        }

        public async Task<IEnumerable<T>> GetAsync(Expression<Func<T, bool>> whereCondition = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = "")
        {
            return await _genericRepository.GetAsync(whereCondition, orderBy, includeProperties);
        }

        public async Task<T> GetById(int id)
        {
            return await _genericRepository.GetById(id);
        }

        public async Task<T> GetById(string id)
        {
            return await _genericRepository.GetById(id);
        }

        public async Task<bool> Update(T entity)
        {
            return await _genericRepository.Update(entity);
        }
    }
}
