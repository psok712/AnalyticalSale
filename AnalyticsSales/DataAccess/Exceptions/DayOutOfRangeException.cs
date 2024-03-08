using System;

namespace DataAccess.Exceptions;

public class DayOutOfRangeException : Exception
{
    public DayOutOfRangeException() { }

    public DayOutOfRangeException(string message)
        : base(message) { }

    public DayOutOfRangeException(string message, Exception inner)
        : base(message, inner) { }
}