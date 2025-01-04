using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DisciplineTradingJournalAPI.DataEntity
{
    [Table("UserTrades")]
    public class UserTrades
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TradeID { get; set; }
        public int UserID { get; set; }
        public string Symbol { get; set; }
        public DateTime EntryDate { get; set; }
        public DateTime? ExitDate { get; set; }
        public TimeSpan? EntryTime { get; set; }
        public TimeSpan? ExitTime { get; set; }
        public decimal EntryPrice { get; set; }
        public decimal? ExitPrice { get; set; }
        public int Quantity { get; set; }
        public string TradeType { get; set; } // e.g., 'Buy', 'Sell', 'Short', 'Cover'
        public decimal? InitialRisk { get; set; }
        public decimal? NetProfitLoss { get; set; } // Net Profit and Loss
        public decimal CapitalUsed { get; set; } // Amount of capital used for the trade
        public decimal? BrokerCharges { get; set; } = 0; // Total charges associated with the trade
        public string MarketType { get; set; } // e.g., 'stock', 'option', 'future', 'commodity'
        public char PositionStatus { get; set; } = 'O';
        public string? TradeStatus { get; set; }
        public decimal? Change { get; set; } = 0;
        public DateTime? CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; } = DateTime.Now;
    }
    public class UserOpenPositions
    {
        public string Symbol { get; set; }
        public int Quantity { get; set; }
        public decimal EntryPrice { get; set; }
        public decimal CurrentPrice { get; set; }
        public decimal ProfitAndLoss { get; set; } 
        public decimal Change { get; set; }
        public int TradeID { get; set; }
        public int UserID { get; set; }
    }
    public class UserClosePositions
    {
        public string Symbol { get; set; }
        public DateTime? TradeEntryDate { get; set; }
        public int Quantity { get; set; }
        public decimal EntryPrice { get; set; }
        public decimal ExitPrice { get; set; }
        public decimal? ProfitAndLoss { get; set; }
        public decimal? BrokerCharges { get; set; }
        public string MarketType { get; set; }
        public string TradeType { get; set; }
        public decimal? Change { get; set; }
        public string TradeStatus { get; set; }
        public int TradeID { get; set; }
        public int UserID { get; set; }
        public double HoldingDays { get; set; }
        public decimal InvestmentAmount { get; set; }
        public decimal? NetROI { get; set; }
    }

    public class UserPerformanceMetric
    {
        public int UserID { get; set; }
        public int? TotalTrades { get; set; }
        public int? WinningTrades { get; set; }
        public int? LosingTrades { get; set; }
        public decimal? AverageWin { get; set; }
        public decimal? AverageLoss { get; set; }
        public decimal? ProfitFactor { get; set; }
        public decimal? WinRate { get; set; }
        public decimal? NetProfitAndLoss { get; set; }
        public decimal? TotalCharge { get; set; }
        public int? WinningDays { get; set; }
        public int? LosingDays { get; set; }
        public decimal? MaxDrawDown { get; set; }
        public decimal? DayWinRate { get; set; }

        public decimal? TradingScore { get; set; }
    }
    public class UsersPositionsAndPerformanceMetrics
    {
        public List<UserOpenPositions> usersOpenPositions { get; set; }

        public UserPerformanceMetric performanceMetric { get; set; }
    }
    public class UsersClosedPositionsAndPerformanceMetrics
    {
        public List<UserClosePositions> userClosePositions { get; set; }

        public UserPerformanceMetric performanceMetric { get; set; }
    }
}
