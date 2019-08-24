using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ForMen.Models
{
    public class Image
    {
        public int ID { get; set; }

        public int ProductID { get; set; }

        public string URL { get; set; }


        [ForeignKey("ProductID")]
        Product Product { get; set; }
    }

    public class ImageRequest
    {
        public int ProductID { get; set; }

        public string Base64EncodedImage { get; set; }
    }
}