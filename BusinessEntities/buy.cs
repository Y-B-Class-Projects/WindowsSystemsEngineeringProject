using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntities
{
    public class buy
    {
        [Key, Column(Order = 0)]
        public int productID { get; set; }

        [Key, Column(Order = 1)]
        public DateTime date { get; set; }

        [Key, Column(Order = 2)]
        public string storeName { get; set; }

        public bool isApproved { get; set; }

        public int amount { get; set; }
        public float price { get; set; }

        public void DeepCopy(buy buy)
        {
            this.productID = buy.productID;
            this.date = buy.date;
            this.amount = buy.amount;
            this.isApproved = buy.isApproved;
            this.price = buy.price;
            this.storeName = buy.storeName;
        }
    }
}