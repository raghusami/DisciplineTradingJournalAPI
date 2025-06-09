using DisciplineTradingJournalAPI.DataEntity;
using System.Threading.Tasks;

namespace DisciplineTradingJournalAPI.Contract
{
    public interface IPreTradeChecklistRepository
    {
        Task AddAsync(PreTradeChecklist preTradeChecklist);
    }
}
