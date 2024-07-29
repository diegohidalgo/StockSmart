using System;

namespace StockSmart.Domain.Exceptions
{
    public abstract class StockSmartException : Exception
    {
        public StockSmartException(string message, object value) : base(message)
        {
            Value = value;
        }

        public int StatusCode { get; protected set; } = 500;
        public object Value { get; private set; }

    }
}
