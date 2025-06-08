using DisciplineTradingJournalAPI.DataEntity;
using System.Threading.Tasks;

namespace DisciplineTradingJournalAPI.Contract
{
    public interface IDashboardRepository
    {
        Task<Dashboard> GetUserPerformanceDashBoardAsync(int userID);
    }

}
