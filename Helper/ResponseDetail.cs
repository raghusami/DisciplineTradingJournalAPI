namespace DisciplineTradingJournalAPI.Helper
{
    public class ResponseDetail
    {
        public object ValidationError { get; set; }
        public int ResponseStatusCode { get; set; }
        public object ResponseData { get; set; }
        public string ResponseMessage { get; set; }
    }
    public static class ResponseMessageInfo
    {
        public const string Failure = "Validation Failure";

        public const string Success = "Submitted Successfully.";

        public const string DeleteSuccess = "Deleted Successfully.";

        public const string TransactionExist = "Cannot delete as transaction exist";
    }
}
