using Microsoft.AspNetCore.SignalR;
using SimpleChat.API.Constants;
using SimpleChat.API.Helpers;
using SimpleChat.BL.Helpers;

namespace SimpleChat.API.Filters;

public class HubExceptionFilter : IHubFilter
{
    public async ValueTask<object?> InvokeMethodAsync(HubInvocationContext invocationContext, Func<HubInvocationContext, ValueTask<object?>> next)
    {
        try
        {
            return await next(invocationContext);
        }
        catch (HubOperationException ex)
        {
            var errorResponse = new OperationFailedResult { ErrorMessage = ex.Message,
                MethodName = invocationContext.HubMethodName};
            
            await invocationContext.Hub.Clients.Caller.SendAsync(HubMethodNames.Error, errorResponse);

            return errorResponse;
        }
    }
}