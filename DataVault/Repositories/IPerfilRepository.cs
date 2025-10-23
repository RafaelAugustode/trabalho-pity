using DataVault.Models;

namespace DataVault.Repositories
{
	public interface IPerfilRepository : IRepository<Perfil>
	{
		Task<IEnumerable<Perfil>> GetPerfisByUsuarioAsync(int userId);
	}
}
