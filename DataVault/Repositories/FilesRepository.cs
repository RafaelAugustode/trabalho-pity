using DataVault.Data;
using DataVault.Models;
using Microsoft.EntityFrameworkCore;

namespace DataVault.Repositories
{
	public class FilesRepository : Repository<Files>, IFilesRepository
	{
		public FilesRepository(ApplicationDbContext context) : base(context) { }

		public async Task<IEnumerable<Files>> GetFilesByUserIdAsync(int userId)
		{
			return await _context.Files
				.Where(f => f.IdUsuario == userId)
				.ToListAsync();
		}
	}
}
