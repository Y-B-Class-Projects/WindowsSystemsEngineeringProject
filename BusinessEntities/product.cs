using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntities
{
    public class product
    {
        public int productID { get; set; }
        public string productName { get; set; }
        public int productCost { get; set; }
        public string Photo { get; set; }

        public product(int productID, string productName, int productCost, string Photo)
        {
            this.productID = productID;
            this.productName = productName;
            this.productCost = productCost;
            this.Photo = Photo;
        }
    }
}
