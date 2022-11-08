namespace TodoManager.Api.Service;

public class TodoValidationException : Exception
{
    public readonly IDictionary<string, string[]> Errors 
        = new Dictionary<string, string[]>();
    public TodoValidationException() { }
    public TodoValidationException(string message) : base(message) { }
    public TodoValidationException(string message, Exception inner)
        : base(message, inner) { }
}