using System.Linq.Expressions;

namespace Framework.Domain;

public interface IRepository<TKey, TModel> where TModel : class
{
    TModel Get(TKey id);
    List<TModel> GetAll();
    void Create(TModel entity);
    bool Exists(Expression<Func<TModel, bool>> expression);
    void SaveChanges();
}