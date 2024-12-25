using Microsoft.EntityFrameworkCore;

namespace WebFormCRUD.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // DbSet for User entities
        public DbSet<User> Users { get; set; } = null!; // Using null-forgiving operator
    }

    public class User
    {
        // Properties with default initialization to avoid nullability warnings
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty; // Initialized to empty string
        public string Email { get; set; } = string.Empty; // Initialized to empty string
        public int Age { get; set; }
    }
}
