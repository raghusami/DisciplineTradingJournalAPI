using DisciplineTradingJournalAPI.Contract;
using DisciplineTradingJournalAPI.DataEntity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DisciplineTradingJournalAPI.DBModel
{
    public class SeriesTradeDetailsRepository : ISeriesTradeDetailsRepository
    {
        private readonly TradingJournalDbContext _context;

        public SeriesTradeDetailsRepository(TradingJournalDbContext context)
        {
            _context = context;
        }
        public async Task AddRangeAsync(IEnumerable<SeriesTradeDetails> trade)
        {
            await _context.SeriesTradeDetails.AddRangeAsync(trade);
        }

    }

}
