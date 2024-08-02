using Microsoft.EntityFrameworkCore;
using trade.InfraModel.DataAccess;
namespace trade.InfraModel.DataAccess
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Token> Tokens { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
                entity.Property(e => e.IsDeleted).HasDefaultValue(false);
                entity.HasQueryFilter(e => !e.IsDeleted);
            });

            modelBuilder.Entity<Token>(entity =>
             {
                 entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
                 entity.Property(e => e.IsDeleted).HasDefaultValue(false);
                 entity.HasOne(t => t.User)
                       .WithMany(u => u.Tokens)
                       .HasForeignKey(t => t.UserId);
             });
            var socialMediaCategoryId = Guid.NewGuid();
            var gamingCategoryId = Guid.NewGuid();

            modelBuilder.Entity<Category>().HasData(
               new Category { Id = socialMediaCategoryId, CategoryName = "Social Media", CreatedAt = DateTime.UtcNow },
               new Category { Id = gamingCategoryId, CategoryName = "Gaming", CreatedAt = DateTime.UtcNow });

            // Seed data for Product
            modelBuilder.Entity<Product>().HasData(
                new Product { Id = Guid.NewGuid(), Name = "Facebook Account", CategoryId = socialMediaCategoryId, CreatedAt = DateTime.UtcNow },
                new Product { Id = Guid.NewGuid(), Name = "TikTok Account", CategoryId = socialMediaCategoryId, CreatedAt = DateTime.UtcNow },
                new Product { Id = Guid.NewGuid(), Name = "Garena Account", CategoryId = gamingCategoryId, CreatedAt = DateTime.UtcNow });
        }
    }
}
