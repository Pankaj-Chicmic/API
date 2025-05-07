using EasyAPI.RunTime;

namespace EasyAPI
{
    namespace Example
    {
        [System.Serializable]
        public class ExampleRequestPayload : RequestPayloadBase
        {
            public string exampleField;
        }

        [System.Serializable]
        public class ExampleResponsePayload : RequestResponseBase
        {
            public string exampleField;
        }
    }
}