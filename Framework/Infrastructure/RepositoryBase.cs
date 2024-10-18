using System.Linq.Expressions;
using Framework.Domain;
using Microsoft.EntityFrameworkCore;

namespace Framework.Infrastructure;

public class RepositoryBase<TKey, TModel> :  IRepository<TKey, TModel> where TModel : class
{
    private readonly DbContext _context;

    public RepositoryBase(DbContext context)
    {
        _context = context;
    }

    public TModel Get(TKey id)
    {
        return _context.Find<TModel>(id);
    }

    public List<TModel> GetAll()
    {
        return _context.Set<TModel>().ToList();
    }

    public void Create(TModel entity)
    {
        _context.Add(entity);
    }

    public bool Exists(Expression<Func<TModel, bool>> expression)
    {
        return _context.Set<TModel>().Any(expression);
    }

    public void SaveChanges()
    {
        _context.SaveChanges();
    }
}