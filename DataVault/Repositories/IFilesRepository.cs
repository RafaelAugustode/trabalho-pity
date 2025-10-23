using DataVault.Models;

namespace DataVault.Repositories
{
	public interface IFilesRepository : IRepository<Files>
	{
		Task<IEnumerable<Files>> GetFilesByUserIdAsync(int userId);
        Task<long> GetTotalStorageUsedAsync(int userId);
    }
}
