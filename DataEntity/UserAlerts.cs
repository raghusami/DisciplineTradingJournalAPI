using System;
using System.ComponentModel.DataAnnotations;

namespace DisciplineTradingJournalAPI.DataEntity
{
    public class UserAlerts
    {
        [Key]
        public int AlertID { get; set; }
        public int UserID { get; set; }
        public string AlertMessage { get; set; }
        public string AlertType { get; set; }
        public string TriggerCondition { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsAcknowledged { get; set; } = false;

    }
}
