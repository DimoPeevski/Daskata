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

        private DbSet<T> DbSet<T>() where T : class
        {
            return _context.Set<T>();
        }
    }
}
