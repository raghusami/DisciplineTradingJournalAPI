using DisciplineTradingJournalAPI.Contract;
using DisciplineTradingJournalAPI.DataEntity;
using System.Threading.Tasks;

namespace DisciplineTradingJournalAPI.DBModel
{
    public class PreTradeChecklistRepository : IPreTradeChecklistRepository
    {
        private readonly TradingJournalDbContext _context;

        public PreTradeChecklistRepository(TradingJournalDbContext dbContext)
        {
            _context = dbContext;
        }
        public async Task AddAsync(PreTradeChecklist preTradeChecklist)
        {
            await _context.PreTradeChecklist.AddAsync(preTradeChecklist);
            await _context.SaveChangesAsync();

        }
    }
}
