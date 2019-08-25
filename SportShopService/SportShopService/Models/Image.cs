using System.ComponentModel.DataAnnotations.Schema;

namespace SportShop.Models
{
    public class Image
    {
        public int ID { get; set; }

        public int ProductID { get; set; }

        public string URL { get; set; }

        [ForeignKey("ProductID")]
        private Product Product { get; set; }
    }

    public class ImageRequest
    {
        public int ProductID { get; set; }

        public string Base64EncodedImage { get; set; }
    }
}