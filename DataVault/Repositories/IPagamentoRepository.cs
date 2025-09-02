using DataVault.Models;

namespace DataVault.Repositories
{
	public interface IPagamentoRepository : IRepository<Pagamento>
	{
		Task<IEnumerable<Pagamento>> GetPagamentosByUsuarioAsync(int userId);
	}
}
