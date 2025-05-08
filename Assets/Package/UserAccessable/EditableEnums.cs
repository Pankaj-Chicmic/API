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
            [DisplayName("LoginData")]
            Logindata,
            [DisplayName("RequestPayloadBase")]
            Requestpayloadbase
        }

        [Serializable]
        public enum ResponseEnum
        {

            [DisplayName("ExampleResponsePayload")]
            Exampleresponsepayload,
            [DisplayName("UserAccount")]
            Useraccount,
            [DisplayName("RequestResponseBase")]
            Requestresponsebase
        }

        [Serializable]
        public enum EndPoints
        {

            [DisplayName(".com/todos")]
            ComTodos,
            [DisplayName("/access/login")]
            AccessLogin
        }
    }
}