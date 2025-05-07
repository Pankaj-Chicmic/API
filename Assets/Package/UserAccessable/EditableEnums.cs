using System;
using static EasyAPI.TypeFinder;
namespace EasyAPI
{
    namespace RunTime
    {
        [Serializable]
        public enum PayLoadEnum
        {

    [DisplayName("None")]
None,
    [DisplayName("ExampleRequestPayload")]
Examplerequestpayload,
    [DisplayName("XYZ")]
Xyz,
    [DisplayName("RequestPayloadBase")]
Requestpayloadbase
}

        [Serializable]
        public enum ResponseEnum
        {

    [DisplayName("ExampleResponsePayload")]
Exampleresponsepayload,
    [DisplayName("RequestResponseBase")]
Requestresponsebase
}

        [Serializable]
        public enum EndPoints
        {

    [DisplayName(".com/todos")]
ComTodos,
    [DisplayName("11Example1")]
_11example1,
    [DisplayName("XYZ")]
Xyz
}
    }
}