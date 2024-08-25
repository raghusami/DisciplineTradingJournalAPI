using System;
using System.ComponentModel.DataAnnotations;

namespace DisciplineTradingJournalAPI.DataEntity
{
    public class TradeSettings
    {
        [Key]
        public int SettingID { get; set; }
        public int UserID { get; set; }
        public int? MaxTradesPerDay { get; set; }
        public decimal? RiskAmount { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    }
}
