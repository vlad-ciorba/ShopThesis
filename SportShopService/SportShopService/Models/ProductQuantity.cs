using System.ComponentModel.DataAnnotations.Schema;

namespace SportShop.Models
{
    public class ProductQuantity
    {
        public int ID { get; set; }

        public int ProductID { get; set; }

        public int Quantity { get; set; }

        [ForeignKey("ProductID")]
        private Product Product { get; set; }
    }
}