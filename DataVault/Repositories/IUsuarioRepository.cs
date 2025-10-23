using DataVault.Models;

namespace DataVault.Repositories
{
    public interface IUsuarioRepository : IRepository<Usuario>
    {
        Task<Usuario?> GetByEmailAsync(string email);
    }
}