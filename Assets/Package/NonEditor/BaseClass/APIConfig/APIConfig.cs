using UnityEngine;
namespace EasyAPI
{
    namespace RunTime
    {
        [CreateAssetMenu(fileName = "APIConfig", menuName = "Easy API/API Config", order = 3)]
        public class APIConfig : ScriptableObject
        {
            public string baseUrl;
            [Range(1, 100)]
            public int defaultRequestTimeout;
            [Range(1, 100)]
            public int defaultRetryCount;
        }
    }
}