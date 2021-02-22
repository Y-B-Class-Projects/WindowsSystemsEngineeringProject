using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntities
{
    public class product
    {
        [Required, Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long productID { get; set; }
        public string productName { get; set; }
        public string Photo { get; set; }

        public void DeepCopy(product p)
        {
            Photo = p.Photo;
            productID = p.productID;
            productName = p.productName;
        }
    }
}
