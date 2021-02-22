using BusinessEntities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class BuyContext : DbContext
    {
        public BuyContext() : base("BuysDB")
        {

        }

        public DbSet<buy> Buys { get; set; }
    }
}
