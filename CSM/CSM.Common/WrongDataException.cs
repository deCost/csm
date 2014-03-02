using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSM
{
    public class WrongDataException : ApplicationException
    {

        public WrongDataException()
            : base("")
        {
        }

        public WrongDataException(string message)
            : base(message)
        {
        }

        public WrongDataException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
