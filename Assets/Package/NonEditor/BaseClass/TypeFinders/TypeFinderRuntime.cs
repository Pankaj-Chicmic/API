using System;
using System.Collections.Generic;
using UnityEngine;

namespace EasyAPI
{
    public class TypeFinderRuntime
    {
        public static Dictionary<string, Type> cachedTypes = new Dictionary<string, Type>();
        
        public static Type FindTypeByName(string typeName)
        {
            if (!cachedTypes.ContainsKey(typeName))
                cachedTypes.Add(typeName, TypeFinder.FindTypeByName(typeName));
            return cachedTypes[typeName];
        }

        public static object CreateInstanceFromClassName(string className)
        {
            Type type = FindTypeByName(className);
            if (type != null)
            {
                // If it's a ScriptableObject
                if (typeof(ScriptableObject).IsAssignableFrom(type))
                    return ScriptableObject.CreateInstance(type);

                // If it's a regular class with a default constructor
                return Activator.CreateInstance(type);
            }

            throw new Exception($"Type '{className}' not found at runtime.");
        }
    }
}