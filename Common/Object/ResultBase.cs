using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Object
{
    public class ResultBase<T>
    {
        /// <summary>
        /// The Status of the Result
        /// </summary>
        public T Status { get; set; }

        /// <summary>
        /// The friendly message fire out
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// idreturn
        /// </summary>
        public int IdReturn { get; set; }

    }
}
