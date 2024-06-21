using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Data
{
    public class ProductContext : DbContext
    {
        public ProductContext(DbContextOptions<ProductContext> options) : base(options) { }
        public DbSet<Product> Products { get; set; }
    }

}
