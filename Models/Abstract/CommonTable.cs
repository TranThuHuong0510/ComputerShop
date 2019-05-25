using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Abstract
{
    public class CommonTable : ICommonTable
    {
        public bool Active { get; set; }
        public string AddWho { get; set; }
        public string EditWho { get; set; }
        public DateTime AddDate { get; set; }
        public DateTime? EditDate { get; set; }
    }
}
