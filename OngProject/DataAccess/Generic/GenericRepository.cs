using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OngProject.DataAccess.Generic
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        Task<IEnumerable<TEntity>> GetAllAsync();
        ValueTask<TEntity> GetByIdAsync(int id);
        Task<bool> AddAsync(TEntity entity);
        Task<bool> Delete(TEntity entity);
    }

    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        private readonly IUnitOfWork _unitOfWork;

        public GenericRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork; 
        }


        public async Task<bool> AddAsync(TEntity entity)
        {
            bool created = false;

            try
            {
                var save = await _unitOfWork.Context.Set<TEntity>().AddAsync(entity);

                if (save != null)
                    created = true;
            }
            catch (Exception)
            {
                throw;
            }
            return created;
        }

        public Task<bool> Delete(TEntity entity)
        {
            bool deleted = false;

            try
            {
                var remove =  _unitOfWork.Context.Set<TEntity>().Remove(entity);

                if (remove != null)
                    deleted = true;
            }
            catch (Exception)
            {
                throw;
            }

            return Task.FromResult(deleted);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await _unitOfWork.Context.Set<TEntity>().ToListAsync();
        }

        public async ValueTask<TEntity> GetByIdAsync(int id)
        {
            return await _unitOfWork.Context.Set<TEntity>().FindAsync(id);
        }
    }
}
