using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Abstract
{
    public class AuditTable : IAuditTable
    {
        public string MetaKeyword { get; set; }
        public string MetaDescription { get; set; }
        public bool Active { get; set; }
        public string AddWho { get; set; }
        public string EditWho { get; set; }
        public DateTime AddDate { get; set; }
        public DateTime? EditDate { get; set; }
    }
}
