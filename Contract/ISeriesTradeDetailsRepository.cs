using DisciplineTradingJournalAPI.DataEntity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DisciplineTradingJournalAPI.Contract
{
    public interface ISeriesTradeDetailsRepository
    {
        Task AddRangeAsync(IEnumerable<SeriesTradeDetails> trade);

    }
}
