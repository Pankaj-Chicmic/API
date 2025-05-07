using System;
using UnityEditor;

namespace EasyAPI
{
    namespace RunTime
    {
        [Serializable]
        public class RequestClass
        {
            public string endPoint;
            public RequestTypes requestTypes;
            public PayLoadEnum payLoadClass;
            public ResponseEnum responseClass;
            public RequestClass CloneMe()
            {
                return new RequestClass()
                {
                    endPoint = endPoint,
                    requestTypes = requestTypes,
                    payLoadClass = payLoadClass,
                    responseClass = responseClass
                };
            }
        }

        [Serializable]
        public enum RequestTypes
        {
            GET,
            POST,
            PUT,
            DELETE
        }

        [System.Serializable]
        public struct EnumData
        {
            public string enumFullName;
            public MonoScript enumFile;
        }
    }
}