using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeEffortCore.Exceptions
{
     [Serializable]
    public class NoUserNameException : Exception
    {
        public NoUserNameException()
        {
        }

        public NoUserNameException(string message)
        : base(message)
        {
        }

        public NoUserNameException(string message, Exception inner)
        : base(message, inner)
        {
        }
    }

    [Serializable]
    public class NoUserPasswordException : Exception
    {
        public NoUserPasswordException()
        {
        }

        public NoUserPasswordException(string message)
        : base(message)
        {
        }

        public NoUserPasswordException(string message, Exception inner)
        : base(message, inner)
        {
        }
    }
    [Serializable]
    public class NoUserEmailException : Exception
    {
        public NoUserEmailException()
        {
        }

        public NoUserEmailException(string message)
        : base(message)
        {
        }

        public NoUserEmailException(string message, Exception inner)
        : base(message, inner)
        {
        }
    }
    
}
