using DisciplineTradingJournalAPI.DataEntity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DisciplineTradingJournalAPI.Contract
{
    public interface ITradingUsersRepository
    {
        Task<TradingUsers> AddAsync(TradingUsers tradingUser,string rawPassword);
        Task<TradingUsers> SignInAsync(string userName, string passWord);
        Task<TradingUsers> GetByIdAsync(int userId);
        Task<IEnumerable<TradingUsers>> GetAllAsync();
        Task<TradingUsers> UpdateAsync(TradingUsers tradingUser);
        Task<bool> DeleteAsync(int userId);
    }
}
