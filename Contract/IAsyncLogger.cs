using System;
using System.Threading.Tasks;

namespace DisciplineTradingJournalAPI.Contract
{
    public interface IAsyncLogger
    {
        Task LogAsync(string text, string fileName = null);
        Task LogAsync(string text, DateTime dateTime, string fileName = null);
        Task BeginLogAsync(string fileName);
        Task LogAsync(Exception exception, string message, string fileName = null);
        Task StartLogAsync(string fileName = null);
        Task EndLogAsync(string fileName = null);
    }
}
