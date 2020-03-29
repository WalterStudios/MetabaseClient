using System;
using System.Collections.Generic;
using System.Text;

namespace MetabaseClient.Exceptions
{
    public class AuthenticationException : Exception
    {
        public AuthenticationException() { }
        public AuthenticationException(string message) : base(message) { }
        public AuthenticationException(string message, Exception inner) : base(message, inner) { }
    }
}
