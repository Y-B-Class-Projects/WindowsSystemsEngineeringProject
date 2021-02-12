using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessEntities;

namespace DataLayer
{
    public class DAL
    {
        public void addProduct(product p)
        {
            using (var ctx = new ProductsContext())
            {
                ctx.Products.Add(p);
                ctx.SaveChanges();
            }
        }

        public string getGoogleData()
        {
            return GoogleAPI.getGoogleData();
        }
    }
}
