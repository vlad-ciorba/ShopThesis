using System.ComponentModel.DataAnnotations.Schema;

namespace SportShop.Models
{
    public class Comment
    {
        public int ID { get; set; }

        public int ProductID { get; set; }

        public string Text { get; set; }

        [ForeignKey("ProductID")]
        private Product Product { get; set; }
    }
}