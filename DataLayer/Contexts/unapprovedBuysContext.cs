using BusinessEntities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class unapprovedBuysContext : DbContext
    {
        public unapprovedBuysContext() : base("unapprovedBuysContextDB")
        {

        }

        public DbSet<buy> Buys { get; set; }
    }
}
