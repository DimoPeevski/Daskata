using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Configuration;

namespace Daskata.Data
{
    public class DaskataDbContext : IdentityDbContext
    {
        public DaskataDbContext(DbContextOptions<DaskataDbContext> options)
            : base(options)
        {
        }

    }
}
