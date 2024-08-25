using DisciplineTradingJournalAPI.Contract;
using DisciplineTradingJournalAPI.DataEntity;
using DisciplineTradingJournalAPI.Helper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DisciplineTradingJournalAPI.DBModel
{

    public class UserTradesRepository : IUserTradesRepository
    {
        private readonly TradingJournalDbContext _context;
        private readonly ITradingChargesRepository _trading;
        private readonly IPerformanceMetricRepository _performanceMetric;
        public UserTradesRepository(TradingJournalDbContext context, ITradingChargesRepository trading, IPerformanceMetricRepository performanceMetric)
        {
            _context = context;
            _trading = trading;
            _performanceMetric = performanceMetric;
        }

        public async Task<IEnumerable<UserTrades>> GetAllAsync()
        {
            return await _context.UserTrades.ToListAsync();
        }

        public async Task<UserTrades> GetByIdAsync(int tradeID)
        {
            return await _context.UserTrades.FindAsync(tradeID);
        }

        public async Task<UserTrades> AddAsync(UserTrades userTrade)
        {

            userTrade.CapitalUsed = (userTrade.Quantity * userTrade.EntryPrice);
            _context.UserTrades.Add(userTrade);
            await _context.SaveChangesAsync();
            return userTrade;
        }

        public async Task<UserTrades> UpdateAsync(UserTrades userTrade)
        {
            var userTradeUpdate = await _context.UserTrades
                .FirstOrDefaultAsync(x => x.TradeID == userTrade.TradeID);

            if (userTradeUpdate == null)
            {
                return null;
            }

            decimal entryPrice = userTradeUpdate.EntryPrice;
            decimal exitPrice = userTrade.ExitPrice ?? 0;
            int quantity = userTrade.Quantity;

            userTradeUpdate.ExitDate = userTrade.ExitDate;
            userTradeUpdate.ExitTime = userTrade.ExitTime;
            userTradeUpdate.ExitPrice = userTrade.ExitPrice;
            userTradeUpdate.PositionStatus = Constants.PositionStatusClosed;

            if (userTrade.BrokerCharges > 0)
            {
                userTradeUpdate.BrokerCharges = userTrade.BrokerCharges;
                userTradeUpdate.NetProfitLoss = _trading.ProfitAndLoss(entryPrice, exitPrice, quantity) - userTrade.BrokerCharges;
                userTradeUpdate.Change = _trading.Changes(entryPrice, exitPrice);
            }
            else
            {
                var tradingCharges = userTradeUpdate.MarketType == MarketType.Stock
                    ? _trading.EquityCalculateCharges(entryPrice, exitPrice, quantity)
                    : _trading.FAndOCalculateCharges(entryPrice, exitPrice, quantity);

                userTradeUpdate.BrokerCharges = tradingCharges.TotalCharges;
                userTradeUpdate.NetProfitLoss = tradingCharges.NetPL;
                userTradeUpdate.Change = tradingCharges.Change;
            }

            userTradeUpdate.TradeStatus = userTradeUpdate.NetProfitLoss > 0
                ? Constants.TradeStatusWin
                : Constants.TradeStatusLoss;

            _context.Entry(userTradeUpdate).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            await _performanceMetric.CalculateMetricsAsync(userTradeUpdate.UserID);

            return userTradeUpdate;
        }


        public async Task DeleteAsync(int tradeID)
        {
            var userTrade = await _context.UserTrades.FindAsync(tradeID);
            if (userTrade != null)
            {
                _context.UserTrades.Remove(userTrade);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<UsersPositionsAndPerformanceMetrics> GetUsersOpenPositionsAsync(int userID)
        {
            try
            {
                // Fetching the user's open positions
                var userOpenPositionsResult = await _context.UserTrades
                    .Where(x => x.UserID == userID && x.PositionStatus == Constants.PositionStatusOpen)
                    .Select(x => new UserOpenPositions
                    {
                        TradeID = x.TradeID,
                        UserID = x.UserID,
                        Symbol = x.Symbol,
                        CurrentPrice = x.ExitPrice ?? 0,
                        EntryPrice = x.EntryPrice,
                        Quantity = x.Quantity,
                        ProfitAndLoss = _trading.ProfitAndLoss(x.EntryPrice, x.ExitPrice ?? 0, x.Quantity),
                        Change = _trading.Changes(x.EntryPrice, x.ExitPrice ?? 0)
                    })
                    .ToListAsync();

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
                        TotalCharge = x.TotalCharge 
                    })
                    .FirstOrDefaultAsync();

                // Creating the final result object
                var result = new UsersPositionsAndPerformanceMetrics
                {
                    usersOpenPositions = userOpenPositionsResult,
                    performanceMetric = performanceMetricsResult 
                };
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null; // Return null instead of an empty list for a single object
            }
        }
    }

}
