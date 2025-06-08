using DisciplineTradingJournalAPI.Contract;
using DisciplineTradingJournalAPI.DataEntity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using System.Linq;
using DisciplineTradingJournalAPI.DBModel;

namespace DisciplineTradingJournalAPI.Controllers
{
    public class OptionsAnalysisController :BaseController
    {
        private readonly IOptionsAnalysis _optionsAnalysis;

        public OptionsAnalysisController(IOptionsAnalysis optionsAnalysis)
        {
            _optionsAnalysis = optionsAnalysis;
        }
        [HttpPost]
        [Route("PostOptionsData")]
        public async Task<IActionResult> PostOptionsData([FromBody] NSEOptionChainResponse inputData)
        {
            var userTrades = await _optionsAnalysis.AddOptionAnalysisDataAsync(inputData);
            return SuccessResponseWithData(userTrades);
        }
        [HttpPost]
        [Route("PostGoldOptionsData")]
        public async Task<IActionResult> PostGoldOptionsData([FromBody] CrudeOilInputData inputData)
        {
            var userTrades = await _optionsAnalysis.AddOptionAnalysisGoldDataAsync(inputData);
            return SuccessResponseWithData(userTrades);
        }
        [HttpGet]
        [Route("GetCrudeOilOptionsData")]
        public async Task<IActionResult> GetCrudeOilOptionsData(int addDays)
        {
            var userTrades = await _optionsAnalysis.AddOptionAnalysisCrudeOilDataAsync(addDays);
            return SuccessResponseWithData(userTrades);
        }
        [HttpPost]
        [Route("PostCrudeOilOptionsData")]
        public async Task<IActionResult> PostCrudeOilOptionsData([FromBody] CrudeOilInputData inputData)
        {
            var userTrades = await _optionsAnalysis.AddOptionAnalysisCrudeOilDataAsync(inputData);
            return SuccessResponseWithData(userTrades);
        }
    }
}
