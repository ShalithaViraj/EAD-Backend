using Microsoft.EntityFrameworkCore;
using Travalers.Entities.Sql;

namespace Travalers.Data
{
    public class SqliteDbContext : DbContext
    {

        public SqliteDbContext(DbContextOptions<SqliteDbContext> options) : base(options) 
        { 
        
        }

        public DbSet<UserSql> Users => Set<UserSql>();

    }
}
