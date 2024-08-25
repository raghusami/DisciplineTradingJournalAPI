namespace DisciplineTradingJournalAPI.DataEntity
{
    public class TradingCharges
    {
        public decimal STTTotal { get; set; }
        public decimal TransactionCharges { get; set; }
        public decimal DPCharges { get; set; }
        public decimal GST { get; set; }
        public decimal SEBICharges { get; set; }
        public decimal StampDuty { get; set; }
        public decimal TotalCharges { get; set; }
        public decimal PointsToBreakeven { get; set; }
        public decimal NetPL { get; set; }
        public decimal Change { get; set; } = 0;    
    }
}
