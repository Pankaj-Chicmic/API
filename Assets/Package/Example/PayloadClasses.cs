using EasyAPI.RunTime;
using System;

namespace EasyAPI
{
    namespace Example
    {
        [System.Serializable]
        public class ExampleRequestPayload : RequestPayloadBase
        {
            public string exampleField;
        }

        [Serializable]
        public class LoginData : RequestPayloadBase
        {
            public string email;
            public string password;
        }
    }
}