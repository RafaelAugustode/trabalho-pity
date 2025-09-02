using DataVault.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataVault.Repositories
{
    public interface IFeedbackRepository : IRepository<Feedback>
    {
        Task<IEnumerable<Feedback>> GetByMonthAsync(int month, int year);
        Task<IEnumerable<Feedback>> GetByUserAsync(int usuarioId);
    }
}