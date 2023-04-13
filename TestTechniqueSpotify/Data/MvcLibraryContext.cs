using Microsoft.EntityFrameworkCore;

namespace MvcLibrary.Data
{
    public class MvcLibraryContext : DbContext
    {
        public MvcLibraryContext(DbContextOptions<MvcLibraryContext> options)
            : base(options)
        {
        }

        public DbSet<MvcLibrary.Models.Library> Library { get; set; } = default!;
    }
}
