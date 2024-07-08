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
            var errorResponse = new OperationFailedResult{ MethodName = invocationContext.HubMethodName };

            if (ex.ErrorMessages is null)
                errorResponse.ErrorMessage = ex.Message;
            else
                errorResponse.ErrorMessages = ex.ErrorMessages; 

            await invocationContext.Hub.Clients.Caller.SendAsync(HubMethodNames.Error, errorResponse);

            return errorResponse;
        }
        catch (Exception ex)
        {
            throw new HubException(ex.Message);
        }
    }
}