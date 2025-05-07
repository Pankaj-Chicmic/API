namespace EasyAPI
{
    namespace RunTime
    {
        public class RequestPayloadBase
        {
        }
        public class RequestResponseBase
        {
            public bool success;
            public int responseCode;
            public string failureMessage;
        }
    }
}