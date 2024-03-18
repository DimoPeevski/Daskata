using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Daskata.Infrastructure.Data
{
    public class DaskataDbContext : IdentityDbContext
    {
        public DaskataDbContext(DbContextOptions<DaskataDbContext> options)
            : base(options)
        {
        }

    }
}
