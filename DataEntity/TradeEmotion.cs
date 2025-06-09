using System;
using System.ComponentModel.DataAnnotations;

namespace DisciplineTradingJournalAPI.DataEntity
{
    public class TradeEmotions
    {
        [Key]
        public int EmotionId { get; set; }

        public int TradeId { get; set; }                  

        public string EmotionAt { get; set; }             

        public string EmotionTag { get; set; }            

        public int EmotionIntensity { get; set; }       

        public bool IsPositive { get; set; }               

        public string Notes { get; set; }             

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
