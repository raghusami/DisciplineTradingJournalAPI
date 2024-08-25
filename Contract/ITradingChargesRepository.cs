using DisciplineTradingJournalAPI.DataEntity;
using System.Threading.Tasks;

namespace DisciplineTradingJournalAPI.Contract
{
    public interface ITradingChargesRepository
    {
       TradingCharges EquityCalculateCharges(decimal buyPrice, decimal sellPrice, int quantity);
       TradingCharges FAndOCalculateCharges(decimal buyPrice, decimal sellPrice, int quantity);
       decimal ProfitAndLoss(decimal buyPrice, decimal sellPrice, int quantity);
       decimal Changes(decimal buyPrice, decimal sellPrice);
    }
}
