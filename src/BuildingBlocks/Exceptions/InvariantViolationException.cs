using System;

namespace BuildingBlocks.Exceptions
{
    public class InvariantViolationException : Exception
    {
        public InvariantViolationException(string message) : base(message) { }
    }
}
