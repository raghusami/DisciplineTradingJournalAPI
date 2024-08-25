using DisciplineTradingJournalAPI.DataEntity;
using System.Threading.Tasks;

namespace DisciplineTradingJournalAPI.Contract
{
    public interface IPerformanceMetricRepository
    {
        Task<PerformanceMetric> CalculateMetricsAsync(int userID);
    }
}
