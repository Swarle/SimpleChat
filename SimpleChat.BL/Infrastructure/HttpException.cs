using System.Net;
using Utility.Constants;

namespace SimpleChat.BL.Infrastructure;

public class HttpException : Exception
{
    public HttpException(HttpStatusCode statusCode)
    {
        StatusCode = statusCode;
    }
    public HttpException(HttpStatusCode statusCode, Dictionary<string, string> errorMessages,
        object? result = null)
    {
        StatusCode = statusCode;
        ErrorMessages = errorMessages;
        Result = result;
    }
    public HttpException(HttpStatusCode statusCode, string errorMessage,
        object? result = null)
    {
        StatusCode = statusCode;
        ErrorMessages.Add(SD.DefaultErrorKey, errorMessage);
        Result = result;
    }
    
    public HttpStatusCode StatusCode { get; set; }
    public Dictionary<string, string> ErrorMessages { get; set; } = [];
    public object? Result { get; set; }
}