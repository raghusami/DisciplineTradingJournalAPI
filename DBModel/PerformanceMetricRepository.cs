using DisciplineTradingJournalAPI.Contract;
using DisciplineTradingJournalAPI.DataEntity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DisciplineTradingJournalAPI.DBModel
{
    public class PerformanceMetricRepository : IPerformanceMetricRepository
    {
        private readonly TradingJournalDbContext _context;

        public PerformanceMetricRepository(TradingJournalDbContext context)
        {
            _context = context;
        }

        public async Task<PerformanceMetric> CalculateMetricsAsync(int userID)
        {
            // Retrieve the user's trades
            var trades = await _context.UserTrades
                .Where(x => x.UserID == userID)
                .ToListAsync();

            if (!trades.Any())
                return null; // Return null if there are no trades for the user

            // Initialize performance metric calculations
            int totalTrades = trades.Count;
            int winningTrades = trades.Count(t => t.NetProfitLoss > 0);
            int losingTrades = trades.Count(t => t.NetProfitLoss < 0);

            // Use ternary operator to check for trades
            decimal totalWinAmount = trades
                .Where(t => t.NetProfitLoss > 0)
                .Sum(t => t.NetProfitLoss) ?? 0;
            decimal totalLossAmount = trades
                .Where(t => t.NetProfitLoss < 0)
                .Sum(t => t.NetProfitLoss) ?? 0;

            decimal totalProfitAndLoss = trades.Sum(t => t.NetProfitLoss) ?? 0;

            decimal totalCharge = trades.Sum(t => t.BrokerCharges) ?? 0;

            decimal? averageWin = winningTrades > 0 ? totalWinAmount / winningTrades : null;
            decimal? averageLoss = losingTrades > 0 ? totalLossAmount / losingTrades : null;

            decimal? profitFactor = totalLossAmount != 0 ? totalWinAmount / Math.Abs(totalLossAmount) : null;
            decimal? winRate = totalTrades > 0 ? (decimal)(winningTrades / (decimal)totalTrades) * 100 : null;

            // Prepare the PerformanceMetric object
            var performanceMetric = new PerformanceMetric
            {
                UserID = userID,
                TotalTrades = totalTrades,
                WinningTrades = winningTrades,
                LosingTrades = losingTrades,
                AverageWin = averageWin,
                AverageLoss = averageLoss,
                ProfitFactor = profitFactor,
                WinRate = winRate,
                NetProfitAndLoss = totalProfitAndLoss,
                TotalCharge = totalCharge

            };

            // Check if metrics already exist for the user
            var existingMetric = await _context.PerformanceMetrics
                .FirstOrDefaultAsync(x => x.UserID == userID);

            if (existingMetric != null)
            {
                // Update the existing metrics
                existingMetric.TotalTrades = performanceMetric.TotalTrades;
                existingMetric.WinningTrades = performanceMetric.WinningTrades;
                existingMetric.LosingTrades = performanceMetric.LosingTrades;
                existingMetric.AverageWin = performanceMetric.AverageWin;
                existingMetric.AverageLoss = performanceMetric.AverageLoss;
                existingMetric.ProfitFactor = performanceMetric.ProfitFactor;
                existingMetric.WinRate = performanceMetric.WinRate;
                existingMetric.NetProfitAndLoss = performanceMetric.NetProfitAndLoss;
                existingMetric.TotalCharge = performanceMetric.TotalCharge;
                existingMetric.UpdatedAt = DateTime.Now;

                // Mark the entity as modified
                _context.Entry(existingMetric).State = EntityState.Modified;
            }
            else
            {
                // Add a new metrics entry
                performanceMetric.CreatedAt = DateTime.Now;
                await _context.PerformanceMetrics.AddAsync(performanceMetric);
            }

            // Save changes to the database
            await _context.SaveChangesAsync();
            return performanceMetric;
        }
    }
}
