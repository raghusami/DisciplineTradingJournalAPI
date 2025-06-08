using DisciplineTradingJournalAPI.DataEntity;
using System.Threading.Tasks;

namespace DisciplineTradingJournalAPI.Contract
{
    public interface ISeriesTracker
    {
        Task AddAsync(SeriesTracker tradingUser);
        
    }
}
