using Daskata.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Daskata.Infrastructure.Common
{
    public class Repository : IRepository
    {
        private readonly DbContext _context;

        public Repository(DaskataDbContext context)
        {
            _context = context;
        }


        public IQueryable<T> All<T>() where T : class
        {
            return DbSet<T>();
        }

        public IQueryable<T> AllReadonly<T>() where T : class
        {
            return DbSet<T>()
                .AsNoTracking();
        }
        public async Task AddAsync<T>(T entity) where T : class
        {
            await _context.Set<T>().AddAsync(entity);
        }

        public void Remove<T>(T entity) where T : class
        {
            _context.Set<T>().Remove(entity);
        }

        public Task<int> SaveChangesAsync()
        {
            return _context.SaveChangesAsync();
        }


        private DbSet<T> DbSet<T>() where T : class
        {
            return _context.Set<T>();
        }
    }
}
