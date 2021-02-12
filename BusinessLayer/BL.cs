using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer;
using BusinessEntities;


namespace BusinessLayer
{
    public class BL
    {
        DataLayer.DAL dal;
        public BL()
        {
            dal = new DAL();
        }

        public void addProduct(product p)
        {
            dal.addProduct(p);
        }

        public string getGoogleData()
        {
            return GoogleAPI.getGoogleData();
        }
    }
}
