using EasyAPI.RunTime;
using UnityEngine;
namespace EasyAPI
{
    namespace Example
    {
        public class ExampleAPIHit : MonoBehaviour
        {
            private void Update()
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    APIManager.Instance.HitAPI<LoginData, UserAccount>(EndPoints.AccessLogin, new LoginData()
                    {
                        email = "pankaj.kumar@chicmicstudios.in",
                        password = "Pankaj$123"
                    }, null, (response) =>
                    {
                        if (response.success)
                        {
                            Debug.Log($"API Hit is successful, got json Data as  {JsonUtility.ToJson(response)}");
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
                    }, 
                    (value) =>
                    {
                        Debug.Log(value);
                    }
                    );
                }
                if (Input.GetMouseButtonDown(0))
                {
                    APIClass aPIClass = new APIClass(EndPoints.AccessLogin, new LoginData()
                    {
                        email = "pankaj.kumar@chicmicstudios.in",
                        password = "Pankaj$123"
                    });
                    aPIClass.AddResponseListener((response) =>
                    {
                        if (response.success)
                        {
                            Debug.Log($"API Hit is successful, got json Data as  {JsonUtility.ToJson(response)}");
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
                    aPIClass.AddProgressListener((value) =>
                    {
                        Debug.Log(value);
                    });
                    aPIClass.HitAPI();
                }
            }
        }
    }
}