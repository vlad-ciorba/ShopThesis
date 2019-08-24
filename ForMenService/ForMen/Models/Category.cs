using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ForMen.Models
{
    public class Category
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public int? ParentID { get; set; }


        [ForeignKey("ParentID")]
        Category CategoryFK { get; set; }
    }
}