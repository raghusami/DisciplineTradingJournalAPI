namespace DisciplineTradingJournalAPI.Helper
{
    public class AppConfiguration
    {
        public DBConfiguration DBConfiguration { get; set; }

        public ApplicationConfiguration ApplicationConfiguration { get; set; }

        public LoggerConfiguration loggerConfiguration { get; set; }

    }
    public record DBConfiguration
    {
        public string CoreDBConnectionString { get; set; }
    }
    public record LoggerConfiguration
    {
        public string LogFilePath { get; set; }

        public bool LogsWriteToFile { get; set; }

        public string LogsWriteToSeqURL { get; set; }

    }
    public record ApplicationConfiguration
    {
        public string AESEncryptionDecryptionKey { get; set; }
    }

}
