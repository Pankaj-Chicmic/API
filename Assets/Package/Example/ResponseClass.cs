using EasyAPI.RunTime;
using UnityEngine;
namespace EasyAPI
{
    namespace Example
    {
        [System.Serializable]
        public class ExampleResponsePayload : RequestResponseBase
        {
            public string exampleField;
        }

        [System.Serializable]
        public class UserAccount : RequestResponseBase
        {
            [SerializeField]
            public string uid;

            [SerializeField]
            public string email;

            [SerializeField]
            public bool emailVerified;

            [SerializeField]
            public bool disabled;
        }
    }
}