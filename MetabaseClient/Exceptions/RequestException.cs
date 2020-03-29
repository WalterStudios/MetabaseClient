using System;
using System.Collections.Generic;
using System.Text;

namespace MetabaseClient.Exceptions
{
    public class RequestException : Exception
    {
        public RequestException() { }
        public RequestException(string message) : base(message) { }
        public RequestException(string message, Exception inner) : base(message, inner) { }
    }
}
