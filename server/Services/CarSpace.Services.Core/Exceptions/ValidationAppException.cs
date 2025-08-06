namespace CarSpace.Services.Core.Exceptions;

public class ValidationAppException : Exception
{
    public ValidationAppException(string message) : base(message) { }
}
