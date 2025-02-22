﻿using System.Net;
using Microsoft.AspNetCore.SignalR;
using SimpleChat.BL.Helpers;
using SimpleChat.BL.Infrastructure;
using SimpleChat.DAL.Abstract;
using SimpleChat.DAL.Entities;
using SimpleChat.DAL.Repository;

namespace SimpleChat.BL.Extensions;

public static class RepositoryExtensions
{
    public static async Task<TEntity> GetByIdOrThrowHubOperationExceptionAsync<TEntity>(this IRepository<TEntity> repository, Guid id)
        where TEntity : Entity
    {
        var entity = await repository.GetByIdAsync(id) ?? 
                   throw new HubOperationException($"Entity with type {typeof(TEntity)} and does not exist");
    
        return entity;
    }
}