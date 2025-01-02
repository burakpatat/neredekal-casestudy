using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace HotelService.Infrastructure.Repository
{
    public interface IRepository<T> where T : class
    {
        Task<List<T>> GetAllAsync();
        Task<T> GetByIdAsync(Guid id);
        Task<T> GetByIntIdAsync(int id);
        Task CreateAsync(T entity);
        Task AddRangeAsync(IEnumerable<T> entities);
        Task UpdateAsync(T entity);
        Task RemoveAsync(T entity);
        Task RemoveRangeAsync(IEnumerable<T> entities);
        Task<T?> GetByFilterAsync(Expression<Func<T, bool>> filter);
        IQueryable<T> GetWhere(Expression<Func<T, bool>> filter);
        DbSet<T> Table { get; }
        Task<T> GetSingleAsync(Expression<Func<T, bool>> method, bool tracking = true);
        Task<int> SaveAsync();

        ValueTask DisposeAsync();
    }
}
