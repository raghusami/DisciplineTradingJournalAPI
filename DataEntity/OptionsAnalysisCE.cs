using System;
using System.ComponentModel.DataAnnotations;

namespace DisciplineTradingJournalAPI.DataEntity
{
    public class OptionsAnalysisCE
    {
        [Key]
        public int CallId { get; set; }
        public int StrikePrice { get; set; }
        public string ExpiryDate { get; set; }
        public string TradeDate { get; set; }
        public string Underlying { get; set; }
        public string OptionType { get; set; }
        public int OpenInterest { get; set; }
        public int ChangeInOpenInterest { get; set; }
        public double LastPrice { get; set; }
        public double ImpliedVolatility { get; set; }
        public double UnderlyingValue { get; set; }
    }
    public class OptionsAnalysisPE
    {

        [Key]
        public int PutId { get; set; }
        public int StrikePrice { get; set; }
        public string ExpiryDate { get; set; }
        public string TradeDate { get; set; }
        public string Underlying { get; set; }
        public string OptionType { get; set; } 
        public int OpenInterest { get; set; }
        public int ChangeInOpenInterest { get; set; }
        public double LastPrice { get; set; }
        public double ImpliedVolatility { get; set; }
        public double UnderlyingValue { get; set; }
    }
    public class OptionsAnalysisCrudeOil

    {
        [Key]
        public int Id { get; set; }
        public int StrikePrice { get; set; }
        public DateTime ExpiryDate { get; set; }
        public DateTime TradeDate { get; set; }
        public string Underlying { get; set; }
        public double UnderlyingValue { get; set; }
        public int CEOpenInterest { get; set; }
        public int CEChangeInOpenInterest { get; set; }
        public double CELastPrice { get; set; }
        public decimal CEVolume { get; set; }
        public int PEOpenInterest { get; set; }
        public int PEChangeInOpenInterest { get; set; }
        public double PELastPrice { get; set; }
        public decimal PEVolume { get; set; }

    }

    public class DayCrudeOilData

    {
        [Key]
        public int Id { get; set; }
        public int StrikePrice { get; set; }
        public DateTime ExpiryDate { get; set; }
        public DateTime TradeDate { get; set; }
        public string Underlying { get; set; }
        public double UnderlyingValue { get; set; }
        public int CEOpenInterest { get; set; }
        public int CEChangeInOpenInterest { get; set; }
        public double CELastPrice { get; set; }
        public decimal CEVolume { get; set; }
        public int PEOpenInterest { get; set; }
        public int PEChangeInOpenInterest { get; set; }
        public double PELastPrice { get; set; }
        public decimal PEVolume { get; set; }
        public DateTime CreatedDate { get; set; }

    }
}
