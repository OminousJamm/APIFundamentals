using APIFundamentals.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace APIFundamentals.DBContexts
{
    public class AppDBContext: DbContext
    {
        public DbSet<User> Users { get; set; }


        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasIndex(u => new { u.UserName, u.Email }).IsUnique();
            modelBuilder.Entity<User>().HasData(

                new User { Id = 1, UserName = "Carlos Pérez", Email = "carlos@example.com", Password = "Admin" },
                new User { Id = 2, UserName = "Ana Gómez", Email = "ana@example.com", Password = "Admin" },
                new User { Id = 3, UserName = "Luis Torres", Email = "luis@example.com", Password = "Admin" }
            );
        }
    }
}