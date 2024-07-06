using Microsoft.AspNetCore.SignalR;
using SimpleChat.DAL.Abstract;
using SimpleChat.DAL.Entities;
using SimpleChat.DAL.Repository;

namespace SimpleChat.BL.Extensions;

public static class RepositoryExtensions
{
    public static async Task<TEntity> CheckIfEntityExist<TEntity>(this IRepository<TEntity> repository, Guid id)
        where TEntity : Entity
    {
        var entity = await repository.GetByIdAsync(id) ?? 
                   throw new HubException($"Entity with type {typeof(TEntity)} and id {id} does not exist");

        return entity;
    }
}