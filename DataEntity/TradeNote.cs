using System;
using System.ComponentModel.DataAnnotations;

namespace DisciplineTradingJournalAPI.DataEntity
{
    public class TradeNote
    {
        [Key]
        public int NoteID { get; set; }
        public int TradeID { get; set; }
        public int? UserID { get; set; }
        public string NoteText { get; set; }
        public byte[] Screenshot { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;


    }
}
