using System;
using UnityEngine;
using UnityEngine.Networking;
namespace EasyAPI
{
    namespace RunTime
    {
        public static class Requests
        {
            public static UnityWebRequestAsyncOperation Post(string route, string jsonData, int requestTimeout, Action<string> onSuccess = null, Action<int, string> onFailure = null, Action<int> onConnectionError = null)
            {
                Debug.Log($"Hitting [POST] ::API:: {route} ::SendingData:: {jsonData}");
                byte[] bite = System.Text.Encoding.UTF8.GetBytes(jsonData);
                UnityWebRequest request = new UnityWebRequest(route, UnityWebRequest.kHttpVerbPOST)
                {
                    uploadHandler = new UploadHandlerRaw(bite),
                    downloadHandler = new DownloadHandlerBuffer(),
                };
                return CommonCallBack(request, "POST", route, onSuccess, onFailure, onConnectionError, requestTimeout);
            }

            public static UnityWebRequestAsyncOperation PUT(string route, string jsonData, int requestTimeout, Action<string> onSuccess = null, Action<int, string> onFailure = null, Action<int> onConnectionError = null)
            {
                Debug.Log($"Hitting [PUT] ::API:: {route} ::SendingData:: {jsonData}");

                byte[] bite = System.Text.Encoding.UTF8.GetBytes(jsonData);
                UnityWebRequest request = new UnityWebRequest(route, UnityWebRequest.kHttpVerbPUT)
                {
                    uploadHandler = new UploadHandlerRaw(bite),
                    downloadHandler = new DownloadHandlerBuffer(),
                };

                return CommonCallBack(request, "PUT", route, onSuccess, onFailure, onConnectionError, requestTimeout);
            }

            public static UnityWebRequestAsyncOperation Delete(string route, string jsonData, int requestTimeout, Action<string> onSuccess = null, Action<int, string> onFailure = null, Action<int> onConnectionError = null)
            {
                Debug.Log($"Hitting [DELETE] ::API:: {route} ::SendingData:: {jsonData}");

                byte[] bite = System.Text.Encoding.UTF8.GetBytes(jsonData);

                UnityWebRequest request = new UnityWebRequest(route, UnityWebRequest.kHttpVerbDELETE)
                {
                    uploadHandler = new UploadHandlerRaw(bite),
                    downloadHandler = new DownloadHandlerBuffer(),
                };

                return CommonCallBack(request, "DELETE", route, onSuccess, onFailure, onConnectionError, requestTimeout);
            }

            public static UnityWebRequestAsyncOperation Get(string route, string jsonData, int requestTimeout, Action<string> onSuccess = null, Action<int, string> onFailure = null, Action<int> onConnectionError = null)
            {
                Debug.Log($"Hitting [Get] ::API:: {route} ::SendingData:: {jsonData}");
                byte[] bite = System.Text.Encoding.UTF8.GetBytes(jsonData);
                UnityWebRequest request = new UnityWebRequest(route, UnityWebRequest.kHttpVerbGET)
                {
                    uploadHandler = new UploadHandlerRaw(bite), // Set the upload handler with the given data
                    downloadHandler = new DownloadHandlerBuffer(), // Set the download handler to store the response data
                };
                return CommonCallBack(request, "GET", route, onSuccess, onFailure, onConnectionError, requestTimeout);
            }

            #region CommonCallBack
            private static UnityWebRequestAsyncOperation CommonCallBack(UnityWebRequest request, string type, string route, Action<string> onSuccess, Action<int, string> onFailure, Action<int> onConnectionError, int requestTimeout)
            {
                request.timeout = requestTimeout;

                UnityWebRequestAsyncOperation asyncOperation = request.SendWebRequest();

                asyncOperation.completed += operation =>
                {
                    Debug.Log($"Hit Response [GET] ::API:: {route} ::Result:: {request.result} ::ResponseCode:: {request.responseCode} ::ReceivedData:: {request.downloadHandler.text}");
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
            #endregion
        }
    }
}