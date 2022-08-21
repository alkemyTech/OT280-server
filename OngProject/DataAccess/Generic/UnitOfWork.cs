using System;

namespace OngProject.DataAccess.Generic
{
    public interface IUnitOfWork : IDisposable
    {
        AppDbContext Context { get; }
        void commit();
    }

    public class UnitOfWork : IUnitOfWork
    {
        public AppDbContext Context { get; }

        public UnitOfWork(AppDbContext context)
        {
            Context = context;
        }

        public void commit()
        {
            Context.SaveChanges();
        }

        public void Dispose()
        {
            Context.Dispose();
        }
    }
}
