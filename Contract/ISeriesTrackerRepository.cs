using DisciplineTradingJournalAPI.DataEntity;
using System.Threading.Tasks;

namespace DisciplineTradingJournalAPI.Contract
{
    public interface ISeriesTrackerRepository
    {
        Task AddAsync(SeriesTracker tradingUser);
        
    }
}
