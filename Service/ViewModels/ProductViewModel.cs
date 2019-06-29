using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.ViewModels
{
    public class ProductViewModel
    {
        //public Guid? BranchId { get; set; }
        //[Required(ErrorMessage = "{0} is required")]
        //[StringLength(100, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 10)]
        //public string BranchName { get; set; }
        //public string Address { get; set; }
        //public string Phone { get; set; }
        //public IEnumerable<StorerViewModel> Storers { get; set; }
        [Required(ErrorMessage ="{0} is required")]
        [MaxLength(20)]
        [StringLength(20, ErrorMessage ="{0} length must be <=20")]
        public string ProductID { get; set; }
        [Required]
        [MaxLength(200)]
        [StringLength(200, ErrorMessage = "{0} length must be <=200")]
        public string DescriptionVN { get; set; }
        [Required]
        [MaxLength(200)]
        [StringLength(200, ErrorMessage = "{0} length must be <=200")]
        public string DescriptionEN { get; set; }
        [Required(ErrorMessage ="{0} is required")]
        public int ProductGroupID { get; set; }
    }
}
