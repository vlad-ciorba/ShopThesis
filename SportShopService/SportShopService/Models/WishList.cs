using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace SportShop.Models
{
    public class WishList
    {
        public int ID { get; set; }

        public int UserID { get; set; }

        public virtual List<ProductQuantity> Products { get; set; }

        [ForeignKey("UserID")]
        private User User { get; set; }
    }
}