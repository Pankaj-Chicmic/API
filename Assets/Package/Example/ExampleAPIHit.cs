using EasyAPI.RunTime;
using UnityEngine;
namespace EasyAPI
{
    namespace Example
    {
        public class ExampleAPIHit : MonoBehaviour
        {
            // -1 is Connection Error
            // -2 is API HIT Method Error

            private void Start()
            {
                FindFirstObjectByType<APIOnButton>().SetRequestPayloadBase(new LoginData()
                {
                    email = "pankaj.kumar@chicmicstudios.in",
                    password = "Pankaj$123"
                });
            }
            private void Update()
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    APIManager.Instance.HitAPI<XYZ, ExampleResponsePayload>(EndPoints.ComTodos, null, null, (response) =>
                    {
                        if (response.success)
                        {
                            Debug.Log($"API Hit is successfull, got json Data as  {JsonUtility.ToJson(response)}");
                        }
                        else
                        {
                            if (response.responseCode == -1)
                            {
                                // This Means Error is due to Network
                                Debug.Log($"API Hit has Failed,\n Response Code is {response.responseCode} \n Failure Message Is {response.failureMessage} \n Json Is {JsonUtility.ToJson(response)}");
                            }
                            else if (response.responseCode == -2)
                            {
                                // This Means Error is due to API HIT METHOD CALL
                                Debug.Log($"API Hit has Failed,\n Response Code is  {response.responseCode} \n Failure Message Is {response.failureMessage} \n Json Is {JsonUtility.ToJson(response)}");
                            }
                            else
                            {
                                // This Means Error has come from backend
                                Debug.Log($"API Hit has Failed,\n Response Code is {response.responseCode} \n Failure Message Is {response.failureMessage} \n Json Is {JsonUtility.ToJson(response)}");
                            }
                        }
                    });
                }
            }
            public void LoginResponse(RequestResponseBase requestResponseBase)
            {
                if (requestResponseBase is UserAccount loginResponse)
                {
                    Debug.Log(JsonUtility.ToJson(loginResponse));
                }
            }
            public void APIPRocessed(ValueFloat valueFloat)
            {
                Debug.Log(valueFloat.Value);
            }
        }
    }
}