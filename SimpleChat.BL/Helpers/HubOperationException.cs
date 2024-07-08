namespace SimpleChat.BL.Helpers;

public class HubOperationException : Exception
{
    public Dictionary<string, string>? ErrorMessages { get; set; }
    public HubOperationException(string message) : base(message)
    {
        
    }

    public HubOperationException(Dictionary<string, string> errorMessages) =>
        ErrorMessages = errorMessages;
}