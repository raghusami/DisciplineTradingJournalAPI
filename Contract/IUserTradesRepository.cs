using DisciplineTradingJournalAPI.DataEntity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DisciplineTradingJournalAPI.Contract
{
    public interface IUserTradesRepository
    {
        Task<IEnumerable<UserTrades>> GetAllAsync();
        Task<UserTrades> GetByIdAsync(int tradeID);
        Task<UserTrades> AddAsync(UserTrades userTrade);
        Task<UsersPositionsAndPerformanceMetrics> GetUsersOpenPositionsAsync(int userID);
        Task<UserTrades> UpdateAsync(UserTrades userTrade);
        Task DeleteAsync(int tradeID);
    }
}
