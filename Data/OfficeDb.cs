using Microsoft.EntityFrameworkCore;
using MinimalAPIPostgreSQLEntityFramework.Models;

namespace MinimalAPIPostgreSQLEntityFramework.Data
{
    public class OfficeDb : DbContext
    {
        public OfficeDb(DbContextOptions<OfficeDb> options) : base(options)
        {

        }

        public DbSet<Employee> Employees => Set<Employee>();
    }
}
