using DisciplineTradingJournalAPI.Contract;
using DisciplineTradingJournalAPI.DataEntity;
using System.Threading.Tasks;

namespace DisciplineTradingJournalAPI.DBModel
{
    public class SeriesTrackerRepository : ISeriesTracker
    {
        private readonly TradingJournalDbContext _context;

        public SeriesTrackerRepository(TradingJournalDbContext context)
        {
            _context = context;
        }

       
        public async Task AddAsync(SeriesTracker tracker)
        {
            await _context.SeriesTracker.AddAsync(tracker);
        }
    }

}
