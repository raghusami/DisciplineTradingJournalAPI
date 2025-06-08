using DisciplineTradingJournalAPI.Contract;
using DisciplineTradingJournalAPI.DataEntity;
using DisciplineTradingJournalAPI.DBModel;
using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


namespace DisciplineTradingJournalAPI.DesignPattern
{
   

    public interface IUnitOfWork : IDisposable
    {
        ISeriesTracker SeriesTrackers { get; }
        ISeriesTradeDetailsRepository SeriesTradeDetails { get; }
        Task<int> CompleteAsync();
    }
}
