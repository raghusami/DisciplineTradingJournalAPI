using DisciplineTradingJournalAPI.Contract;
using DisciplineTradingJournalAPI.DBModel;
using System.Threading.Tasks;

namespace DisciplineTradingJournalAPI.DesignPattern
{

    public class UnitOfWork : IUnitOfWork
    {
        private readonly TradingJournalDbContext _context;

        public ISeriesTrackerRepository SeriesTrackers { get; private set; }
        public ISeriesTradeDetailsRepository SeriesTradeDetails { get; private set; }

        public UnitOfWork(TradingJournalDbContext context)
        {
            _context = context;
            SeriesTrackers = new SeriesTrackerRepository(context);
            SeriesTradeDetails = new SeriesTradeDetailsRepository(context);
        }

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }

}
