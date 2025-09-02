using DataVault.Data;
using DataVault.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataVault.Repositories
{
    public class FeedbackRepository : Repository<Feedback>, IFeedbackRepository
    {
        public FeedbackRepository(ApplicationDbContext context) : base(context) { }

        public async Task<IEnumerable<Feedback>> GetByMonthAsync(int month, int year)
        {
            return await _context.Feedbacks
                .Where(f => f.DataCriacao.Month == month && f.DataCriacao.Year == year)
                .ToListAsync();
        }

        public async Task<IEnumerable<Feedback>> GetByUserAsync(int usuarioId)
        {
            return await _context.Feedbacks
                .Where(f => f.IdUsuario == usuarioId)
                .ToListAsync();
        }
    }
}
