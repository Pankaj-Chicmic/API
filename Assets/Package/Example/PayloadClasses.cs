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

        public class XYZ : RequestPayloadBase {

        }

    }
}