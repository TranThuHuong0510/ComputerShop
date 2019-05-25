using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Models
{
    [Table("Storers")]
    public class Storer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Description { get; set; }

        public bool Active { get; set; }
        public DateTime AddDate { get; set; }
        public DateTime EditDate { get; set; }
        public string AddWho { get; set; }
        public string EditWho { get; set; }
    }
}
