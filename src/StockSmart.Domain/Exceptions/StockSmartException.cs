using System;

namespace StockSmart.Domain.Exceptions;

public abstract class StockSmartException(string message, object value) : Exception(message)
{
    public int StatusCode { get; protected set; } = 500;
    public object Value { get; private set; } = value;

}
