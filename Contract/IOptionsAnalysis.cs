using DisciplineTradingJournalAPI.DataEntity;
using System.Threading.Tasks;

namespace DisciplineTradingJournalAPI.Contract
{
    public interface IOptionsAnalysis
    {
        Task<string> AddOptionAnalysisDataAsync(NSEOptionChainResponse inputData);

        Task<string> AddOptionAnalysisCrudeOilDataAsync(int addDay);

        Task<string> AddDayCrudeOilDataAsync();

        Task<string> AddOptionAnalysisGoldDataAsync(CrudeOilInputData inputData);
        Task<string> AddOptionAnalysisDataAsync(InputData inputData);

        Task<string> AddOptionAnalysisCrudeOilDataAsync(CrudeOilInputData inputData);
    }
}
