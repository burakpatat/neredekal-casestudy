using HotelService.Infrastructure.Repository;

namespace HotelService.Infrastructure.UnitOfWork
{
    public interface IUnitOfWork : IAsyncDisposable
    {
        IRepository<T> GetRepository<T>() where T : class;
        TRepository GetCustomRepository<TRepository>() where TRepository : class;
        Task<int> SaveChangesAsync();
    }
}
