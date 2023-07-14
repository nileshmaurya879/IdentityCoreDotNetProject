using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProductAPI.Model;
using System;

namespace ProductAPI.Data
{
    public class ProductContext : IdentityDbContext<ApplicationUser>
    {
        public ProductContext(DbContextOptions<ProductContext> options):base(options)
        {

        }

        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Book> Books { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Book>(entity =>
            {
                entity.ToTable("Book");
            });

            modelBuilder.Entity<Book>(entity =>
            {
                entity.ToTable("Product");
            });
        }
    }
}
