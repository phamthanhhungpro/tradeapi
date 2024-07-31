using Microsoft.EntityFrameworkCore;
namespace trade.InfraModel.DataAccess
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = Guid.NewGuid(),
                    Email = "su@trade.vn",
                    PassWordHash = "123 123",
                    Role = Shared.Enum.RoleEnum.Admin,
                    CreatedBy = Guid.NewGuid(),
                    CreatedAt = DateTime.UtcNow,
                    IsDeleted = false
                });

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
                entity.Property(e => e.IsDeleted).HasDefaultValue(false);
                entity.HasQueryFilter(e => !e.IsDeleted);
            });
        }
    }
}
