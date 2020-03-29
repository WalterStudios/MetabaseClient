using System;
using System.Collections.Generic;
using System.Text;

namespace MetabaseClient.Exceptions
{
    public class TableResponseException : Exception
    {
        public TableResponseException() { }
        public TableResponseException(string message) : base(message) { }
        public TableResponseException(string message, Exception inner) : base(message, inner) { }
    }
}
