using System;
using System.ComponentModel.DataAnnotations;

namespace DisciplineTradingJournalAPI.DataEntity
{
    public class TradingUserProfile
    {
        [Key]
        public int ProfileID { get; set; }
        public int UserID { get; set; }
        public string Bio { get; set; }
        public string ProfilePictureURL { get; set; }
        public string ContactNumber { get; set; }
        public string Address { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
