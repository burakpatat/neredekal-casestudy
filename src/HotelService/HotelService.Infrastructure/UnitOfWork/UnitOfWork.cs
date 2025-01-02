using HotelService.Infrastructure.Persistence;
using HotelService.Infrastructure.Repository;


namespace HotelService.Infrastructure.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly HotelDbContext _context;
        private readonly Dictionary<Type, object> _repositories = new();

        public UnitOfWork(HotelDbContext context)
        {
            _context = context;
        }

        public IRepository<T> GetRepository<T>() where T : class
        {
            var type = typeof(T);
            if (!_repositories.ContainsKey(type))
            {
                var repositoryInstance = new Repository<T>(_context);
                _repositories[type] = repositoryInstance;
            }
            return (IRepository<T>)_repositories[type];
        }
        public TRepository GetCustomRepository<TRepository>() where TRepository : class
        {
            var type = typeof(TRepository);
            if (!_repositories.ContainsKey(type))
            {
                var repositoryInstance = Activator.CreateInstance(type, _context);
                _repositories[type] = repositoryInstance;
            }
            return (TRepository)_repositories[type];
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async ValueTask DisposeAsync()
        {
            await _context.DisposeAsync();
        }
    }

}
