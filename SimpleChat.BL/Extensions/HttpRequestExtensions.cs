using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Utility.Constants;

namespace SimpleChat.BL.Extensions;

public static class HttpRequestExtensions
{
    public static Guid? GetUserIdHeader(this HttpRequest request)
    {
        var userIdString = request.Headers[SD.UserIdHeaderKey].ToString();

        return Guid.TryParse(userIdString, out var userId) 
            ? userId :  null;
    }

    public static Guid GetUserIdHeaderOrThrow(this HttpRequest request)
    {
        var id = request.GetUserIdHeader() ??
            throw new HubException("UserId is not type of Guid or value does not exist in Header");

        return id;
    }
}