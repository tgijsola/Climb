using System;

namespace Climb.Exceptions
{
    public class BadRequestException : Exception
    {
        public readonly string argumentName;

        public BadRequestException()
        {
            argumentName = string.Empty;
        }

        public BadRequestException(string argumentName, string message)
            : base(message)
        {
            this.argumentName = argumentName;
        }
    }
}