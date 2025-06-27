using Entities.Models;
using Lamar;
using Microsoft.EntityFrameworkCore.Storage;

namespace DAL.UoW
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IContainer _container;

        private readonly AutoSystemDbContext _context;

        public UnitOfWork(IContainer container, AutoSystemDbContext context)
        {
            _context = context;
            _container = container;
        }


        public TRepository GetRepository<TRepository>() where TRepository : class
        {
            return _container.GetInstance<TRepository>();
        }

        public int Save()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
        }
        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await _context.Database.BeginTransactionAsync();
        }

        public async Task<int> CommitAsync()
        {
            return await _context.SaveChangesAsync();
        }


    }

}