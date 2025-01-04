using DisciplineTradingJournalAPI.DataEntity;
using System.Threading.Tasks;

namespace DisciplineTradingJournalAPI.Contract
{
    public interface IOptionsAnalysis
    {
        Task<string> AddOptionAnalysisDataAsync(InputData inputData);

        Task<string> AddOptionAnalysisCrudeOilDataAsync(CrudeOilInputData inputData);
    }
}
