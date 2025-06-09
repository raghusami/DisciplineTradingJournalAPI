using DisciplineTradingJournalAPI.DataEntity;
using System.Threading.Tasks;

namespace DisciplineTradingJournalAPI.Contract
{
    public interface ITradeEmotionRepository
    {
        Task AddAsync(TradeEmotions tradeEmotion);
    }
}
