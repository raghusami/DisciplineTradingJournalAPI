using System;
using System.ComponentModel.DataAnnotations;

namespace DisciplineTradingJournalAPI.DataEntity
{
    public class PerformanceMetric
    {
        [Key]
        public int MetricID { get; set; }
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
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;


    }

}
