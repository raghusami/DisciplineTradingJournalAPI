using System;
using System.ComponentModel.DataAnnotations;

namespace DisciplineTradingJournalAPI.DataEntity
{
    public class Notification
    {
        [Key]
        public int NotificationID { get; set; }
        public int UserID { get; set; }
        public string Message { get; set; }
        public bool IsRead { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    }
}
