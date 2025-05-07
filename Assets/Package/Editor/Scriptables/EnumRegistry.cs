using EasyAPI.RunTime;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
namespace EasyAPI
{
    namespace Editor
    {
        [CreateAssetMenu(fileName = "EnumRegistry", menuName = "Easy API/Enum Registry", order = 1)]
        public class EnumRegistry : ScriptableObject
        {
            [SerializeField] private List<EnumData> enumEntries = new List<EnumData>();

            public bool GetEnumData(string enumFullName, out string fullPath)
            {
                try
                {
                    EnumData enumData = enumEntries.Find(e => e.enumFullName == enumFullName);
                    fullPath = GetFullPath(enumData.enumFile);
                    return true;
                }
                catch (Exception exception)
                {
                    Debug.LogError($"Failed to get path and class Name {exception}");
                    fullPath = null;
                    return false;
                }
            }

            public static string GetFullPath(MonoScript script)
            {
                if (script == null)
                {
                    Debug.LogError("Input MonoScript is null.");
                    return null;
                }

                string assetPath = AssetDatabase.GetAssetPath(script);

                if (string.IsNullOrEmpty(assetPath))
                {
                    Debug.LogWarning($"Could not find asset path for MonoScript '{script.name}'. It might not be a project asset file.");
                    return null;
                }

                try
                {
                    string projectRoot = Directory.GetCurrentDirectory();

                    string fullPath = Path.Combine(projectRoot, assetPath);

                    fullPath = Path.GetFullPath(fullPath);

                    return ToRelativeAssetPath(fullPath);
                }
                catch (System.Exception ex)
                {
                    Debug.LogError($"Error converting asset path '{assetPath}' to full path for script '{script.name}': {ex.Message}");
                    return null;
                }
            }
            public static string ToRelativeAssetPath(string fullPath)
            {
                int index = fullPath.IndexOf("Assets", System.StringComparison.OrdinalIgnoreCase);
                if (index >= 0)
                    return fullPath.Substring(index).Replace("\\", "/");
                else
                    throw new System.ArgumentException("Path does not contain 'Assets'");
            }
        }
    }
}