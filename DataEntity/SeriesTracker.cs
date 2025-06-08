using System;
using System.ComponentModel.DataAnnotations;

namespace DisciplineTradingJournalAPI.DataEntity
{
    public class SeriesTracker
    {
        [Key]
        public int TrackerId { get; set; }

        public int UserId { get; set; }

        public int TotalTrades {  get; set; }

        public bool IsCompleted { get; set; }
        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedAt { get; set; }
    }

    public class SeriesTradeDetails
    {
        [Key]
        public int DetailId { get; set; }

        public int TrackerId { get; set; }

        public int TradeNumber { get; set; }

        public string TradeSeriesName { get; set; }

    }


}
