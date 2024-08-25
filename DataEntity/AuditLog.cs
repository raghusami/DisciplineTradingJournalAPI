using System;
using System.ComponentModel.DataAnnotations;

namespace DisciplineTradingJournalAPI.DataEntity
{
    public class AuditLog
    {
        [Key]
        public int LogID { get; set; }
        public int? UserID { get; set; }
        public string Action { get; set; }
        public string Details { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

  
    }
}
