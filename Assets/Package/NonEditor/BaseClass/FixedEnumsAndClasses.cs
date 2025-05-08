using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace EasyAPI
{
    namespace RunTime
    {
        [Serializable]
        public class ProgressEvent : UnityEvent<ValueFloat>
        {
        }

        [Serializable]
        public class RequestResponseEvent : UnityEvent<RequestResponseBase>
        {
        }
        [Serializable]
        public class ValueFloat
        {
            private float value;
            public float Value
            {
                get
                {
                    return value;
                }
                set
                {
                    this.value = value;
                }
            }
        }
        public class MethodStorage
        {
            public Type type1;
            public Type type2;
            public MethodInfo method;
            public MethodStorage(Type type1, Type type2, MethodInfo method)
            {
                this.type1 = type1;
                this.type2 = type2;
                this.method = method;
            }
        }
        [Serializable]
        public class DataTypeAndContentType
        {
            public DataType dataType = DataType.Json;
            public string contentType = "application/json";
        }
        [Serializable]
        public class RequestClass
        {
            public string endPoint;
            public RequestTypes requestTypes;
            public PayLoadEnum payLoadClass;
            public ResponseEnum responseClass;
            public IntAndBool requestTimeout;
            public IntAndBool retryInfo;
            public DataTypeStruct dataTypeStruct;
            public ContentTypeStruct contentTypeStruct;
        }

        [Serializable]
        public class ContentTypeStruct
        {
            public bool contentTypeOverride;
            public string contentType;
        }

        [Serializable]
        public class DataTypeStruct
        {
            public bool dataTypeOverride;
            public DataType dataType;
        }

        [Serializable]
        public enum RequestTypes
        {
            GET,
            POST,
            PUT,
            DELETE
        }
        [Serializable]
        public enum DataType
        {
            Json,
            Form
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

        [System.Serializable]
        public class HeaderKeysAndValue
        {
            public string key;
            public string value;
        }
    }
}