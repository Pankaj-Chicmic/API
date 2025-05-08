#define Debug
using SimpleJSON;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
namespace EasyAPI
{
    namespace RunTime
    {
        public static class Requests
        {
            public static UnityWebRequestAsyncOperation Post(string route, DataType dataType, string jsonData, int requestTimeout, List<HeaderKeysAndValue> headerKeysAndValues, Action<string> onSuccess = null, Action<int, string> onFailure = null, Action<int> onConnectionError = null)
            {
                #region Debug

                string headersAre = "";
                foreach (var item in headerKeysAndValues)
                {
                    headersAre += $"{item.key} : {item.value} ,";
                }
                Debug.Log($"Hitting [POST] ::API:: {route} ::SendingData:: {jsonData} :: Headers Are {headersAre}");

                #endregion Debug

                byte[] bite = GetBites(jsonData, dataType);
                UnityWebRequest request = new UnityWebRequest(route, UnityWebRequest.kHttpVerbPOST)
                {
                    uploadHandler = new UploadHandlerRaw(bite),
                    downloadHandler = new DownloadHandlerBuffer(),
                };
                return CommonCallBack(request, route, headerKeysAndValues, onSuccess, onFailure, onConnectionError, requestTimeout);
            }

            public static UnityWebRequestAsyncOperation PUT(string route, DataType dataType, string jsonData, int requestTimeout, List<HeaderKeysAndValue> headerKeysAndValues, Action<string> onSuccess = null, Action<int, string> onFailure = null, Action<int> onConnectionError = null)
            {
                #region Debug

                string headersAre = "";
                foreach (var item in headerKeysAndValues)
                {
                    headersAre += $"{item.key} : {item.value} ,";
                }
                Debug.Log($"Hitting [PUT] ::API:: {route} ::SendingData:: {jsonData} :: Headers Are {headersAre}");

                #endregion Debug

                byte[] bite = GetBites(jsonData, dataType);
                UnityWebRequest request = new UnityWebRequest(route, UnityWebRequest.kHttpVerbPUT)
                {
                    uploadHandler = new UploadHandlerRaw(bite),
                    downloadHandler = new DownloadHandlerBuffer(),
                };
                return CommonCallBack(request, route, headerKeysAndValues, onSuccess, onFailure, onConnectionError, requestTimeout);
            }

            public static UnityWebRequestAsyncOperation Delete(string route, DataType dataType, string jsonData, int requestTimeout, List<HeaderKeysAndValue> headerKeysAndValues, Action<string> onSuccess = null, Action<int, string> onFailure = null, Action<int> onConnectionError = null)
            {
                #region Debug

                string headersAre = "";
                foreach (var item in headerKeysAndValues)
                {
                    headersAre += $"{item.key} : {item.value} ,";
                }
                Debug.Log($"Hitting [DELETE] ::API:: {route} ::SendingData:: {jsonData} :: Headers Are {headersAre}");

                #endregion Debug

                byte[] bite = GetBites(jsonData, dataType);
                UnityWebRequest request = new UnityWebRequest(route, UnityWebRequest.kHttpVerbDELETE)
                {
                    uploadHandler = new UploadHandlerRaw(bite),
                    downloadHandler = new DownloadHandlerBuffer(),
                };
                return CommonCallBack(request, route, headerKeysAndValues, onSuccess, onFailure, onConnectionError, requestTimeout);
            }

            public static UnityWebRequestAsyncOperation Get(string route, DataType dataType, string jsonData, int requestTimeout, List<HeaderKeysAndValue> headerKeysAndValues, Action<string> onSuccess = null, Action<int, string> onFailure = null, Action<int> onConnectionError = null)
            {

                #region Debug

                string headersAre = "";
                foreach (var item in headerKeysAndValues)
                {
                    headersAre += $"{item.key} : {item.value} ,";
                }
                Debug.Log($"Hitting [GET] ::API:: {route} ::SendingData:: {jsonData} :: Headers Are {headersAre}");

                #endregion Debug

                byte[] bite = GetBites(jsonData, dataType);
                UnityWebRequest request = new UnityWebRequest(route, UnityWebRequest.kHttpVerbGET)
                {
                    uploadHandler = new UploadHandlerRaw(bite),
                    downloadHandler = new DownloadHandlerBuffer(),
                };
                return CommonCallBack(request, route, headerKeysAndValues, onSuccess, onFailure, onConnectionError, requestTimeout);
            }

            #region CommonCallBack

            private static byte[] GetBites(string jsonData, DataType dataType)
            {
                byte[] bite = null;
                if (DataType.Json == dataType)
                {
                    bite = System.Text.Encoding.UTF8.GetBytes(jsonData);
                }
                else if (dataType == DataType.Form)
                {
                    WWWForm form = ConvertJsonToWWWForm(jsonData);
                    bite = form.data;
                }
                else
                {
                    Debug.LogError("Wrong Data Type");
                    return null;
                }
                return bite;
            }
            private static UnityWebRequestAsyncOperation CommonCallBack(UnityWebRequest request, string route, List<HeaderKeysAndValue> headerKeysAndValues, Action<string> onSuccess, Action<int, string> onFailure, Action<int> onConnectionError, int requestTimeout)
            {
                if (headerKeysAndValues != null)
                {
                    foreach (var item in headerKeysAndValues)
                    {
                        request.SetRequestHeader(item.key, item.value);
                    }
                }
                request.timeout = requestTimeout;

                UnityWebRequestAsyncOperation asyncOperation = request.SendWebRequest();

                asyncOperation.completed += operation =>
                {
                    Debug.Log($"Hit Response [{request.method}] ::API:: {route} ::Result:: {request.result} ::ResponseCode:: {request.responseCode} ::ReceivedData:: {request.downloadHandler.text}");
                    if (request.result == UnityWebRequest.Result.ConnectionError)
                    {
                        onConnectionError?.Invoke(-1);
                    }
                    else if (request.result == UnityWebRequest.Result.Success)
                    {
                        onSuccess?.Invoke(request.downloadHandler.text);
                    }
                    else
                    {
                        onFailure?.Invoke((int)request.responseCode, request.downloadHandler.text);
                    }

                    request.Dispose();
                };
                return asyncOperation;
            }

            private static WWWForm ConvertJsonToWWWForm(string jsonString)
            {
                var wwwForm = new WWWForm();
                if (string.IsNullOrWhiteSpace(jsonString))
                {
                    return wwwForm;
                }
                JSONNode json = JSON.Parse(jsonString);

                foreach (KeyValuePair<string, JSONNode> pair in json.AsObject)
                {
                    wwwForm.AddField(pair.Key, pair.Value.Value);
                }

                return wwwForm;
            }
            #endregion
        }
    }
}