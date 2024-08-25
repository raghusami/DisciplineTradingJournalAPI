using System;
using System.ComponentModel.DataAnnotations;

namespace DisciplineTradingJournalAPI.DataEntity
{
    public class TradingAccountValues
    {
        [Key]
        public int AccountValueID { get; set; }
        public int UserID { get; set; }
        public DateTime DateRecorded { get; set; }
        public decimal AccountValue { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
