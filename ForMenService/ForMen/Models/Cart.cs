using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ForMen.Models
{
    public class Cart
    {
        public int ID { get; set; }

        public int UserID { get; set; }

        public virtual List<ProductQuantity> Products { get; set; }


        [ForeignKey("UserID")]
        User User { get; set; }
    }
}