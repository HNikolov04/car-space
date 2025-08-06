namespace CarSpace.Services.Core.Exceptions;

public class ImageProcessingException : Exception
{
    public ImageProcessingException(string message, Exception? inner = null) : base(message, inner) { }
}
