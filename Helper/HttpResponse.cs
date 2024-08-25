using System.Net;

namespace DisciplineTradingJournalAPI.Helper
{
    public class HttpApiResponse
    {
        public bool IsSuccess { get; set; }
        public string ErrorMessage { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public string ReasonPhrase { get; set; }
        public SAPResponseData Content { get; set; }
    }
    public class SAPResponseData
    {
        public Metadata d { get; set; }
        public class Metadata
        {
            public string ERROR_FLAG { get; set; }

            public string MESSAGE { get; set; }

            public string pdf { get; set; }

            public string Invno { get; set; }

            public string Message { get; set; }
        }
    }
}
