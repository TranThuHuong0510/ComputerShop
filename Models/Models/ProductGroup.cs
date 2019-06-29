using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.Abstract;

namespace Models.Models
{
    [Table("ProductGroup")]
    public class ProductGroup : AuditTable
    {
        [Key]
        public int ID { get; set; }
        [Required]
        public int ProductGroupID { get; set; }
        [Required]
        [MaxLength(200)]
        public string Description { get; set; }
        //public bool Active { get; set; }
        public string Image { get; set; }
        public virtual IEnumerable<Product> Products { get; set; }
    }
}
