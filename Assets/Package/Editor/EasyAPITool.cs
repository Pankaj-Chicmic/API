using EasyAPI.RunTime;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace EasyAPI
{
    namespace Editor
    {
        public static class EasyAPIMenus
        {
            private static EnumRegistry enumRegistry
            {
                get
                {
                    string[] guids = AssetDatabase.FindAssets($"t:{nameof(EnumRegistry)}");

                    if (guids.Length > 0)
                    {
                        if (guids.Length > 1)
                        {
                            Debug.LogError("Found More than one EnumRegistry asset in project. Please make sure there is only one.");
                            return null;
                        }
                        else
                        {
                            Debug.Log($"Found EnumRegistry asset in project. Loading...");
                            string path = AssetDatabase.GUIDToAssetPath(guids[0]);
                            return AssetDatabase.LoadAssetAtPath<EnumRegistry>(path);
                        }
                    }
                    else
                    {
                        Debug.LogWarning("No MyDataObject asset found in project.");
                        return null;
                    }
                }
            }

            private static Settings FindAPIDataAsset()
            {
                string[] guids = AssetDatabase.FindAssets($"t:{nameof(Settings)}");

                if (guids.Length > 1)
                {
                    Debug.LogError("Found more than one APIData asset. Please ensure there is only one.");
                    return null;
                }

                if (guids.Length == 1)
                {
                    string path = AssetDatabase.GUIDToAssetPath(guids[0]);
                    return AssetDatabase.LoadAssetAtPath<Settings>(path);
                }

                Debug.LogWarning("No APIData asset found.");
                return null;
            }

            [MenuItem("Tools/EasyAPI/Settings")]
            public static void SelectAPIData()
            {
                Settings asset = FindAPIDataAsset();
                if (asset != null)
                {
                    Selection.activeObject = asset;
                    EditorGUIUtility.PingObject(asset);
                    Debug.Log("Selected APIData asset.");
                }
            }

            [MenuItem("Tools/EasyAPI/Save")]
            public static void RegenerateAPIData()
            {
                foreach (var item in FindAPIDataAsset().GetEndPoints())
                {
                    foreach (var itemToTestWith in FindAPIDataAsset().GetEndPoints())
                    {
                        if (item.endPoint == itemToTestWith.endPoint && item != itemToTestWith)
                        {
                            Debug.LogError($"Failed to update enum. Can not have Two types of same endPoint.Duplicate EndPoint {item.endPoint}");
                            return;
                        }
                    }
                }
                string fullPath;
                if (enumRegistry.GetEnumData(nameof(PayLoadEnum), out fullPath))
                {
                    bool result = EnumInheritanceUpdater.UpdateEnumFromInheritance(nameof(PayLoadEnum), fullPath, nameof(RequestPayloadBase));
                    if (result)
                    {
                        Debug.Log($"Successfully updated enum '{nameof(PayLoadEnum)}' via static call.");
                    }
                    else
                    {
                        Debug.LogError($"Failed to update enum '{nameof(PayLoadEnum)}' via static call. Check previous logs for errors.");
                    }
                }
                else
                {
                    Debug.LogError($"Failed to update enum '{nameof(PayLoadEnum)}' via static call. Check previous logs for errors.");
                }
                if (enumRegistry.GetEnumData(nameof(ResponseEnum), out fullPath))
                {
                    bool result = EnumInheritanceUpdater.UpdateEnumFromInheritance(nameof(ResponseEnum), fullPath, nameof(RequestResponseBase));
                    if (result)
                    {
                        Debug.Log($"Successfully updated enum '{nameof(ResponseEnum)}' via static call.");
                    }
                    else
                    {
                        Debug.LogError($"Failed to update enum '{nameof(ResponseEnum)}' via static call. Check previous logs for errors.");
                    }
                }
                else
                {
                    Debug.LogError($"Failed to update enum '{nameof(PayLoadEnum)}' via static call. Check previous logs for errors.");
                }
                if (enumRegistry.GetEnumData(nameof(RunTime.EndPoints), out fullPath))
                {
                    List<string> namesOfEnums = new List<string>();
                    foreach (var requestClass in FindAPIDataAsset().GetEndPoints())
                    {
                        namesOfEnums.Add(requestClass.endPoint);
                    }
                    EnumAdder.UpdateEnumFile(fullPath, nameof(RunTime.EndPoints), namesOfEnums);
                }
                else
                {
                    Debug.LogError($"Failed to update enum '{nameof(RunTime.PayLoadEnum)}' via static call. Check previous logs for errors.");
                }
                AssetDatabase.Refresh();
            }

            [MenuItem("Tools/EasyAPI/API Hander")]
            public static void InstantiateMyPrefab()
            {
                const string APIHander = "APIHander";

                string[] guids = AssetDatabase.FindAssets($"{APIHander} t:Prefab");

                if (guids.Length == 0)
                {
                    Debug.LogWarning($"No prefab named '{APIHander}' found in the project.");
                    return;
                }

                if (guids.Length > 1)
                {
                    Debug.LogError($"Multiple prefabs named '{APIHander}' found. Please ensure there is only one.");
                    return;
                }

                string path = AssetDatabase.GUIDToAssetPath(guids[0]);
                GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);

                if (prefab == null)
                {
                    Debug.LogError("Failed to load prefab.");
                    return;
                }

                GameObject instance = PrefabUtility.InstantiatePrefab(prefab) as GameObject;

                if (instance != null)
                {
                    Undo.RegisterCreatedObjectUndo(instance, APIHander);
                    Selection.activeGameObject = instance;
                    Debug.Log($"Instantiated '{prefab.name}' in scene.");
                }
                else
                {
                    Debug.LogError("Failed to instantiate prefab.");
                }
            }
        }
    }
}