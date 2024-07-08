using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.SignalR;
using SimpleChat.BL.Helpers;

namespace SimpleChat.API.Extensions;

public static class HubExtensions
{
    public static void ValidateValue<TEntity>(this Hub hub, TEntity entity)
        where TEntity : class
    {
        var validationResults = new List<ValidationResult>();
        var validationContext = new ValidationContext(entity);

        if (!Validator.TryValidateObject(entity, validationContext, validationResults, true))
        {
            var errorMessages = new Dictionary<string, string>();
            foreach (var validationResult in validationResults)
            {
                foreach (var memberName in validationResult.MemberNames)
                {
                    errorMessages.Add(memberName, validationResult.ErrorMessage);
                }
            }

            throw new HubOperationException(errorMessages);
        }
    }
}