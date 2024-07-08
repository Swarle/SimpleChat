namespace SimpleChat.API.Helpers;

public class OperationFailedResult
{
    public Dictionary<string, string>? ErrorMessages { get; set; }
    public string? ErrorMessage { get; set; }
    public required string MethodName { get; set; }
}