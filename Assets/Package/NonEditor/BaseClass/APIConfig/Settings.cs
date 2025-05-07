using System.Collections.Generic;
using UnityEngine;
namespace EasyAPI
{
    namespace RunTime
    {
        [CreateAssetMenu(fileName = "APIData", menuName = "Easy API/API Manager", order = 2)]
        public class Settings : ScriptableObject
        {
            [SerializeField] private APIConfig apiConfig;
            [SerializeField] private List<RequestClass> endPoints;

            public APIConfig GetAPIConfig()
            {
                return apiConfig;
            }
            public List<RequestClass> GetEndPoints()
            {
                return endPoints;
            }

            public RequestClass GetRequestClass(EndPoints endPoint)
            {
                foreach (var item in endPoints)
                {
                    Debug.Log(item.endPoint +" "+endPoint.GetDisplayName());
                    if (item.endPoint == endPoint.GetDisplayName())
                    {
                        return item;
                    }
                }
                Debug.Log($"Could Not Find Request Class For End Point {endPoint}");
                return null;
            }
        }
    }
}