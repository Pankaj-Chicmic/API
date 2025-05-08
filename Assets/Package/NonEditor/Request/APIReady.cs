using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace EasyAPI
{
    namespace RunTime
    {
        public class APIClass
        {
            #region Static

            private static MethodInfo apiHitMethod = null;
            private static MethodInfo GetAPIHitMethod()
            {
                if (apiHitMethod == null)
                {
                    apiHitMethod = typeof(APIManager).GetMethod(nameof(APIManager.Instance.HitAPI));
                }
                return apiHitMethod;
            }

            private static List<MethodStorage> methods = new List<MethodStorage>();
            private static MethodInfo GetGenericMethod(Type payloadClassType, Type responseClassType)
            {
                foreach (var item in methods)
                {
                    if (item.type1 == payloadClassType && item.type2 == responseClassType)
                    {
                        return item.method;
                    }
                }
                MethodInfo method = GetAPIHitMethod().MakeGenericMethod(payloadClassType, responseClassType);
                methods.Add(new MethodStorage(payloadClassType, responseClassType, method));
                return method;
            }

            #endregion Static

            private EndPoints endPoints;
            private Dictionary<string, string> queryParams;
            private List<HeaderKeysAndValue> headerKeysAndValues;
            private RequestPayloadBase payload;
            private Action<RequestResponseBase> gotResponse;
            private Action<float> progress;

            public APIClass(EndPoints endPoints, RequestPayloadBase payload = null, List<HeaderKeysAndValue> headerKeysAndValues = null, Action<RequestResponseBase> gotResponse = null, Action<float> progress = null)
            {
                ResetData(endPoints, payload, headerKeysAndValues, gotResponse, progress);
            }

            public void ResetData(EndPoints endPoints, RequestPayloadBase payload, List<HeaderKeysAndValue> headerKeysAndValues, Action<RequestResponseBase> response, Action<float> progress)
            {
                this.endPoints = endPoints;
                this.payload = payload;
                this.headerKeysAndValues = headerKeysAndValues;
                this.gotResponse = response;
                this.progress = progress;
            }

            public void ChangeRequestPayload(RequestPayloadBase requestPayloadBase)
            {
                this.payload = requestPayloadBase;
            }

            #region Params

            public void AddQueryParam(string key, string value)
            {
                if (queryParams == null)
                    queryParams = new Dictionary<string, string>();

                if (queryParams.ContainsKey(key))
                {
                    queryParams[key] = value;
                }
                else
                {
                    queryParams.Add(key, value);
                }
            }

            public void ClearQueryParams()
            {
                queryParams = new Dictionary<string, string>();
            }

            public void RemoveQueryParam(string key)
            {
                queryParams.Remove(key);
            }
            #endregion Params

            #region Header

            public void RemoveHeader(string key)
            {
                if (headerKeysAndValues == null)
                {
                    return;
                }
                var item = headerKeysAndValues.FirstOrDefault(h => h.key == key);
                if (item != null) headerKeysAndValues.Remove(item);
            }

            public void RemoveAllHeader()
            {
                headerKeysAndValues = null;
            }

            public void AddHeader(string key, string value)
            {
                if (headerKeysAndValues == null)
                    headerKeysAndValues = new List<HeaderKeysAndValue>();

                var existing = headerKeysAndValues.FirstOrDefault(h => h.key == key);
                if (existing != null)
                {
                    existing.value = value;
                }
                else
                {
                    headerKeysAndValues.Add(new HeaderKeysAndValue() { key = key, value = value });
                }
            }

            #endregion Header

            #region Progress Listener
            public void AddProgressListener(Action<float> progress)
            {
                this.progress += progress;
            }

            public void RemoveAllProgressListener()
            {
                this.progress = null;
            }

            public void RemoveProgressListener(Action<float> progress)
            {
                this.progress -= progress;
            }
            #endregion Progress Listener

            #region Response Listener
            public void AddResponseListener(Action<RequestResponseBase> gotResponse)
            {
                this.gotResponse += gotResponse;
            }

            public void RemoveAllResponseListener()
            {
                this.gotResponse = null;
            }

            public void RemoveResponseListener(Action<RequestResponseBase> gotResponse)
            {
                this.gotResponse -= gotResponse;
            }

            #endregion Response Listener

            public void HitAPI()
            {
                var apiManager = APIManager.Instance;
                ResponseEnum responseType;
                PayLoadEnum payloadType;
                if (!apiManager.GetResponseTypeAndPayloadType(endPoints, out payloadType, out responseType))
                {
                    Debug.LogError("Error Occurred see previous Log");
                }
                Type responseClassType = TypeFinder.FindTypeByName(responseType.GetDisplayName());
                if (responseClassType == null)
                {
                    Debug.LogError($"Response type '{responseType.GetDisplayName()}' could not be resolved.");
                    return;
                }
                Type payloadClassType = TypeFinder.FindTypeByName(payloadType.GetDisplayName());

                System.Reflection.MethodInfo genericMethod = null;

                if (genericMethod == null)
                {
                    if (payloadType != PayLoadEnum.None)
                    {
                        if (payloadClassType == null)
                        {
                            Debug.LogError($"Payload type '{payloadType.GetDisplayName()}' could not be resolved.");
                            return;
                        }
                        genericMethod = GetGenericMethod(payloadClassType, responseClassType);
                    }
                    else
                    {
                        if (payload != null)
                        {
                            Debug.LogWarning($"Payload should be null, processing with null");
                        }
                        genericMethod = GetGenericMethod(typeof(RequestPayloadBase), responseClassType);
                    }
                }

                Action<RequestResponseBase> callback = (RequestResponseBase response) =>
                {
                    gotResponse?.Invoke(response);
                };
                Action<float> _progress = (float value) =>
                {
                    progress?.Invoke(value);
                };
                genericMethod.Invoke(apiManager, new object[] { endPoints, payloadType == PayLoadEnum.None ? null : ConvertPayloadToType(payload, payloadClassType), headerKeysAndValues, callback, _progress, queryParams });
            }

            private object ConvertPayloadToType(RequestPayloadBase basePayload, Type targetType)
            {
                if (basePayload == null) return null;

                if (targetType.IsInstanceOfType(basePayload))
                {
                    return basePayload;
                }

                string json = JsonUtility.ToJson(basePayload);
                return JsonUtility.FromJson(json, targetType);
            }
        }
    }
}