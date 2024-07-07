namespace SimpleChat.API.Helpers;

public class OperationFailedResult
{
    public required string ErrorMessage { get; set; }
    public required string MethodName { get; set; }
}