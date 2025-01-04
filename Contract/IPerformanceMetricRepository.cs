using DisciplineTradingJournalAPI.DataEntity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DisciplineTradingJournalAPI.Contract
{
    public interface IPerformanceMetricRepository
    {
        Task<PerformanceMetric> CalculateMetricsAsync(int userID,List<UserTrades> trades);

        Task<UserPerformanceMetric> GetUserPerformanceMetricAsync(int userID);
    }
}
