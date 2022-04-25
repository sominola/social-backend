namespace Social.Domain.Exceptions;

public class ValidationException:Exception
{
    public readonly IEnumerable<string> Errors;

    public ValidationException(string message, IEnumerable<string> errors) : base(message)
    {
        Errors = errors;
    }
}

