using Microsoft.EntityFrameworkCore;
using ProniaApplication.Models;
using System.Drawing;

namespace ProniaApplication.DAL
{
    public class AppDBContext:DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options):base(options) { }

        public DbSet<Slide> Slides { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
      
        public DbSet<ProductsImage> ProductsImages { get; set; }

        public DbSet<Tag> Tags { get; set; }
        public DbSet<ProductTag> ProductTags { get; set; }

        public DbSet<Models.Color> Colors { get; set; }
        public DbSet<Models.Size> Sizes { get; set; }

        public DbSet<ProductColor> ProductColors { get; set; }
        public DbSet<ProductSize> ProductSizes { get; set; }
    }
}
