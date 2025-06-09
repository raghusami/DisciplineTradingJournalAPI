using System;
using System.ComponentModel.DataAnnotations;

namespace DisciplineTradingJournalAPI.DataEntity
{
    public class PreTradeChecklist
    {
        [Key]
        public int ChecklistId { get; set; }
        public int TradeId { get; set; }
        public bool HasValidSetup { get; set; }
        public bool IsCalm { get; set; }
        public bool AcceptsRisk { get; set; }
        public bool SleptWell { get; set; }
        public bool IsNotRevengeTrade { get; set; }
        public bool TookTimeToReviewPlan { get; set; }   
        public string Timeframe { get; set; }      
        public string Notes { get; set; }         
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
