namespace Project.Exceptions;
public class IdMismatchException : Exception
{
    public IdMismatchException(string message) : base(message) { }
}