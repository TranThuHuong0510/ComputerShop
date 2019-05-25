using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Abstract
{
    public interface IAuditTable : ICommonTable
    {
        string MetaKeyword { get; set; }
        string MetaDescription { get; set; }
    }
}
