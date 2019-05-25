using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Object
{
    public class ObjectResult
    {
        /// <summary>
        /// Status Code
        /// </summary>
        public int StatusCode { get; set; }

        /// <summary>
        /// Message
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Result Object
        /// </summary>
        public object Result { get; set; }
    }
}
