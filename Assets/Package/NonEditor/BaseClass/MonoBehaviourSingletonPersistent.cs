
using UnityEngine;
namespace EasyAPI
{
    namespace RunTime
    {
        public class MonoBehaviourSingletonPersistent<T> : MonoBehaviour
            where T : MonoBehaviour
        {
            private static T _instance;
            public static T Instance
            {
                get
                {
                    if (_instance == null)
                    {
                        _instance = FindObjectOfType<T>();

                        if (_instance == null)
                        {
                            T[] prefabs = Resources.LoadAll<T>("Singletons");
                            if (prefabs.Length > 0)
                            {
                                _instance = GameObject.Instantiate(prefabs[0]).GetComponent<T>();
                            }
                            if (_instance == null)
                            {
                                GameObject obj = new GameObject(typeof(T).Name);
                                _instance = obj.AddComponent<T>();
                            }
                        }
                        DontDestroyOnLoad(_instance.gameObject);
                    }
                    return _instance;
                }
            }
            public virtual void Awake()
            {
                if (_instance == null)
                {
                    _instance = this as T;
                    DontDestroyOnLoad(this);
                }
                else if (_instance != this)
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}