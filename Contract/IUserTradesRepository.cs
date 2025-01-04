using DisciplineTradingJournalAPI.DataEntity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DisciplineTradingJournalAPI.Contract
{
    public interface IUserTradesRepository
    {
        Task<List<UserTrades>> GetAllAsync(int userID);
        Task<UserTrades> GetByIdAsync(int tradeID);
        Task<UserTrades> AddAsync(UserTrades userTrade);
        Task<UsersPositionsAndPerformanceMetrics> GetUsersOpenPositionsWithTradeMetricAsync(int userID);
        Task<UsersClosedPositionsAndPerformanceMetrics> GetUsersClosedPositionsWithTradeMetricAsync(int userID);
        Task<List<UserOpenPositions>> GetUsersOpenPositionsAsync(int userID);
        Task<List<UserClosePositions>> GetUsersClosedPositionsAsync(int userID);
        Task<UserTrades> UpdateAsync(UserTrades userTrade);
        Task DeleteAsync(int tradeID);
    }
}
