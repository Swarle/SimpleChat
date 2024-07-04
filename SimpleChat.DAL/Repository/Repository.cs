using Microsoft.EntityFrameworkCore;
using SimpleChat.DAL.Abstract;
using SimpleChat.DAL.Extensions;

namespace SimpleChat.DAL.Repository;

public class Repository<TEntity> : IRepository<TEntity> where TEntity : Entity
{
    private readonly DbContext _context;
    protected readonly DbSet<TEntity> DbSet;

    public Repository(DbContext context)
    {
        _context = context;
        DbSet = context.Set<TEntity>();
    }
    
    public async Task<TEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await DbSet.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
    }

    public async Task<List<TEntity>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await DbSet.ToListAsync(cancellationToken);
    }

    public async Task<List<TEntity>> GetAllAsync(BaseSpecification<TEntity> specification, CancellationToken cancellationToken = default)
    {
        IQueryable<TEntity> query = DbSet;

        return await query.ApplySpecification(specification).ToListAsync(cancellationToken);
    }
    
    public async Task<TEntity?> GetFirstOrDefaultAsync(BaseSpecification<TEntity> specification, CancellationToken cancellationToken = default)
    {
        IQueryable<TEntity> query = DbSet;

        return await query.ApplySpecification(specification).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<bool> AnyAsync(BaseSpecification<TEntity> specification, CancellationToken cancellationToken = default)
    {
        IQueryable<TEntity> query = DbSet;

        return await query.ApplySpecification(specification).AnyAsync(cancellationToken);
    }

    public async Task CreateAsync(TEntity entity)
    {
        await DbSet.AddAsync(entity);
    }

    public Task UpdateAsync(TEntity entity)
    {
        DbSet.Update(entity);

        return Task.CompletedTask;
    }

    public Task DeleteAsync(TEntity entity)
    {
        DbSet.Remove(entity);
        
        return Task.CompletedTask;
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}