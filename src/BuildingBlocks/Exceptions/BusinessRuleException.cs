using System;

namespace BuildingBlocks.Exceptions
{
    public sealed class BusinessRuleException : Exception
    {
        public BusinessRuleException(string message) : base(message) { }
    }
}
