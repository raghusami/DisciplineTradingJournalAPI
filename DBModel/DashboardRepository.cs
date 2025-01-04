using DisciplineTradingJournalAPI.Contract;
using DisciplineTradingJournalAPI.DataEntity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DisciplineTradingJournalAPI.DBModel
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly TradingJournalDbContext _context;
        private readonly IPerformanceMetricRepository _performanceMetric;
        private readonly IUserTradesRepository _userTradesRepository;
        public DashboardRepository(TradingJournalDbContext context,
            IPerformanceMetricRepository performanceMetric,
            IUserTradesRepository userTradesRepository)
        {
            _context = context;
            _performanceMetric = performanceMetric;
            _userTradesRepository = userTradesRepository;
        }

        public async Task<Dashboard> GetUserPerformanceDashBoardAsync(int userID)
        {
            var performanceMetric = await _performanceMetric.GetUserPerformanceMetricAsync(userID);

            List<UserTrades> trades = await _userTradesRepository.GetAllAsync(userID);

            var dailyResults = trades
                .GroupBy(t => t.ExitDate)
                .Select(g => new DashboardChartData
                {
                    ExitDate = g.Key,
                    ProfitAndLoss = g.Sum(t => t.NetProfitLoss)
                })
                .OrderBy(data => data.ExitDate)
                .ToList();

            // Calculate the cumulative profit and loss
            decimal? cumulativeProfitAndLoss = 0;
            foreach (var dailyResult in dailyResults)
            {
                cumulativeProfitAndLoss += dailyResult.ProfitAndLoss;
                dailyResult.CumulativeProfitAndLoss = cumulativeProfitAndLoss;
            }

            var finalResult = new Dashboard
            {
                performanceMetric = performanceMetric,
                dashboardChartData = dailyResults
            };
            return finalResult;
        }
    }
}
