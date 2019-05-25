using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Models
{
    [Table("Menus")]
    public class Menu
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        [Required]
        [MaxLength(256)]
        public string URL { get; set; }
        public int? DisplayOrder { get; set; }
        [Required]
        public Guid GroupId { get; set; }
        [ForeignKey("GroupId")]
        public virtual MenuGroup MenuGroup { get; set; }
        [MaxLength(50)]
        public string Target { get; set; }
        public int StatusKey { get; set; }

    }
}
