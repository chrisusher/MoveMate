namespace ChrisUsher.MoveMate.Shared.Exceptions;

public class PropertyAlreadyExistsException : Exception
{
    public PropertyAlreadyExistsException(string message) : base(message)
    {
    }
}
