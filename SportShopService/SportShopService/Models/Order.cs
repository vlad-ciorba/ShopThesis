using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace SportShop.Models
{
    public class Order
    {
        public int ID { get; set; }

        public int UserID { get; set; }

        public int StatusID { get; set; }

        public DateTime Date { get; set; }

        public string Address { get; set; }

        public virtual List<ProductQuantity> Products { get; set; }

        [ForeignKey("UserID")]
        private User User { get; set; }

        [ForeignKey("StatusID")]
        private OrderStatus OrderStatus { get; set; }
    }
}