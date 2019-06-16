using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.ViewModels
{
    public class BranchDetailViewModel
    {
        public Guid? BranchId { get; set; }
        [Required(ErrorMessage = "{0} is required")]
        [StringLength(100, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 10)]
        public string BranchName { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public IEnumerable<StorerViewModel> Storers { get; set; }
    }

    public class StorerViewModel
    {
        public Guid? StorerId { get; set; }
        public string StorerName { get; set; }
    }
}
