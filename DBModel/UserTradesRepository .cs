using DisciplineTradingJournalAPI.Contract;
using DisciplineTradingJournalAPI.DataEntity;
using DisciplineTradingJournalAPI.Helper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        public async Task<List<UserTrades>> GetAllAsync(int userID)
        {
            // Retrieve the user's trades
            var trades = await _context.UserTrades
                .Where(x => x.UserID == userID)
                .OrderBy(t => t.ExitDate) 
                .ToListAsync();
            return trades;
        }
        public async Task<UserTrades> GetByIdAsync(int tradeID)
        {
            return await _context.UserTrades.FindAsync(tradeID);
        }

        public async Task<UserTrades> AddAsync(UserTrades userTrade)
        {
            try
            {


                userTrade.CapitalUsed = (userTrade.Quantity * userTrade.EntryPrice);
                _context.UserTrades.Add(userTrade);
                await _context.SaveChangesAsync();
            }
            catch(Exception ex)
            {

            }
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

            var trades = await GetAllAsync(userTradeUpdate.UserID);

            await _performanceMetric.CalculateMetricsAsync(userTradeUpdate.UserID, trades);

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
        
        public async Task<List<UserOpenPositions>> GetUsersOpenPositionsAsync(int userID)
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

            return userOpenPositionsResult;
        }
        public async Task<List<UserClosePositions>> GetUsersClosedPositionsAsync(int userID)
        {

            // Fetching the user's closed positions
            var userClosedPositionsResult = await _context.UserTrades
                .Where(x => x.UserID == userID && x.PositionStatus == Constants.PositionStatusClosed)
                .Select(x => new UserClosePositions
                {
                    TradeID = x.TradeID,
                    UserID = x.UserID,
                    Symbol = x.Symbol,
                    ExitPrice = x.ExitPrice ?? 0,
                    EntryPrice = x.EntryPrice,
                    Quantity = x.Quantity,
                    ProfitAndLoss = x.NetProfitLoss,
                    Change = x.Change,
                    BrokerCharges = x.BrokerCharges,
                    TradeStatus = x.TradeStatus,
                    MarketType = x.MarketType,
                    TradeType = x.TradeType,
                    HoldingDays = x.ExitDate.HasValue
                    ? (x.ExitDate.Value - x.EntryDate).TotalDays : 0,
                    TradeEntryDate = x.ExitDate,
                    InvestmentAmount = x.CapitalUsed,
                    NetROI = ((x.NetProfitLoss / x.CapitalUsed) * 100)
                })
                .ToListAsync();

            return userClosedPositionsResult;
        }
        public async Task<UsersClosedPositionsAndPerformanceMetrics> GetUsersClosedPositionsWithTradeMetricAsync(int userID)
        {
            try
            {
                var userClosePositions = await GetUsersClosedPositionsAsync(userID);
                var performanceMetric = await _performanceMetric.GetUserPerformanceMetricAsync(userID);

                var result = new UsersClosedPositionsAndPerformanceMetrics
                {
                    userClosePositions = userClosePositions,
                    performanceMetric = performanceMetric
                };
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null; // Return null instead of an empty list for a single object
            }
        }
        public async Task<UsersPositionsAndPerformanceMetrics> GetUsersOpenPositionsWithTradeMetricAsync(int userID)
        {
            try
            {
                var usersOpenPositions = await GetUsersOpenPositionsAsync(userID);
                var performanceMetric = await _performanceMetric.GetUserPerformanceMetricAsync(userID);

                var result = new UsersPositionsAndPerformanceMetrics
                {
                    usersOpenPositions = usersOpenPositions,
                    performanceMetric = performanceMetric
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
