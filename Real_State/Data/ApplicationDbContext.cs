// DbContext/ApplicationDbContext.cs

using Microsoft.EntityFrameworkCore;
using Real_State.Modules;

namespace Real_State.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<RegisterModule> UsersTable { get; set; }
        public DbSet<StateModule> StatesTable { get; set; }



    }
}
