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
    [Table("Orders")]
    public class Order : CommonTable
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        [MaxLength(256)]
        public string CustomerName { get; set; }
        [Required]
        [MaxLength(256)]
        public string CustomerAddress { get; set; }
        [Required]
        [MaxLength(256)]
        public string CustomerEMail { get; set; }
        [Required]
        [MaxLength(256)]
        public string CustomerMobile { get; set; }
        [Required]
        [MaxLength(256)]
        public string CustomerMessage { get; set; }
        [Required]
        [MaxLength(256)]
        public string PaymenMethod { get; set; }
        [Required]
        public int StatusKey { get; set; }

        public virtual IEnumerable<OrderDetail> OrderDetails { get; set; }
    }
}
