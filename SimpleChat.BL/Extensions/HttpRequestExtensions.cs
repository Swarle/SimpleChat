using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using SimpleChat.BL.Infrastructure;
using Utility.Constants;

namespace SimpleChat.BL.Extensions;

public static class HttpRequestExtensions
{
    public static Guid? GetUserId(this HttpRequest request)
    {
        var userIdString = request.Headers[SD.UserIdHeaderKey].ToString();

        return Guid.TryParse(userIdString, out var userId) 
            ? userId :  null;
    }

    public static Guid GetUserIdOrThrowHubException(this HttpRequest request)
    {
        var id = request.GetUserId() ??
            throw new HubException("UserId is not type of Guid or value does not exist in Header");

        return id;
    }
    
    public static Guid GetUserIdOrThrowHttpException(this HttpRequest request)
    {
        var id = request.GetUserId() ??
                 throw new HttpException(HttpStatusCode.Unauthorized,"UserId is not type of Guid or value does not exist in Header");
    
        return id;
    }
}