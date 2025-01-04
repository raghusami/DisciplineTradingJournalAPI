using DisciplineTradingJournalAPI.Contract;
using DisciplineTradingJournalAPI.DataEntity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
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

        public async Task<PerformanceMetric> CalculateMetricsAsync(int userID, List<UserTrades> trades)
        {
            if (!trades.Any())
                return null; // Return null if there are no trades for the user

            // Initialize performance metric calculations
            int totalTrades = trades.Count;
            int winningTrades = trades.Count(t => t.NetProfitLoss > 0);
            int losingTrades = trades.Count(t => t.NetProfitLoss < 0);

            // Summation
            decimal? totalWinAmount = trades.Where(t => t.NetProfitLoss > 0).Sum(t => t.NetProfitLoss);
            decimal? totalLossAmount = trades.Where(t => t.NetProfitLoss < 0).Sum(t => t.NetProfitLoss);
            decimal? totalProfitAndLoss = trades.Sum(t => t.NetProfitLoss);
            decimal? totalCharge = trades.Sum(t => t.BrokerCharges);

            decimal? averageWin = winningTrades > 0 ? totalWinAmount / winningTrades : 0;
            decimal? averageLoss = losingTrades > 0 ? totalLossAmount / losingTrades : 0;
            decimal? profitFactor = totalLossAmount != 0 ? totalWinAmount / Math.Abs((decimal)totalLossAmount) : 0;
            decimal? winRate = totalTrades > 0 ? (decimal)(winningTrades / (decimal)totalTrades) * 100 : 0;

            // Calculate daily win/loss count
            var dailyResults = trades.GroupBy(t => t.ExitDate)
                .Select(g => new
                {
                    Date = g.Key,
                    DailyNetProfitLoss = g.Sum(t => t.NetProfitLoss)
                }).ToList();

            int totalDayTrades = dailyResults.Count;
            int winningDays = dailyResults.Count(d => d.DailyNetProfitLoss > 0);
            int losingDays = dailyResults.Count(d => d.DailyNetProfitLoss < 0);
            decimal? dayWinRate = totalDayTrades > 0 ? (decimal)(winningDays / (decimal)totalDayTrades) * 100 : null;

            decimal? netProfitLossWeight = (totalProfitAndLoss / totalTrades);

            // Calculate max drawdown
            decimal? cumulativeProfit = 0;
            decimal? peak = 0;
            decimal? maxDrawdown = 0;

            foreach (var trade in trades)
            {
                cumulativeProfit += trade.NetProfitLoss;

                if (cumulativeProfit > peak)
                {
                    peak = cumulativeProfit;
                }

                var drawdown = peak - cumulativeProfit;

                if (drawdown > maxDrawdown)
                {
                    maxDrawdown = drawdown;
                }
            }

            //// Calculate weighted score
            //decimal overallScore = (0.30m * (decimal)winRate) +
            //                       (0.30m * (decimal)profitFactor) +
            //                       (0.20m * (decimal)netProfitLossWeight) +
            //                       (0.10m * (decimal)maxDrawdown) +
            //                       (0.10m * (decimal)dayWinRate);

            //// Cap the score at 100
            //overallScore = Math.Min(overallScore, 100);

            // Normalize values
            decimal normalizedWinRate = (decimal)winRate / 100m; // Normalize to 0-1 range
            decimal normalizedProfitFactor = (decimal)profitFactor / 5.00m; // Normalize assuming max value of 5
            decimal normalizedNetProfitPerTrade =(decimal)netProfitLossWeight / 1200m; // Normalize assuming max value of $5000
            decimal normalizedMaxDrawDown =(decimal)maxDrawdown / 20000m; // Normalize assuming max value of $20000
            decimal normalizedDayWinRate =(decimal) dayWinRate / 100m; // Normalize to 0-1 range

            // Calculate components
            decimal winRateComponent = 0.30m * normalizedWinRate * 100; // Scale back to 0-100 range
            decimal profitFactorComponent = 0.30m * normalizedProfitFactor * 100; // Scale back to 0-100 range
            decimal netProfitPerTradeComponent = 0.20m * normalizedNetProfitPerTrade * 100; // Scale back to 0-100 range
            decimal maxDrawDownComponent = 0.10m * (1 - normalizedMaxDrawDown) * 100; // Inverse relation (lower is better)
            decimal dayWinRateComponent = 0.10m * normalizedDayWinRate * 100; // Scale back to 0-100 range

            // Sum components and cap at 100
            decimal overallScore = winRateComponent + profitFactorComponent + netProfitPerTradeComponent + maxDrawDownComponent + dayWinRateComponent;
            decimal cappedScore = Math.Min(overallScore, 100);



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
                TotalCharge = totalCharge,
                WinningDays = winningDays,
                LosingDays = losingDays,
                MaxDrawDown = maxDrawdown,
                DayWinRate = dayWinRate,
                TradingScore = overallScore
            };

            // Check if metrics already exist for the user
            var existingMetric = await _context.PerformanceMetrics.FirstOrDefaultAsync(x => x.UserID == userID);

            if (existingMetric != null)
            {
                // Update existing metrics
                existingMetric.TotalTrades = performanceMetric.TotalTrades;
                existingMetric.WinningTrades = performanceMetric.WinningTrades;
                existingMetric.LosingTrades = performanceMetric.LosingTrades;
                existingMetric.AverageWin = performanceMetric.AverageWin;
                existingMetric.AverageLoss = performanceMetric.AverageLoss;
                existingMetric.ProfitFactor = performanceMetric.ProfitFactor;
                existingMetric.WinRate = performanceMetric.WinRate;
                existingMetric.NetProfitAndLoss = performanceMetric.NetProfitAndLoss;
                existingMetric.TotalCharge = performanceMetric.TotalCharge;
                existingMetric.WinningDays = performanceMetric.WinningDays;
                existingMetric.LosingDays = performanceMetric.LosingDays;
                existingMetric.MaxDrawDown = performanceMetric.MaxDrawDown;
                existingMetric.DayWinRate = performanceMetric.DayWinRate;
                existingMetric.TradingScore = performanceMetric.TradingScore;
                existingMetric.UpdatedAt = DateTime.Now;

                _context.Entry(existingMetric).State = EntityState.Modified;
            }
            else
            {
                // Add new metrics entry
                performanceMetric.CreatedAt = DateTime.Now;
                await _context.PerformanceMetrics.AddAsync(performanceMetric);
            }

            // Save changes to the database
            await _context.SaveChangesAsync();
            return performanceMetric;
        }


        public async Task<UserPerformanceMetric> GetUserPerformanceMetricAsync(int userID)
        {
            // Fetching the user's performance metrics
            var performanceMetricsResult = await _context.PerformanceMetrics
                    .Where(x => x.UserID == userID)
                    .Select(x => new UserPerformanceMetric
                    {
                        UserID = x.UserID,
                        TotalTrades = x.TotalTrades ?? 0,
                        WinningTrades = x.WinningTrades ?? 0,
                        LosingTrades = x.LosingTrades ?? 0,
                        AverageWin = x.AverageWin ?? 0,
                        AverageLoss = x.AverageLoss ?? 0,
                        WinRate = x.WinRate ?? 0,
                        NetProfitAndLoss = x.NetProfitAndLoss ?? 0,
                        TotalCharge = x.TotalCharge,
                        ProfitFactor = x.ProfitFactor ?? 0,
                        WinningDays = x.WinningDays ?? 0,
                        LosingDays = x.LosingDays ?? 0,
                        MaxDrawDown = x.MaxDrawDown ?? 0,
                        DayWinRate = x.DayWinRate ?? 0,
                        TradingScore = x.TradingScore ?? 0
                    })
                    .FirstOrDefaultAsync();

            return performanceMetricsResult;
        }
    }
}
