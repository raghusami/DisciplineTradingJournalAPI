
﻿using Newtonsoft.Json;
using System.Collections.Generic;

namespace DisciplineTradingJournalAPI.DataEntity
{
    public class NSEOptionChainResponse
    {
        [JsonProperty("records")]
        public Records Records { get; set; }

        [JsonProperty("filtered")]
        public Filtered Filtered { get; set; }
    }
    public class Records
    {
        [JsonProperty("expiryDates")]
        public List<string> ExpiryDates { get; set; }

        [JsonProperty("data")]
        public List<OptionData> Data { get; set; }

        [JsonProperty("timestamp")]
        public string Timestamp { get; set; }

        [JsonProperty("underlyingValue")]
        public double UnderlyingValue { get; set; }

        [JsonProperty("strikePrices")]
        public List<double> StrikePrices { get; set; }
    }
    public class OptionData
    {
        public int StrikePrice { get; set; }
        public string ExpiryDate { get; set; }
        public OptionDetails PE { get; set; }
        public OptionDetails CE { get; set; }

    }
    public class OptionDetails
    {
        public int StrikePrice { get; set; }
        public string ExpiryDate { get; set; }
        public string Underlying { get; set; }
        public string Identifier { get; set; }
        public int OpenInterest { get; set; }
        public int ChangeInOpenInterest { get; set; }
        public double PChangeInOpenInterest { get; set; }
        public int TotalTradedVolume { get; set; }
        public double ImpliedVolatility { get; set; }
        public double LastPrice { get; set; }
        public double Change { get; set; }
        public double PChange { get; set; }
        public int TotalBuyQuantity { get; set; }
        public int TotalSellQuantity { get; set; }
        public int BidQty { get; set; }
        public double BidPrice { get; set; }
        public int AskQty { get; set; }
        public double AskPrice { get; set; }
        public double UnderlyingValue { get; set; }
    }
    public class InputData
    {

        public List<OptionData> Data { get; set; }
        public string Timestamp { get; set; }
        public double UnderlyingValue { get; set; }
        public List<int> StrikePrices { get; set; }
    }

    public class Filtered
    {
        [JsonProperty("data")]
        public List<OptionData> Data { get; set; }

        [JsonProperty("CE")]
        public CEPEData CE { get; set; }

        [JsonProperty("PE")]
        public CEPEData PE { get; set; }
    }
    public class CEPEData
    {
        [JsonProperty("totOI")]
        public int TotalOpenInterest { get; set; }

        [JsonProperty("totVol")]
        public int TotalVolume { get; set; }
    }
    public class CrudeOilOptionData
    {
        public object ExtensionData { get; set; }
        public decimal CE_AbsoluteChange { get; set; }
        public decimal CE_AskPrice { get; set; }
        public decimal CE_AskQty { get; set; }
        public decimal CE_BidPrice { get; set; }
        public decimal CE_BidQty { get; set; }
        public decimal CE_ChangeInOI { get; set; }
        public decimal CE_LTP { get; set; }
        public string CE_LTT { get; set; }
        public decimal CE_NetChange { get; set; }
        public decimal CE_OpenInterest { get; set; }
        public decimal CE_StrikePrice { get; set; }
        public decimal CE_Volume { get; set; }
        public object ExpiryDate { get; set; }
        public string LTT { get; set; }
        public decimal PE_AbsoluteChange { get; set; }
        public decimal PE_AskPrice { get; set; }
        public decimal PE_AskQty { get; set; }
        public decimal PE_BidPrice { get; set; }
        public decimal PE_BidQty { get; set; }
        public decimal PE_ChangeInOI { get; set; }
        public decimal PE_LTP { get; set; }
        public string PE_LTT { get; set; }
        public decimal PE_NetChange { get; set; }
        public decimal PE_OpenInterest { get; set; }
        public decimal PE_Volume { get; set; }
        public object Symbol { get; set; } 
        public decimal UnderlyingValue { get; set; }
    }


    public class CrudeOilInputData
    {
        public List<CrudeOilOptionData> Data { get; set; }

        public string TradeDate { get; set; }
        public string ExpiryDate { get; set; }

        public string CommodityType { get; set; }

    }
    public class MCXResponse
    {
        public List<CrudeOilOptionData> Data { get; set; }
    }

    public class ApiResponse
    {
        public MCXResponse d { get; set; }
    }
        public string ExpiryDate { get; set;}
 
    }
}
