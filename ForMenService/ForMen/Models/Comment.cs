using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ForMen.Models
{
    public class Comment
    {
        public int ID { get; set; }

        public int ProductID { get; set; }

        public string Text { get; set; }


        [ForeignKey("ProductID")]
        Product Product { get; set; }
    }
}