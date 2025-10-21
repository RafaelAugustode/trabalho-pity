using DataVault.Data;
using DataVault.Models;
using Microsoft.EntityFrameworkCore;

namespace DataVault.Repositories
{
	public class PagamentoRepository : Repository<Pagamento>, IPagamentoRepository
	{
		public PagamentoRepository(ApplicationDbContext context) : base(context) { }

		public async Task<IEnumerable<Pagamento>> GetPagamentosByUsuarioAsync(int userId)
		{
			return await _context.Pagamentos
				.Where(p => p.IdUsuario == userId)
				.ToListAsync();
		}
	}
}
