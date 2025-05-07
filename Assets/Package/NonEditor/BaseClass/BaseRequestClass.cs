using System;

namespace EasyAPI
{
    namespace RunTime
    {
        [Serializable]
        public class RequestPayloadBase
        {
        }
        [Serializable]
        public class RequestResponseBase
        {
            public bool success;
            public int responseCode;
            public string failureMessage;
        }
    }
}