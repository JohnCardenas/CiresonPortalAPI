using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CiresonPortalAPI
{
    /// <summary>
    /// Base exception class for all CiresonPortalAPI exceptions
    /// </summary>
    public class CiresonException : Exception
    {
        public CiresonException() { }
        public CiresonException(string message) : base(message) { }
        public CiresonException(string message, Exception inner) : base(message, inner) { }
    }

    /// <summary>
    /// This exception class is thrown when an error occurs from within a Cireson REST request
    /// </summary>
    public class CiresonApiException : CiresonException
    {
        public CiresonApiException() { }
        public CiresonApiException(string message) : base(message) { }
        public CiresonApiException(string message, Exception inner) : base(message, inner) { }
    }

    /// <summary>
    /// This exception class is thrown when trying to write to a read-only property (TypeProjection and derived classes)
    /// </summary>
    public class CiresonReadOnlyException : CiresonException
    {
        public CiresonReadOnlyException() { }
        public CiresonReadOnlyException(string message) : base(message) { }
        public CiresonReadOnlyException(string message, Exception inner) : base(message, inner) { }
    }

    /// <summary>
    /// This exception class is thrown when trying to create an object with duplicate IDs or names
    /// </summary>
    public class CiresonDuplicateItemException : CiresonException
    {
        public CiresonDuplicateItemException() { }
        public CiresonDuplicateItemException(string message) : base(message) { }
        public CiresonDuplicateItemException(string message, Exception inner) : base(message, inner) { }
    }
}
