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
                    APIHitPublic();
                }
                if (Input.GetMouseButtonDown(0))
                {
                    APIHitPublic2();
                }
            }
            public void APIHitPublic()
            {
                APIManager.Instance.HitAPI<LoginData, UserAccount>(EndPoints.AccessLogin, new LoginData()
                {
                    email = "pankaj.kumar@chicmicstudios.in",
                    password = "Pankaj$123"
                }, null, ResponseListener,
                    Progress);
            }
            public void APIHitPublic2()
            {
                APIClass aPIClass = new APIClass(EndPoints.AccessLogin, new LoginData()
                {
                    email = "pankaj.kumar@chicmicstudios.in",
                    password = "Pankaj$123"
                });

                aPIClass.AddResponseListener(ResponseListener);
                aPIClass.AddProgressListener(Progress);
                aPIClass.HitAPI();
            }
            private void ResponseListener<T>(T response) where T : RequestResponseBase
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
            }

            private void Progress(float value)
            {
                Debug.Log(value);
            }
        }
    }
}