﻿using System;
using static EzAPI.TypeFinder;
namespace EzAPI
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

    [DisplayName("com")]
Com
}
    }
}