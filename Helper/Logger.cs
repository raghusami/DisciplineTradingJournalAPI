using DisciplineTradingJournalAPI.Contract;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Threading.Tasks;

namespace DisciplineTradingJournalAPI.Helper
{
    public class Logger : IAsyncLogger
    {
        private readonly string _logFilePath;

        public Logger(IOptionsSnapshot<AppConfiguration> appConfiguration)
        {
            _logFilePath = appConfiguration?.Value?.loggerConfiguration?.LogFilePath ?? string.Empty;
        }

        private string GetFileName(string specificFileName)
        {
            return string.IsNullOrWhiteSpace(specificFileName)
                ? $"{_logFilePath}Log_{DateTime.Today:ddMMyyyy}.txt"
                : $"{_logFilePath}{specificFileName}_{DateTime.Today:ddMMyyyy}.txt";
        }
        public async Task LogAsync(string text, string specificFileName = null)
        {
            using (StreamWriter streamWriter = new StreamWriter(GetFileName(specificFileName), true))
            {
                await streamWriter.WriteLineAsync($"{DateTime.Now:dd-MM-yyyy HH:mm:ss} : {text}");
            }
        }

        public async Task LogAsync(string text, DateTime dateTime, string specificFileName = null)
        {
            using (StreamWriter streamWriter = new StreamWriter(GetFileName(specificFileName), true))
            {
                await streamWriter.WriteLineAsync($"{dateTime:dd-MM-yyyy HH:mm:ss} : {text}");
            }
        }
        public async Task LogAsync(Exception exception, string message, string specificFileName = null)
        {
            using (StreamWriter streamWriter = new StreamWriter(GetFileName(specificFileName), true))
            {
                string logMessage = $"{DateTime.Now:dd-MM-yyyy HH:mm:ss} : {message} - Exception: {exception.Message} StackTrace: {exception.StackTrace}";
                await streamWriter.WriteLineAsync(logMessage);
            }
        }
        public Task BeginLogAsync(string fileName)
        {
            return LogAsync($"Process for File {fileName} started", fileName);
        }

        public Task StartLogAsync(string specificFileName = null)
        {
            return LogAsync("---------------------------------------", specificFileName);
        }

        public Task EndLogAsync(string specificFileName = null)
        {
            return LogAsync("---------------------------------------", specificFileName);
        }

        // ILogger interface implementation
        public IDisposable BeginScope<TState>(TState state) => null;

        public bool IsEnabled(LogLevel logLevel) => true;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (IsEnabled(logLevel))
            {
                LogAsync(formatter(state, exception)).GetAwaiter().GetResult();
            }
        }
    }

}
