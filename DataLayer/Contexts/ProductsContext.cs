using BusinessEntities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class ProductsContext : DbContext
    {
        public ProductsContext() : base("ProductsDB2")
        {

        }

        public DbSet<product> Products { get; set; }
    }
}
