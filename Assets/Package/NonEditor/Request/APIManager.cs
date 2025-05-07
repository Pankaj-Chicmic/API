using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
namespace EasyAPI
{
    namespace RunTime
    {
        public class APIManager : MonoBehaviourSingletonPersistent<APIManager>
        {
            [SerializeField] private Settings settings;

            public bool GetReponseTypeAndPayloadType(EndPoints endPoint, out PayLoadEnum payLoadEnum, out ResponseEnum responseEnum)
            {
                RequestClass requestClass = settings.GetRequestClass(endPoint);
                if (requestClass == null)
                {
                    payLoadEnum = PayLoadEnum.None;
                    responseEnum = ResponseEnum.Requestresponsebase;
                    return false;
                }
                payLoadEnum = requestClass.payLoadClass;
                responseEnum = requestClass.responseClass;
                return true;
            }

            public void HitAPI<T1, T2>(EndPoints endPoint, T1 payload, List<HeaderKeysAndValue> headerKeysAndValues, Action<T2> response = null, Action<float> progress = null) where T1 : RequestPayloadBase where T2 : RequestResponseBase, new()
            {
                try
                {
                    RequestClass requestClass = settings.GetRequestClass(endPoint);
                    if (requestClass == null)
                    {
                        HandleOtherResponse(-2, response, $"Request Class Not Found For End Point {endPoint}");
                        return;
                    }

                    if ((new T2()).GetType().Name != requestClass.responseClass.GetDisplayName())
                    {
                        HandleOtherResponse(-2, response, $"Wrong Response Type :: Expected {requestClass.responseClass.GetDisplayName()} Has :: {(new T2()).GetType().Name}");
                        return;
                    }

                    if (requestClass.payLoadClass == PayLoadEnum.None)
                    {
                        if (payload != null)
                        {
                            HandleOtherResponse(-2, response, $"Wrong PayloadType :: Expected None \"Null\" Has :: {payload.GetType().Name}");
                            return;
                        }
                    }
                    else
                    {
                        if (payload.GetType().Name != requestClass.payLoadClass.GetDisplayName())
                        {
                            HandleOtherResponse(-2, response, $"Wrong PayloadType :: Expected {requestClass.payLoadClass.GetDisplayName()}");
                            return;
                        }
                    }


                    string jsonData = payload == null ? " " : JsonUtility.ToJson(payload);
                    int retryRemaing = requestClass.retryInfo.overrideValue ? requestClass.retryInfo.overridenValue : settings.GetAPIConfig().defaultRetryCount;
                    int requestTimeout = requestClass.requestTimeout.overrideValue ? requestClass.requestTimeout.overridenValue : settings.GetAPIConfig().defaultRequestTimeout;

                    KeepSendingRequest(requestClass.requestTypes, requestTimeout, retryRemaing, requestClass.endPoint, jsonData, headerKeysAndValues, response, progress);
                }
                catch (Exception exception)
                {
                    HandleOtherResponse(-2, response, "API HIT FAILED " + exception);
                    return;
                }
            }
            private void KeepSendingRequest<T>(RequestTypes requestTypes, int requestTimeout, int retryRemaining, string endPoint, string jsonData, List<HeaderKeysAndValue> headerKeysAndValues, Action<T> response = null, Action<float> progress = null) where T : RequestResponseBase, new()
            {
                retryRemaining--;
                UnityWebRequestAsyncOperation unityWebRequest = SendRequest<T>(requestTypes, requestTimeout, endPoint, jsonData, headerKeysAndValues, (currResponse) =>
                {
                    if (!currResponse.success)
                    {
                        if (retryRemaining > 0)
                        {
                            KeepSendingRequest(requestTypes, requestTimeout, retryRemaining, endPoint, jsonData, headerKeysAndValues, response);
                        }
                        else
                        {
                            response?.Invoke(currResponse);
                        }
                    }
                    else
                    {
                        response?.Invoke(currResponse);
                    }
                });
                if (unityWebRequest != null && progress != null)
                {
                    StartCoroutine(DownloadProgress(unityWebRequest, progress));
                }
            }
            private UnityWebRequestAsyncOperation SendRequest<T>(RequestTypes requestTypes, int requestTimeout, string endPoint, string jsonData, List<HeaderKeysAndValue> headerKeysAndValues, Action<T> response) where T : RequestResponseBase, new()
            {
                UnityWebRequestAsyncOperation unityWebRequest = null;
                switch (requestTypes)
                {
                    case RequestTypes.GET:
                        unityWebRequest = Requests.Get(settings.GetAPIConfig().baseUrl + endPoint.ToString(), jsonData, requestTimeout, headerKeysAndValues,
                        (responseString) =>
                        {
                            HandleSuccessResponse(responseString, response);
                        },
                        (code, responseString) =>
                        {
                            HandleFailureResponse(code, responseString, response);
                        },
                        (code) =>
                        {
                            HandleOtherResponse(code, response);
                        }
                       );
                        break;
                    case RequestTypes.POST:
                        unityWebRequest = Requests.Post(settings.GetAPIConfig().baseUrl + endPoint.ToString(), jsonData, requestTimeout, headerKeysAndValues,
                            (responseString) =>
                            {
                                HandleSuccessResponse(responseString, response);
                            }, (code, responseString) =>
                            {
                                HandleFailureResponse(code, responseString, response);
                            }, (code) =>
                            {
                                HandleOtherResponse(code, response);
                            });
                        break;
                    case RequestTypes.PUT:
                        unityWebRequest = Requests.PUT(settings.GetAPIConfig().baseUrl + endPoint.ToString(), jsonData, requestTimeout, headerKeysAndValues,
                            (responseString) =>
                            {
                                HandleSuccessResponse(responseString, response);
                            }, (code, responseString) =>
                            {
                                HandleFailureResponse(code, responseString, response);
                            }, (code) =>
                            {
                                HandleOtherResponse(code, response);
                            });
                        break;
                    case RequestTypes.DELETE:
                        unityWebRequest = Requests.Delete(settings.GetAPIConfig().baseUrl + endPoint.ToString(), jsonData, requestTimeout, headerKeysAndValues,
                            (responseString) =>
                            {
                                HandleSuccessResponse(responseString, response);
                            }, (code, responseString) =>
                            {
                                HandleFailureResponse(code, responseString, response);
                            }, (code) =>
                            {
                                HandleOtherResponse(code, response);
                            });
                        break;
                }
                return unityWebRequest;
            }

