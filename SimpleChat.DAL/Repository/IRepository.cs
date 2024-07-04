using SimpleChat.DAL.Abstract;

namespace SimpleChat.DAL.Repository;

public interface IRepository<TEntity>
{
    Task<TEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<List<TEntity>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<List<TEntity>> GetAllAsync(BaseSpecification<TEntity> specification,
        CancellationToken cancellationToken = default);
    
    Task<TEntity?> GetFirstOrDefaultAsync(BaseSpecification<TEntity> specification,
        CancellationToken cancellationToken = default);

    Task<bool> AnyAsync(BaseSpecification<TEntity> specification, CancellationToken cancellationToken = default);
    Task CreateAsync(TEntity entity);
    Task UpdateAsync(TEntity entity);
    Task DeleteAsync(TEntity entity);
    Task SaveChangesAsync();
}