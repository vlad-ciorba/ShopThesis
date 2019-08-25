using System.ComponentModel.DataAnnotations.Schema;

namespace SportShop.Models
{
    public class Category
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public int? ParentID { get; set; }

        [ForeignKey("ParentID")]
        private Category CategoryFK { get; set; }
    }
}