using System;
using System.Collections.Generic;
using System.Text;

namespace UkrywanieWiadomosci.Exceptions
{
    public class BladOperacjiException : Exception
    {
        public BladOperacjiException(string message) : base(message)
        {
        }
    }
}
