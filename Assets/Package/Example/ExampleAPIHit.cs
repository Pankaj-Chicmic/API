using EasyAPI.Example;
using EasyAPI.RunTime;
using UnityEngine;

public class ExampleAPIHit : MonoBehaviour
{
    // -1 is Connection Error
    // -2 is API HIT Method Error
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            APIHandler.Instance.HitAPI<ExampleRequestPayload, ExampleResponsePayload>(EndPoints.Example, new ExampleRequestPayload(), (LoginResponse) =>
            {
                if (LoginResponse.success)
                {
                    Debug.Log($"API Hit is successfull, got json Data as  {JsonUtility.ToJson(LoginResponse)}");
                }
                else
                {
                    if (LoginResponse.responseCode == -1)
                    {
                        // This Means Error is due to Network
                        Debug.Log($"API Hit has Failed,\n Response Code is {LoginResponse.responseCode} \n Failure Message Is {LoginResponse.failureMessage} \n Json Is {JsonUtility.ToJson(LoginResponse)}");
                    }
                    else if (LoginResponse.responseCode == -2)
                    {
                        // This Means Error is due to API HIT METHOD CALL
                        Debug.Log($"API Hit has Failed,\n Response Code is  {LoginResponse.responseCode} \n Failure Message Is {LoginResponse.failureMessage} \n Json Is {JsonUtility.ToJson(LoginResponse)}");
                    }
                    else
                    {
                        // This Means Error has come from backend
                        Debug.Log($"API Hit has Failed,\n Response Code is {LoginResponse.responseCode} \n Failure Message Is {LoginResponse.failureMessage} \n Json Is {JsonUtility.ToJson(LoginResponse)}");
                    }
                }
            });
        }
    }
}
