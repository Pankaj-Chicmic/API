using System;
using System.Collections.Generic;
using UnityEngine;
namespace EasyAPI
{
    namespace RunTime
    {
        [CreateAssetMenu(fileName = "APIConfig", menuName = "Easy API/API Config", order = 3)]
        public class APIConfig : ScriptableObject
        {
            public string baseUrl;
            [Range(1, 100)]
            public int defaultRequestTimeout;
            [Range(1, 100)]
            public int defaultRetryCount;
            public DataType dataType = DataType.Json;
            public List<DataTypeAndContentType> dataTypeAndContentTypes = new List<DataTypeAndContentType>()
            {
                new DataTypeAndContentType()
                {
                    dataType = DataType.Json,
                    contentType = "application/json",
                },
                new DataTypeAndContentType()
                {
                    dataType = DataType.Form,
                    contentType = "application/x-www-form-urlencoded"
                }
            };

            public string ContentType(DataType dataType)
            {
                foreach (var item in dataTypeAndContentTypes)
                {
                    if (item.dataType == dataType)
                    {
                        return item.contentType;
                    }
                }
                Debug.LogWarning("Data Type not found in settings return default content type [application/json]");
                return "application/json";
            }
        }
    }
}