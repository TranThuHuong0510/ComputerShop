using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Abstract
{
    public interface ICommonTable
    {
        bool Active { get; set; }
        string AddWho { get; set; }
        string EditWho { get; set; }
        DateTime? AddDate { get; set; }
        DateTime? EditDate { get; set; }
    }
}
