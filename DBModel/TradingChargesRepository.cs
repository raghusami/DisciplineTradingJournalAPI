using DisciplineTradingJournalAPI.Contract;
using DisciplineTradingJournalAPI.DataEntity;
using DisciplineTradingJournalAPI.Helper;
using Microsoft.Extensions.Options;
using System;

namespace DisciplineTradingJournalAPI.DBModel
{
    public class TradingChargesRepository : ITradingChargesRepository
    {
        private readonly TradingJournalDbContext _context;
        private readonly IOptionsSnapshot<AppConfiguration> _appConfiguration;
        private readonly decimal _dPCharges = 15.34m;
        private readonly decimal _brokerage = 40m;

        public TradingChargesRepository(TradingJournalDbContext context)
        {
            _context = context;
        }

        public decimal Changes(decimal buyPrice, decimal sellPrice)
        {
            return ((sellPrice - buyPrice) / buyPrice) * 100;
        }

        public  TradingCharges EquityCalculateCharges(decimal buyPrice, decimal sellPrice, int quantity)
        {
            decimal buyAmount = buyPrice * quantity;
            decimal sellAmount = sellPrice * quantity;

            // STT Calculation
            decimal sttBuy = 0.001m * buyAmount;
            decimal sttSell = 0.001m * sellAmount;
            decimal sttTotal = sttBuy + sttSell;

            // Transaction Charges Calculation
            decimal totalTradeValue = buyAmount + sellAmount;
            decimal transactionCharges = 0.0000322m * totalTradeValue;

            // SEBI Charges Calculation
            decimal sebiCharges = 10m * totalTradeValue / 1_00_00_000m;

            // GST Calculation
            decimal gst = 0.18m * (sebiCharges + transactionCharges);

            // Stamp Duty Calculation
            decimal stampDuty = Math.Min(0.00015m * buyAmount, 1500m * buyAmount / 1_00_00_000m);

            // Total Tax and Charges Calculation
            decimal totalCharges = sttTotal + transactionCharges + gst + sebiCharges + stampDuty + _dPCharges;

            // Points to Breakeven
            decimal pointsToBreakeven = totalCharges / quantity;

            // Net P&L Calculation
            decimal netPL = (sellPrice - buyPrice) * quantity - totalCharges;

            // Change % Percentage
            decimal change = ((sellPrice - buyPrice) / buyPrice) * 100;

            // Return the result
            return new TradingCharges
            {
                STTTotal = sttTotal,
                TransactionCharges = transactionCharges,
                DPCharges = _dPCharges,
                GST = gst,
                SEBICharges = sebiCharges,
                StampDuty = stampDuty,
                TotalCharges = totalCharges,
                PointsToBreakeven = pointsToBreakeven,
                NetPL = netPL,
                Change = change
            };
        }

        public TradingCharges FAndOCalculateCharges(decimal buyPrice, decimal sellPrice, int quantity)
        {
            decimal buyAmount = buyPrice * quantity;
            decimal sellAmount = sellPrice * quantity;

            // STT Calculation
            decimal sttBuy = 0.001m * buyAmount;
            decimal sttSell = 0.001m * sellAmount;
            decimal sttTotal = sttBuy + sttSell;

            // Transaction Charges Calculation
            decimal totalTradeValue = buyAmount + sellAmount;
            decimal transactionCharges = 0.0000495m * totalTradeValue;

            // SEBI Charges Calculation
            decimal sebiCharges = 10m * totalTradeValue / 1_00_00_000m;

            // GST Calculation
            decimal gst = 0.18m * (_brokerage + sebiCharges + transactionCharges);

            // Stamp Duty Calculation
            decimal stampDuty = Math.Min(0.00003m * buyAmount, 300m * buyAmount / 1_00_00_000m);

            // Total Tax and Charges Calculation
            decimal totalCharges = sttTotal + transactionCharges + gst + sebiCharges + stampDuty + _brokerage;

            // Points to Breakeven
            decimal pointsToBreakeven = totalCharges / quantity;

            // Net P&L Calculation
            decimal netPL = (sellPrice - buyPrice) * quantity - totalCharges;

            // Change % Percentage
            decimal change = ((sellPrice - buyPrice) / buyPrice) * 100;

            // Return the result
            return new TradingCharges
            {
                STTTotal = sttTotal,
                TransactionCharges = transactionCharges,
                DPCharges = _dPCharges,
                GST = gst,
                SEBICharges = sebiCharges,
                StampDuty = stampDuty,
                TotalCharges = totalCharges,
                PointsToBreakeven = pointsToBreakeven,
                NetPL = netPL,
                Change = change
            };
        }

        public decimal ProfitAndLoss(decimal buyPrice, decimal sellPrice, int quantity)
        {
            return (sellPrice - buyPrice) * quantity;
        }
    }
}
