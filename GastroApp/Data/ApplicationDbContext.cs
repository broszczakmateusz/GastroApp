using Microsoft.EntityFrameworkCore;

namespace GastroApp.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
            : base(options) { }
    }
}
