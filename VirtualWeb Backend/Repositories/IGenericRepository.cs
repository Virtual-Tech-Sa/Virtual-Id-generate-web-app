
namespace VID.Repositories
{
    // This interface defines what operations are available
    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id); // Use Guid for UUID
        Task<T> CreateAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(int id); // Changed from int to Guid for UUID
    }
}