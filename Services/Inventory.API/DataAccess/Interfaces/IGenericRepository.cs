using Inventory.API.Domain.Dtos;
using System.Linq.Expressions;

namespace Inventory.API.DataAccess.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        void Add(T entity);
        void AddRange(List<T> entities);
        void Update(T entity);
        void UpdateRange(List<T> entities);
        Task<T> GetById(long id);
        Task<TResult> Max<TResult>(Expression<Func<T, TResult>> where);
        Task<T> GetWhere(Expression<Func<T, bool>> where, params Expression<Func<T, object>>[] include);
        Task<bool> Any(Expression<Func<T, bool>> predicate);
        Task<IdExistsResponseDto> DoesIdsExistInDatabase(List<long> ids, Expression<Func<T, bool>>? filter = null);
        Task<List<T>> GetAll();
        Task<List<T>> GetAll(Expression<Func<T, bool>> predicate);
    }
}
