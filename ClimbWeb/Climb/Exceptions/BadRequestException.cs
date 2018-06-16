using System;

namespace Climb.Exceptions
{
    public class BadRequestException : Exception
    {
        public BadRequestException()
        {
        }

        public BadRequestException(string argumentName, string message)
            : base($"Problem with {argumentName}. {message}")
        {
        }
    }
}