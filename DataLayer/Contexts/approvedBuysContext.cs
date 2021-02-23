using BusinessEntities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class approvedBuysContext : DbContext
    {
        public approvedBuysContext() : base("approvedBuysContextDb")
        {

        }

        public DbSet<buy> Buys { get; set; }
    }
}
