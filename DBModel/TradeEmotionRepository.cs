
using DisciplineTradingJournalAPI.Contract;
using DisciplineTradingJournalAPI.DataEntity;
using System.Threading.Tasks;

namespace DisciplineTradingJournalAPI.DBModel
{
    public class TradeEmotionRepository : ITradeEmotionRepository
    {
         private readonly TradingJournalDbContext _context;
        public TradeEmotionRepository(TradingJournalDbContext dbContext)
        {
            _context = dbContext;
        }

        public Task AddAsync(TradeEmotions tradeEmotion)
        {
           _context.TradeEmotions.Add(tradeEmotion);
            return _context.SaveChangesAsync();
        }
    }
}
