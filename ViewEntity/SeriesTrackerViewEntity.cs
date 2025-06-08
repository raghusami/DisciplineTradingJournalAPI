using System;

namespace DisciplineTradingJournalAPI.ViewEntity
{
    public class SeriesTrackerViewEntity
    {
        public int TotalTrades { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string SeriesName { get;set;}
    }
}
