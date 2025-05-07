using EasyAPI;
using EasyAPI.RunTime;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[Serializable]
public class RequestResponseEvent : UnityEvent<RequestResponseBase>
{
}
[RequireComponent(typeof(UnityEngine.UI.Button))]
public class APIOnButton : MonoBehaviour
{
    [SerializeField] private bool enableAPI;
    [SerializeField] private EndPoints endPoints;
    [SerializeField] private RequestResponseEvent gotResponse;
    [SerializeField] private UnityEvent<float> progress;
    [SerializeField] private RequestPayloadBase payload;

    public void SetRequestPayloadBase(RequestPayloadBase requestPayloadBase)
    {
        this.payload = requestPayloadBase;
    }

    public bool EnableAPI
    {
        set
        {
            enableAPI = value;
        }
        get
        {
            return enableAPI;
        }
    }
    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(HitAPI);
    }

    private void HitAPI()
    {
        if (enableAPI)
        {
            var apiManager = APIManager.Instance;
            ResponseEnum responseType;
            PayLoadEnum payloadType;
            if (!apiManager.GetReponseTypeAndPayloadType(endPoints, out payloadType, out responseType))
            {
                Debug.LogError("Error Occured see previous Log");
            }
            Type responseClassType = TypeFinder.FindTypeByName(responseType.GetDisplayName());
            if (responseClassType == null)
            {
                Debug.LogError($"Response type '{responseType.GetDisplayName()}' could not be resolved.");
                return;
            }

            System.Reflection.MethodInfo method = typeof(APIManager).GetMethod(nameof(apiManager.HitAPI));
            System.Reflection.MethodInfo genericMethod = null;

            Type payloadClasstype = TypeFinder.FindTypeByName(payloadType.GetDisplayName());
            if (payloadType != PayLoadEnum.None)
            {
                if (payloadClasstype == null)
                {
                    Debug.LogError($"Response type '{payloadType.GetDisplayName()}' could not be resolved.");
                    return;
                }
                genericMethod = method.MakeGenericMethod(payloadClasstype, responseClassType);
            }
            else
            {
                genericMethod = method.MakeGenericMethod(typeof(RequestPayloadBase), responseClassType);
            }

            Action<object> callback = (object obj) =>
            {
                var response = obj as RequestResponseBase;
                if (response == null)
                {
                    Debug.LogError("Failed to cast response.");
                    return;
                }
                gotResponse.Invoke(response);
            };
            Action<float> _progress = (float value) =>
            {
                progress?.Invoke(value);
            };
            genericMethod.Invoke(apiManager, new object[] { endPoints, payloadType == PayLoadEnum.None ? null : ConvertPayloadToType(payload, payloadClasstype), callback, _progress });
        }
    }
    private object ConvertPayloadToType(RequestPayloadBase basePayload, Type targetType)
    {
        if (basePayload == null) return null;

        if (targetType.IsInstanceOfType(basePayload))
        {
            return basePayload; // already of correct type
        }

        // Try JSON round-trip to convert
        string json = JsonUtility.ToJson(basePayload);
        return JsonUtility.FromJson(json, targetType);
    }

}
