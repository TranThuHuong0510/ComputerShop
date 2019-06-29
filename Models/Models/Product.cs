using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Models.Abstract;

namespace Models.Models
{
    [Table("Products")]
    public class Product : AuditTable
    {
        //[Key]
        //public Guid Id { get; set; }
        //public string Name { get; set; }
        //public string Alias { get; set; }
        //public Guid CategoryId { get; set; }
        //public string Image { get; set; }
        //public XElement MoreImage { get; set; }
        //public decimal Price { get; set; }
        //public decimal Promotion { get; set; }
        //public int Warranty { get; set; }
        //public string Description { get; set; }
        //public int Content { get; set; }
        //public int StatusKey { get; set; }
        //public bool HomeFlag { get; set; }
        //public bool HotFlag { get; set; }
        //public int ViewCount { get; set; }
        public Guid ID { get; set; }
        [Required]
        [MaxLength(20)]
        public string ProductID { get; set; }
        [Required]
        [MaxLength(200)]
        public string DescriptionVN { get; set; }
        [Required]
        [MaxLength(200)]
        public string DescriptionEN { get; set; }
        [Required]
        public int ProductGroupID { get; set; }
    }
}
