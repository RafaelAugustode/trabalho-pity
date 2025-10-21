using DataVault.Data;
using DataVault.Models;
using Microsoft.EntityFrameworkCore;

namespace DataVault.Repositories
{
	public class PerfilRepository : Repository<Perfil>, IPerfilRepository
	{
		public PerfilRepository(ApplicationDbContext context) : base(context) { }

		public async Task<IEnumerable<Perfil>> GetPerfisByUsuarioAsync(int userId)
		{
			return await _context.Perfis
				.Where(p => p.IdUsuario == userId)
				.ToListAsync();
		}
	}
}
