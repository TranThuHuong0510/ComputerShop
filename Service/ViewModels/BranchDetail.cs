using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.ViewModels
{
    public class BranchDetail
    {
        public Guid BranchId { get; set; }
        public string BranchName { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public Guid StorerId { get; set; }
        public string StorerName { get; set; }
    }
}
