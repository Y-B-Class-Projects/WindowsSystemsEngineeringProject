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
        DAL dal;
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

        public List<buy> getBuys()
        {
            return dal.getBuys();
        }

        public List<string> getStoresNames()
        {
            return dal.getStoresNames();
        }

        public void deleteBuy(buy buy)
        {
            dal.deleteBuy(buy);
        }

        public List<string> getProductsToBuyNames(string storeName)
        {
            return dal.getProductsToBuyNames(storeName);
        }

        public double[] getMonthPrice()
        {
            double[] ret = new double[12];

            foreach (var buy in getApprovedBuys())
            {
                ret[buy.date.Month - 1] += buy.price;
            }
            return ret;
        }

        public double[] getMonthProductsCount()
        {
            double[] ret = new double[12];

            foreach (var buy in getApprovedBuys())
            {
                ret[buy.date.Month - 1] += buy.amount;
            }
            return ret;
        }


        public double[] getWeekPrice()
        {
            double[] ret = new double[7];

            foreach (var buy in getApprovedBuys())
            {
                ret[(int)buy.date.DayOfWeek - 1] += buy.price;
            }
            return ret;
        }

        public double[] getWeekProductsCount()
        {
            double[] ret = new double[7];

            foreach (var buy in getApprovedBuys())
            {
                ret[(int)buy.date.DayOfWeek - 1] += buy.amount;
            }
            return ret;
        }

        public List<buy> getApprovedBuys()
        {
            return (from buy in dal.getBuys()
                    where buy.isApproved == true
                    select buy).ToList();                   
        }


        public List<buy> getUnapprovedBuys()
        {
            return (from buy in dal.getBuys()
                    where buy.isApproved == false
                    select buy).ToList();
        }


        public List<StoreCount> GetStoreCounts()
        {
            List<StoreCount> ret = new List<StoreCount>();

            var storeNames = dal.getStoresNames();

            int[] storesCount = new int[storeNames.Count()];


            foreach (var buy in dal.getBuys())
            {
                storesCount[storeNames.IndexOf(buy.storeName)] += buy.amount;
            }

            for (int i = 0; i < storeNames.Count(); i++)
            {
                ret.Add(new StoreCount() { storeName = storeNames[i], count = storesCount[i] });
            }

            return ret;
        }
    }
}