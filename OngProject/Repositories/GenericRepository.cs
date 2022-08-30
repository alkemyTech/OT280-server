
using Microsoft.EntityFrameworkCore;
using OngProject.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace OngProject.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly IUnitOfWork _unitOfWork;

        public GenericRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<T>> GetAllAsync() => await _unitOfWork.Context.Set<T>().ToListAsync();

        public async Task<IEnumerable<T>> GetAsync(Expression<Func<T, bool>> whereCondition = null, 
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, 
            string includeProperties = "")
        {
            IQueryable<T> query = _unitOfWork.Context.Set<T>();

            if (whereCondition != null)
            {
                query = query.Where(whereCondition);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                return await orderBy(query).ToListAsync();
            }
            else
            {
                return await query.ToListAsync();
            }
        }


        public async Task<bool> CreateAsync(T entity)
        {
            bool created = false;

            try
            {
                var save = await _unitOfWork.Context.Set<T>().AddAsync(entity);

                if (save != null)
                    created = true;
            }
            catch (Exception)
            {
                throw;
            }
            return created;
        }

        public async Task<T> GetById(int id)
        {
            return await _unitOfWork.Context.Set<T>().FindAsync(id);
        }

        public async Task<T> GetById(string id)
        {
            return await _unitOfWork.Context.Set<T>().FindAsync(id);
        }

        public Task<bool> Update(T entity)
        {            
            _unitOfWork.Context.Set<T>().Update(entity);

            return Task.FromResult(true);
        }

        public void Delete(T entity)
        {
            _unitOfWork.Context.Set<T>().Remove(entity);
            _unitOfWork.Context.SaveChanges();
        }

        public async Task<int> DeleteAsync(T entity)
        {
            int deleted = 0;

            try
            {
                _unitOfWork.Context.Set<T>().Remove(entity);
                var save = await _unitOfWork.Context.SaveChangesAsync();

                if (save > 0)
                    deleted = 1;
            }
            catch (Exception)
            {
                throw;
            }
            return deleted;
        }

        public bool Delete_(T entity)
        {
            bool deleted = false;

            try
            {
                var remove = _unitOfWork.Context.Set<T>().Remove(entity);

                if (remove != null)
                    deleted = true;
            }
            catch (Exception)
            {
                throw;
            }

            return deleted;
        }
    }
}
