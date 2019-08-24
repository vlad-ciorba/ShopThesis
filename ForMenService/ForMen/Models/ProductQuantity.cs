using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ForMen.Models
{
    public class ProductQuantity
    {
        public int ID { get; set; }

        public int ProductID { get; set; }

        public int Quantity { get; set; }


        [ForeignKey("ProductID")]
        Product Product { get; set; }
    }
}