            private void HandleFailureResponse<T>(int responseCode, string responseString, Action<T> response) where T : RequestResponseBase, new()
            {
                T responseData = new T();
                try
                {
                    responseData = JsonUtility.FromJson<T>(responseString);
                    responseData.success = false;
                    responseData.responseCode = responseCode;
                    responseData.failureMessage = "Failed From Backend";
                    response?.Invoke(responseData);
                }
                catch (Exception ex)
                {
                    Debug.LogWarning($"Json Not Parseable {ex}");
                    responseData.success = false;
                    response?.Invoke(responseData);
                }
            }

            private void HandleSuccessResponse<T>(string responseString, Action<T> response) where T : RequestResponseBase, new()
            {
                T responseData = new T();
                try
                {
                    responseData = JsonUtility.FromJson<T>(responseString);
                    responseData.success = true;
                    response?.Invoke(responseData);
                }
                catch (Exception ex)
                {
                    Debug.LogWarning($"Json Not Parseable {ex}");
                    responseData.success = true;
                    response?.Invoke(responseData);
                }
            }

            private void HandleOtherResponse<T>(int code, Action<T> response, string message = null) where T : RequestResponseBase, new()
            {
                T responseData = new T();
                responseData.success = false;
                responseData.failureMessage = message == null ? "Unknown Error" : message;
                responseData.responseCode = code;
                response?.Invoke(responseData);
            }


            private IEnumerator DownloadProgress(UnityWebRequestAsyncOperation operation, Action<float> progress)
            {
                while (!operation.isDone)
                {
                    progress?.Invoke(operation.progress);
                    yield return null;
                }
            }
        }
    }
}