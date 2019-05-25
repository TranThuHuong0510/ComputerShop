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
    [Table("PostGroups")]
    public class PostGroup : AuditTable
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        [Required]
        [MaxLength(256)]
        public string Alias { get; set; }
        [Required]
        public string Description { get; set; }
        public Guid? ParentId { get; set; }
        public string Image { get; set; }
        public virtual IEnumerable<Post> Posts { get; set; }
    }
}
