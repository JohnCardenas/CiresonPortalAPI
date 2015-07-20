using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CiresonPortalAPI
{
    /// <summary>
    /// This exception class is thrown when an error occurs from within a Cireson REST request
    /// </summary>
    public class CiresonApiException : Exception
    {
        public CiresonApiException() { }
        public CiresonApiException(string message) : base(message) { }
        public CiresonApiException(string message, Exception inner) : base(message, inner) { }
    }
}
