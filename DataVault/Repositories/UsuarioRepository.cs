using DataVault.Data;
using DataVault.Models;
using Microsoft.EntityFrameworkCore;

namespace DataVault.Repositories
{
    public class UsuarioRepository : Repository<Usuario>, IUsuarioRepository
    {
        public UsuarioRepository(ApplicationDbContext context) : base(context) { }

        public async Task<Usuario?> GetByEmailAsync(string email)
        {
            return await _context.Usuario
                .FirstOrDefaultAsync(u => u.Email == email);
        }
    }
}