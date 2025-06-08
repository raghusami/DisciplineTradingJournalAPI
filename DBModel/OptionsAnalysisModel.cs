using DisciplineTradingJournalAPI.Contract;
using DisciplineTradingJournalAPI.DataEntity;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DisciplineTradingJournalAPI.DBModel
{
    public class OptionsAnalysisModel : IOptionsAnalysis
    {
        private readonly TradingJournalDbContext _context;
        public OptionsAnalysisModel(TradingJournalDbContext context)
        {
            _context = context;
        }
        public async Task<string> AddOptionAnalysisCrudeOilDataAsync(int addDays)
        {
            string result = string.Empty;
            try
            {
                CrudeLiveDataProcess crudeLiveDataProcess = new CrudeLiveDataProcess();
                ApiResponse inputData = await crudeLiveDataProcess.GetCrudeOilLiveData();
                // Lists for PE and CE data
                var analysisRecordsPE = new List<OptionsAnalysisCrudeOil>();
                var underlyingValue = inputData.d.Data.First().UnderlyingValue;

                var maxStrikePrice = underlyingValue + 5000;
                var minStrikePrice = underlyingValue - 5000;

                foreach (var option in inputData.d.Data.Where(o =>
                 o.CE_StrikePrice >= minStrikePrice &&
                 o.CE_StrikePrice <= maxStrikePrice &&
                 o.CE_StrikePrice % 1000 == 0))
                {
                    // Add CE/PE data

                    var expiryMonth = Convert.ToDateTime(option.ExpiryDate);
                    var currentMonth = DateTime.Now.Month;

                    analysisRecordsPE.Add(new OptionsAnalysisCrudeOil
                    {
                        StrikePrice = Convert.ToInt32(option.CE_StrikePrice),
                        ExpiryDate = Convert.ToDateTime(option.ExpiryDate),
                        TradeDate = DateTime.Now.AddDays(addDays),
                        Underlying = "GOLD",
                        CEOpenInterest = Convert.ToInt32(option.CE_OpenInterest),
                        CEChangeInOpenInterest = Convert.ToInt32(option.CE_ChangeInOI),
                        CELastPrice = Convert.ToDouble(option.CE_LTP),
                        CEVolume = option.CE_Volume,
                        PEOpenInterest = Convert.ToInt32(option.PE_OpenInterest),
                        PEChangeInOpenInterest = Convert.ToInt32(option.PE_ChangeInOI),
                        PELastPrice = Convert.ToDouble(option.PE_LTP),
                        PEVolume = option.PE_Volume,
                        UnderlyingValue = Convert.ToInt32(option.UnderlyingValue),
                    });
                }
                // Batch save both PE and CE data
                await _context.OptionsAnalysisCrudeOil.AddRangeAsync(analysisRecordsPE);
                await _context.SaveChangesAsync(); // Single SaveChangesAsync call for better performance

                return "Analysis data saved successfully.";
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.Error.WriteLine($"Error saving analysis data: {ex.Message}");
                return $"Error saving analysis data: {ex.Message}";
            }
        }

        public async Task<string> AddDayCrudeOilDataAsync()
        {
            string result = string.Empty;
            try
            {
                CrudeLiveDataProcess crudeLiveDataProcess = new CrudeLiveDataProcess();
                ApiResponse inputData = await crudeLiveDataProcess.GetCrudeOilLiveData();
                // Lists for PE and CE data
                var analysisRecordsPE = new List<DayCrudeOilData>();
                var underlyingValue = inputData.d.Data.First().UnderlyingValue;

                var maxStrikePrice = underlyingValue + 5000;
                var minStrikePrice = underlyingValue - 5000;
                foreach (var option in inputData.d.Data.Where(o =>
                 o.CE_StrikePrice >= minStrikePrice &&
                 o.CE_StrikePrice <= maxStrikePrice &&
                 o.CE_StrikePrice % 1000 == 0))
                {
                    // Add CE/PE data

                    var expiryMonth = Convert.ToDateTime(option.ExpiryDate);
                    var currentMonth = DateTime.Now.Month;

                    analysisRecordsPE.Add(new DayCrudeOilData
                    {
                        StrikePrice = Convert.ToInt32(option.CE_StrikePrice),
                        ExpiryDate = Convert.ToDateTime(option.ExpiryDate),
                        TradeDate = DateTime.Now,
                        Underlying = "GOLD",
                        CEOpenInterest = Convert.ToInt32(option.CE_OpenInterest),
                        CEChangeInOpenInterest = Convert.ToInt32(option.CE_ChangeInOI),
                        CELastPrice = Convert.ToDouble(option.CE_LTP),
                        CEVolume = option.CE_Volume,
                        PEOpenInterest = Convert.ToInt32(option.PE_OpenInterest),
                        PEChangeInOpenInterest = Convert.ToInt32(option.PE_ChangeInOI),
                        PELastPrice = Convert.ToDouble(option.PE_LTP),
                        PEVolume = option.PE_Volume,
                        UnderlyingValue = Convert.ToInt32(option.UnderlyingValue),
                        CreatedDate = DateTime.Now
                    });
                }
                // Batch save both PE and CE data
                await _context.DayCrudeOilData.AddRangeAsync(analysisRecordsPE);
                await _context.SaveChangesAsync(); // Single SaveChangesAsync call for better performance

                return "Analysis data saved successfully.";
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.Error.WriteLine($"Error saving analysis data: {ex.Message}");
                return $"Error saving analysis data: {ex.Message}";
            }
        }
        public async Task<string> AddOptionAnalysisGoldDataAsync(CrudeOilInputData inputData)
        {
            string result = string.Empty;
            try
            {
                // Lists for PE and CE data
                var analysisRecordsPE = new List<OptionsAnalysisCrudeOil>();

                var underlyingValue = inputData.Data.First().UnderlyingValue;

                foreach (var option in inputData.Data.Where(o =>
                 o.CE_LTP >= 0 && o.PE_LTP > 0))
                {
                    // Add CE/PE data

                    var expiryMonth = Convert.ToDateTime(option.ExpiryDate);
                    var currentMonth = DateTime.Now.Month;
                    analysisRecordsPE.Add(new OptionsAnalysisCrudeOil
                    {
                        StrikePrice = Convert.ToInt32(option.CE_StrikePrice),
                        ExpiryDate = Convert.ToDateTime(inputData.ExpiryDate),
                        TradeDate = Convert.ToDateTime(inputData.TradeDate),
                        Underlying = "Gold",
                        CEOpenInterest = Convert.ToInt32(option.CE_OpenInterest),
                        CEChangeInOpenInterest = Convert.ToInt32(option.CE_ChangeInOI),
                        CELastPrice = Convert.ToDouble(option.CE_LTP),
                        CEVolume = option.CE_Volume,
                        PEOpenInterest = Convert.ToInt32(option.PE_OpenInterest),
                        PEChangeInOpenInterest = Convert.ToInt32(option.PE_ChangeInOI),
                        PELastPrice = Convert.ToDouble(option.PE_LTP),
                        PEVolume = option.PE_Volume,
                        UnderlyingValue = Convert.ToInt32(option.UnderlyingValue),
                    });

                }

                // Batch save both PE and CE data
                await _context.OptionsAnalysisCrudeOil.AddRangeAsync(analysisRecordsPE);
                await _context.SaveChangesAsync(); // Single SaveChangesAsync call for better performance

                return "Analysis data saved successfully.";
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.Error.WriteLine($"Error saving analysis data: {ex.Message}");
                return $"Error saving analysis data: {ex.Message}";
            }
        }

        public async Task<string> AddOptionAnalysisDataAsync(NSEOptionChainResponse inputData)
        {
            string result = string.Empty;
            try
            {
                // Lists for PE and CE data
                var analysisRecordsPE = new List<OptionsAnalysisPE>();
                var analysisRecordsCE = new List<OptionsAnalysisCE>();

                foreach (var option in inputData.Records.Data)
                {
                    // Add PE data

                    var expiryMonth = Convert.ToDateTime(option.ExpiryDate);

                    var currentMonth = DateTime.Now.Month;
                    if (option.PE != null && currentMonth == expiryMonth.Month)
                    {
                        analysisRecordsPE.Add(new OptionsAnalysisPE
                        {
                            StrikePrice = option.PE.StrikePrice,
                            ExpiryDate = option.PE.ExpiryDate,
                            TradeDate = inputData.Records.Timestamp,
                            Underlying = option.PE.Underlying,
                            OpenInterest = option.PE.OpenInterest,
                            ChangeInOpenInterest = option.PE.ChangeInOpenInterest,
                            LastPrice = option.PE.LastPrice,
                            ImpliedVolatility = option.PE.ImpliedVolatility,
                            UnderlyingValue = option.PE.UnderlyingValue,
                            OptionType = "PE",
                        });
                    }
                    // Add CE data
                    if (option.CE != null && currentMonth == expiryMonth.Month)
                    {
                        analysisRecordsCE.Add(new OptionsAnalysisCE
                        {
                            StrikePrice = option.CE.StrikePrice,
                            ExpiryDate = option.CE.ExpiryDate,
                            TradeDate = inputData.Records.Timestamp,
                            Underlying = option.CE.Underlying,
                            OpenInterest = option.CE.OpenInterest,
                            ChangeInOpenInterest = option.CE.ChangeInOpenInterest,
                            LastPrice = option.CE.LastPrice,
                            OptionType = "CE",
                            ImpliedVolatility = option.CE.ImpliedVolatility,
                            UnderlyingValue = option.CE.UnderlyingValue
                        });
                    }
                }
                await _context.OptionsAnalysisPE.AddRangeAsync(analysisRecordsPE);
                await _context.OptionsAnalysisCE.AddRangeAsync(analysisRecordsCE);
                await _context.SaveChangesAsync();
                return "Analysis data saved successfully.";
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.Error.WriteLine($"Error saving analysis data: {ex.Message}");
                return $"Error saving analysis data: {ex.Message}";
            }
        }

        // Example to parse the timestamp
        public static DateTime ConvertUnixTimestampToDateTime(string unixTimestamp)
        {
            // Extract the number part from /Date(1734114295000)/
            var match = System.Text.RegularExpressions.Regex.Match(unixTimestamp, @"\d+");
            if (match.Success)
            {
                long milliseconds = long.Parse(match.Value);
                // Convert to DateTime
                DateTime dateTime = DateTimeOffset.FromUnixTimeMilliseconds(milliseconds).UtcDateTime;
                return dateTime;
            }
            throw new FormatException("Invalid timestamp format.");
        }


    }
}