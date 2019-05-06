using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Models
{
    public class CompanyInformation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string CompanyID { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public string TaxCode { get; set; }
        public string Phone { get; set; }
        public string Director { get; set; }
        public string ChiefAccountant { get; set; }
        public string CompanyType { get; set; }

        public bool Active { get; set; }
        public DateTime AddDate { get; set; }
        public DateTime EditDate { get; set; }
        public string AddWho { get; set; }
        public string EditWho { get; set; }
    }
}
