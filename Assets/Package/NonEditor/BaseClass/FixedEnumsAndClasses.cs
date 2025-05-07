using System;
using UnityEditor;
using UnityEngine;

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
            public IntAndBool requestTimeout;
            public IntAndBool retryInfo;
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

        [System.Serializable]
        public class IntAndBool
        {
            public bool overrideValue;
            [Range(1, 100)]
            public int overridenValue = 1;
        }
    }
}