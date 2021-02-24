using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer;
using BusinessEntities;
using Microsoft.ML;
using Microsoft.ML.Trainers;
using System.Diagnostics;
using System.Collections;
using Accord.MachineLearning.Rules;

namespace BusinessLayer
{
    public class BL
    {
        DAL dal;
        public BL()
        {
            dal = new DAL();
        }

        public List<AssociationRule<int>> AI()
        {
            Trace.WriteLine("Starting AI:");

            var allBuys = dal.getApprovedBuys().ToArray();

            int[][] dataset = new int[allBuys.Count()][];

            for (int i = 0; i < allBuys.Count(); i++)
            {
                dataset[i] = new int[] {(int)allBuys[i].date.DayOfWeek, (int)allBuys[i].productID};
            }

            Apriori apriori = new Apriori(threshold: 3, confidence: 0.2);

            AssociationRuleMatcher<int> classifier = apriori.Learn(dataset);

            AssociationRule<int>[] rules = classifier.Rules;
 
            return rules.ToList();
        }



        public product getProduct(long ID)
        {
            return dal.getProduct(ID);
        }

        public List<buy> getGoogleData()
        {
            var ret = dal.getApprovedBuys();
            ret.AddRange(dal.getUnapprovedBuys());
            return ret;
        }

        public IEnumerable getProducts()
        {
            return dal.getProducts();
        }

        public void updateProduct(product p)
        {
            dal.updateProduct(p);
        }

        public List<buy> getAndUpdateFromGoogleBuys()
        {
            dal.updateFromGoogle();
            return getGoogleData();
        }

        public void updateUnappruvedBuy(buy b)
        {
            dal.updateUnappruvedBuy(b);
        }

        public void updateAppruvedBuy(buy b)
        {
            dal.updateAppruvedBuy(b);
        }


        public void appruveBuy(buy buy)
        {
            dal.deleteUnapprovedBuy(buy);
            dal.addApprovedBuy(buy);
        }



        public List<buy> getBuys()
        {
            return getGoogleData();
        }

        public List<string> getStoresNames()
        {
            return dal.getStoresNames();
        }

        public void deleteBuy(buy buy)
        {
            dal.deleteUnapprovedBuy(buy);
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
                ret[(int)buy.date.DayOfWeek] += buy.price;
            }
            return ret;
        }

        public double[] getProductWeek(product product)
        {
            double[] ret = new double[7];

            foreach (var buy in getApprovedBuys())
            {
                if(product.productID == buy.productID)
                    ret[(int)buy.date.DayOfWeek] += buy.amount;
            }
            return ret;
        }

        public double[] getWeekProductsCount()
        {
            double[] ret = new double[7];

            foreach (var buy in getApprovedBuys())
            {
                ret[(int)buy.date.DayOfWeek] += buy.amount;
            }
            return ret;
        }

        public List<buy> getApprovedBuys()
        {
            return dal.getApprovedBuys();                  
        }


        public List<buy> getUnapprovedBuys()
        {
            return dal.getUnapprovedBuys();
        }


        public List<StoreCount> GetStoreCounts()
        {
            List<StoreCount> ret = new List<StoreCount>();

            var storeNames = dal.getStoresNames();

            int[] storesCount = new int[storeNames.Count()];


            foreach (var buy in getBuys())
            {
                storesCount[storeNames.IndexOf(buy.storeName)] += buy.amount;
            }

            for (int i = 0; i < storeNames.Count(); i++)
            {
                ret.Add(new StoreCount() { storeName = storeNames[i], count = storesCount[i] });
            }

            return ret;
        }

        public double[] getStoreWeek(string storeName)
        {
            double[] ret = new double[7];

            foreach (var buy in getApprovedBuys())
            {
                if (storeName == buy.storeName)
                    ret[(int)buy.date.DayOfWeek - 1] += buy.amount;
            }
            return ret;
        }

        public int[,] getCommonProducts()
        {
            int[,] ret = new int[5,2];

            var buys = dal.getApprovedBuys();

            if(buys.Count() > 5)
            {
                int[] count = new int[buys.Count];
                int[] products = new int[buys.Count]; ;

                for (int i = 0; i < buys.Count(); i++)
                {
                    count[i] = buys[i].productID;
                    products[i] = buys.Select(buy => buy.productID == buys[i].productID).Count(); ;
                }

                Array.Sort(count, products);

                for (int i = 0; i < 5; i++)
                {
                    ret[i, 0] = products[i];
                    ret[i, 1] = count[i];
                }

                return ret;
            }

            return null;
        }
    }
}