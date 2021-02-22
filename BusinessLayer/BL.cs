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

        

        public product getProduct(long ID)
        {
            return dal.getProduct(ID);
        }

        public List<buy> getGoogleData()
        {
            return dal.getBuys();
        }

        public void updateProduct(product p)
        {
            dal.updateProduct(p);
        }

        public List<buy> getAndUpdateFromGoogleBuys()
        {
            dal.updateFromGoogle();
            return dal.getBuys();
        }

        public void updateBuy(buy b)
        {
            dal.updateBuy(b);
        }
    }
}