using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SportShop.Models
{
    public class Product
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public int CategoryID { get; set; }

        public int? DisplayImageID { get; set; }

        public int Quantity { get; set; }

        public string Size { get; set; }

        public string Color { get; set; }

        public DateTime CreatedDate { get; set; }

        [ForeignKey("CategoryID")]
        private Category Category { get; set; }
    }
}