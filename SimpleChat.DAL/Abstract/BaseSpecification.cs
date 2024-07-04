using System.Linq.Expressions;
using SimpleChat.DAL.Extensions;

namespace SimpleChat.DAL.Abstract;

public abstract class BaseSpecification<TEntity>
{
    public Expression<Func<TEntity, bool>>? Expression { get; private set; }
    public List<string> IncludeString { get; private set; } = [];
    public Expression<Func<TEntity, object>>? OrderBy { get; private set; }
    public Expression<Func<TEntity, object>>? ThenBy { get; private set; }
    public bool IsOrderByDescending { get; private set; } = false;
    public bool IsThenByDescending { get; private set; } = false;


    protected BaseSpecification(Expression<Func<TEntity, bool>> expression)
    {
        Expression = expression;
    }

    protected BaseSpecification() { }

    protected void AddExpression(Expression<Func<TEntity, bool>> expression)
    {
        Expression = Expression == null ? expression 
            : Expression.And(expression);
    }

    protected void AddOrderBy(Expression<Func<TEntity, object>> orderByExpression, bool isDescending = false)
    {
        OrderBy = orderByExpression;
        IsOrderByDescending = isDescending;
    }

    protected void AddThenBy(Expression<Func<TEntity, object>> thenByExpression, bool isDescending = false)
    {
        ThenBy = thenByExpression;
        IsThenByDescending = isDescending;
    }
    
    protected void AddInclude(string include)
    {
        IncludeString.Add(include);
    }
    

}